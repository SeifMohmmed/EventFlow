using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Infrastructure.Outbox;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Events.IntegrationEvents;
using EventFlow.Modules.Ticketing.Application.Abstractions.Authentication;
using EventFlow.Modules.Ticketing.Application.Abstractions.Data;
using EventFlow.Modules.Ticketing.Application.Abstractions.Payments;
using EventFlow.Modules.Ticketing.Application.Carts;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.Domain.Orders;
using EventFlow.Modules.Ticketing.Domain.Payments;
using EventFlow.Modules.Ticketing.Domain.Tickets;
using EventFlow.Modules.Ticketing.Infrastructure.Authentication;
using EventFlow.Modules.Ticketing.Infrastructure.Customers;
using EventFlow.Modules.Ticketing.Infrastructure.Database;
using EventFlow.Modules.Ticketing.Infrastructure.Events;
using EventFlow.Modules.Ticketing.Infrastructure.Inbox;
using EventFlow.Modules.Ticketing.Infrastructure.Orders;
using EventFlow.Modules.Ticketing.Infrastructure.Outbox;
using EventFlow.Modules.Ticketing.Infrastructure.Payments;
using EventFlow.Modules.Ticketing.Infrastructure.Tickets;
using EventFlow.Modules.Users.IntegrationEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace EventFlow.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    /// <summary>
    /// Registers the Ticketing module services.
    /// </summary>
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventHandlers();

        services.AddIntegrationEventHandlers();

        services.AddInfrastructure(configuration);

        // Discover and register the module's endpoints.
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    /// <summary>
    /// Registers the Ticketing module's message consumers.
    /// </summary>
    public static void ConfigureConsumers(
        IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserRegisteredIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserProfileUpdatedIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<EventPublishedIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<TicketTypePriceChangedIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<EventCancellationStartedIntegrationEvent>>();
    }

    /// <summary>
    /// Registers the infrastructure services for the Ticketing module.
    /// </summary>
    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register the Ticketing module DbContext and configure PostgreSQL,
        // migration history, domain event publishing, and snake_case naming.
        services.AddDbContext<TicketingDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(
                            HistoryRepository.DefaultTableName,
                            Schemas.Ticketing))
                // Publish domain events after a successful SaveChanges call.
                .AddInterceptors(
                    sp.GetRequiredService<InsertOutboxMessagesInterceptor>())
                // Convert table and column names to snake_case.
                .UseSnakeCaseNamingConvention());

        // Register repositories.
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        // Register the Unit of Work implementation.
        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<TicketingDbContext>());

        // Register the in-memory shopping cart service.
        services.AddSingleton<CartService>();

        services.AddSingleton<IPaymentService, PaymentService>();

        services.AddScoped<ICustomerContext, CustomerContext>();

        // Bind Ticketing module outbox settings from configuration.
        services.Configure<OutboxOptions>(
            configuration.GetSection("Ticketing:Outbox"));

        // Register Quartz configuration for the outbox processing job.
        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Ticketing:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();
    }

    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        // Discover all domain event handlers in the Application assembly.
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            // Register the original handler.
            services.TryAddScoped(domainEventHandler);

            // Determine the domain event handled by this handler.
            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            // Create the matching IdempotentDomainEventHandler<T>.
            Type closedIdempotentHandler =
                typeof(IdempotentDomainEventHandler<>)
                    .MakeGenericType(domainEvent);

            // Decorate the original handler with the idempotent wrapper.
            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlers = Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }
    }

}
