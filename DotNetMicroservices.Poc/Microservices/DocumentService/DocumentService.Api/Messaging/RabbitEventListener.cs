using System;
using System.Collections.Generic;
using EasyNetQ;
using EasyNetQ.Topology;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Api.Messaging;

public class RabbitEventListener
{
    private readonly IBus bus;
    private readonly IServiceProvider serviceProvider;

    public RabbitEventListener(IBus bus, IServiceProvider serviceProvider)
    {
        this.bus = bus;
        this.serviceProvider = serviceProvider;
    }

    public void ListenTo(List<Type> eventTypes)
    {
        foreach (var eventType in eventTypes)
        {
            GetType()
                .GetMethod("Subscribe", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.MakeGenericMethod(eventType)
                .Invoke(this, Array.Empty<object>());
        }
    }

    private void Subscribe<T>() where T : INotification
    {
        var exchange = bus.Advanced.ExchangeDeclare("lab-dotnet-micro", ExchangeType.Topic);
        var queue = bus.Advanced.QueueDeclare("lab-document-service-" + typeof(T).Name);
        bus.Advanced.Bind(exchange, queue, typeof(T).Name.ToLower());

        bus.Advanced.Consume(queue, (IMessage<T> msg, MessageReceivedInfo info) =>
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                return mediator.Publish(msg.Body);
            }
        });
    }
}
