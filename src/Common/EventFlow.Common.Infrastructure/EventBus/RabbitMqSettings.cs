namespace EventFlow.Common.Infrastructure.EventBus;

// RabbitMQ connection settings used by MassTransit.
public sealed record RabbitMqSettings(
    string Host,
    string UserName = "guest",
    string Password = "guest");
