using Branef.Empresas.API;
using Branef.Empresas.DependencyInjection;
using Branef.Empresas.Domain;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

string parentPath =
    Directory.GetParent(
        Directory.GetCurrentDirectory()
    )?.FullName ?? "";

var sharedSettingsPath = $"{parentPath}/{ApplicationConstants.SharedSettingsFileName}";

builder.Configuration.AddJsonFile(sharedSettingsPath);

builder.Services.AddCors(config =>
{
    config.AddPolicy(
        ApplicationConstants.CorsPolicyName,

        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );
});

builder
    .Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder
    .Services
    .AddDomainModelValidators()
    .AddDomainOpenApiSettings()
    .AddDomainReadAndWriteDbContext(builder.Configuration)
    .AddDomainRepositories()
    .AddDomainQueryToExpressionAdapters()
    .AddDomainsConverters()
    .AddDomainServices()
    .AddMassTransitForDomainEvents(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddSerilog(Log.Logger);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining(typeof(Branef.Empresas.CQRS.Entry));
});

builder
    .Services
    .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder
    .Services
    .AddApiVersioning()
    .AddApiExplorer();

builder.Services.AddSwaggerGen(options =>
{   
    options.OperationFilter<SwaggerDefaultValues>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(opt =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            opt.SwaggerEndpoint(url, name);
        }
    });
}

app.UseRouting();
app.UseCors(ApplicationConstants.CorsPolicyName);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "Handled {RequestPath}";
    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;
    
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});

app.Run();
