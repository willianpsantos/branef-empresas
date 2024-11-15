using Branef.Empresas.Data.Entities;
using Branef.Empresas.DB;
using Branef.Empresas.Domain;
using Branef.Empresas.Domain.Interfaces.Base;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using Branef.Empresas.Domain.Queries;
using Branef.Empresas.Domain.QueryAdapters;
using Branef.Empresas.Domain.Validators;
using Branef.Empresas.Events;
using Branef.Empresas.Repositories;
using Branef.Empresas.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Branef.Empresas.Domain.Interfaces.Converters;
using Branef.Empresas.Domain.Converters;
using Branef.Empresas.Domain.EventMessages;
using MongoDB.Driver;

namespace Branef.Empresas.DependencyInjection
{
    public static class DomainServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainOpenApiSettings(this IServiceCollection services)
        {
            return services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
        }

        public static IServiceCollection AddDomainReadAndWriteDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<BranefWriteDbContext>(options =>
                {
                    var connectionString = configuration.GetConnectionString(ApplicationConstants.WriteDbConnectionStringName);
                    options.UseSqlServer(connectionString);
                })
                .AddDbContext<BranefReadDbContext>(options =>
                {
                    var connectionString = configuration.GetConnectionString(ApplicationConstants.ReadDbConnectionStringName);
                    var client = new MongoClient(connectionString);
                    options.UseMongoDB(client, "branef");
                });
        }

        public static IServiceCollection AddDomainInMemoryReadDbContext(this IServiceCollection services)
        {
            return services.AddDbContext<BranefWriteDbContext>(options =>
            {
                options.UseInMemoryDatabase(ApplicationConstants.InMemoryDatabaseName, options =>
                {
                    options.EnableNullChecks();
                });
            });
        }

        public static IServiceCollection AddDomainInMemoryWriteDbContext(this IServiceCollection services)
        {
            return services.AddDbContext<BranefReadDbContext>(options =>
            {
                options.UseInMemoryDatabase(ApplicationConstants.InMemoryDatabaseName, options =>
                {
                    options.EnableNullChecks();
                });
            });
        }

        public static IServiceCollection AddDomainQueryToExpressionAdapters(this IServiceCollection services)
        {
            return services
                .AddScoped<IQueryToExpressionAdapter<CompanyQuery, Company>, CompanyQueryToExpressionAdapter>();
        }

        public static IServiceCollection AddDomainsConverters(this IServiceCollection services)
        {
            return services
                .AddScoped<ICompanyDomainConverter, CompanyDomainConverter>();
        }

        public static IServiceCollection AddDomainRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IRepository<Company, BranefReadDbContext>, CompanyReadRepository>()
                .AddScoped<IRepository<Company, BranefWriteDbContext>, CompanyWriteRepository>();
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ICompanyReplicationService, CompanyReplicationService>()
                .AddScoped<ICompanyService<BranefReadDbContext>, CompanyReadService>()
                .AddScoped<ICompanyService<BranefWriteDbContext>, CompanyWriteService>();
        }

        public static IServiceCollection AddDomainModelValidators(this IServiceCollection services)
        {
            return services
                .AddScoped<IValidator<InsertOrUpdateCompanyCommand>, InsertOrUpdateCompanyCommandValidator>();
        }

        public static IServiceCollection AddMassTransitForDomainEvents(this IServiceCollection services, IConfiguration configuration)
        {
            var messageBrokerConfiguration = configuration.GetSection(ApplicationConstants.MessageBrokerConfiguratioSectionName);

            return services.AddMassTransit(options =>
            {
                options.AddConsumer<CompanyInsertedOrUpdatedConsumer>();
                options.AddConsumer<CompanyDeletedEventConsumer>();

                options.AddRequestClient<CompanyAddedOrUpdateEventMessage>();
                options.AddRequestClient<CompanyDeleteEventMessage>();

                options.SetKebabCaseEndpointNameFormatter();

                options.UsingRabbitMq((context, cfg) =>
                {
                    var host = messageBrokerConfiguration.GetValue<string>("Host");
                    var port = messageBrokerConfiguration.GetValue<ushort>("Port");
                    var username = messageBrokerConfiguration.GetValue<string>("Username");
                    var password = messageBrokerConfiguration.GetValue<string>("Password");

                    cfg.Host(host, port, "/", h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
