﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq.Shared.Messaging
{
    public abstract class MessageListenerBase : BackgroundService
    {
        protected readonly RabbitMqConfiguration Configuration;

        protected abstract string Subject { get; }
        protected abstract string Queue { get; }

        private ConnectionFactory _connectionFactory;
        private ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    _connectionFactory = new ()
                    {
                        HostName = Configuration.Hostname,
                        Port = Configuration.Port,
                        UserName = Configuration.UserName,
                        Password = Configuration.Password
                    };
                }
                
                return _connectionFactory;
            }
        }

        private IConnection Connection => ConnectionFactory.CreateConnection();

        protected IModel Channel { get; } 

        protected MessageListenerBase(IOptions<RabbitMqConfiguration> options)
        {
            Configuration = options.Value;

            Channel = Connection.CreateModel();
            Channel.QueueDeclare(queue: Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected abstract void HandleMessage(object sender, BasicDeliverEventArgs args);
    }
}