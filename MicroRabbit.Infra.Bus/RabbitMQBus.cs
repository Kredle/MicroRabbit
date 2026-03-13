using System.Text;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.Infra.Bus;

public sealed class RabbitMQBus : IEventBus
{
    private readonly IServiceScopeFactory _scopeFactory;

    // eventName -> handlers
    private readonly Dictionary<string, List<Type>> _handlers = new();
    private readonly List<Type> _eventTypes = new();

    // keep consumer resources alive
    private readonly Dictionary<string, IConnection> _connections = new();
    private readonly Dictionary<string, IChannel> _channels = new();

    public RabbitMQBus(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SendCommand<T>(T command) where T : Command
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command).ConfigureAwait(false);
    }

    public async Task Publish<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var eventName = @event.GetType().Name;

        // Keep this identical to consumer declaration (matches your existing queue)
        await channel.QueueDeclareAsync(
            queue: eventName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: "", routingKey: eventName, body: body);
    }

    public async Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        // use the service interface type that is registered in DI
        var handlerServiceType = typeof(IEventHandler<T>);

        if (!_eventTypes.Contains(typeof(T)))
            _eventTypes.Add(typeof(T));

        if (!_handlers.ContainsKey(eventName))
            _handlers[eventName] = new List<Type>();

        if (_handlers[eventName].Any(h => h == handlerServiceType))
            throw new ArgumentException(
                $"Handler type {handlerServiceType.Name} already registered for '{eventName}'",
                nameof(handlerServiceType)
            );

        _handlers[eventName].Add(handlerServiceType);

        await StartBasicConsume<T>().ConfigureAwait(false);
    }

    private async Task StartBasicConsume<T>() where T : Event
    {
        var eventName = typeof(T).Name;

        if (_channels.ContainsKey(eventName))
            return; // already consuming

        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        _connections[eventName] = connection;
        _channels[eventName] = channel;

        await channel.QueueDeclareAsync(
            queue: eventName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += Consumer_Received;

        await channel.BasicConsumeAsync(
            queue: eventName,
            autoAck: false,
            consumer: consumer
        );
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);

            if (sender is AsyncEventingBasicConsumer consumer)
            {
                await consumer.Channel.BasicAckAsync(e.DeliveryTag, multiple: false).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            if (sender is AsyncEventingBasicConsumer consumer)
            {
                await consumer.Channel.BasicNackAsync(e.DeliveryTag, multiple: false, requeue: true).ConfigureAwait(false);
            }
        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        // write raw message for debugging
        try
        {
            File.AppendAllText("/tmp/rabbit_messages.log", $"[{DateTime.UtcNow:o}] EventName={eventName} Message={message}\n");
        }
        catch { }

        if (!_handlers.TryGetValue(eventName, out var subscribers))
            return;

        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
        if (eventType == null)
            return;

        var @event = JsonConvert.DeserializeObject(message, eventType);
        if (@event == null)
            return;

        using var scope = _scopeFactory.CreateScope();

        foreach (var subscriber in subscribers)
        {
            var handler = scope.ServiceProvider.GetService(subscriber);
            if (handler == null) continue;

            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
            var handle = concreteType.GetMethod("Handle");
            if (handle == null) continue;

            await (Task)handle.Invoke(handler, new[] { @event })!;
        }
    }
}
