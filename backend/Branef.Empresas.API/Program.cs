using Branef.Empresas.DependencyInjection;
using Branef.Empresas.Domain;
using Serilog;
using Serilog.Events;

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



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales API V1");
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
