using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Outbox
{
    public class RabbitMqOutboxPublisher : IOutboxPublisher, IAsyncDisposable
    {
        private const string ExchangeName = "personal-finance.events";

        private readonly ConnectionFactory _connectionFactory;
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public RabbitMqOutboxPublisher(string connectionString)
        {
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionString),
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true,
                ClientProvidedName = "personal-finance-identity-service"
            };
        }

        public async Task PublishAsync(string type, string content, CancellationToken cancellationToken = default)
        {
            await EnsureConnectionAsync(cancellationToken);

            var body = Encoding.UTF8.GetBytes(content);

            var properties = new BasicProperties
            {
                ContentType = "application/json",
                Type = type,
                Persistent = true,
                MessageId = Guid.NewGuid().ToString()
            };

            await _channel!.BasicPublishAsync(exchange: ExchangeName, routingKey: type, mandatory: false, basicProperties: properties, body: body, cancellationToken: cancellationToken);
        }

        private async Task EnsureConnectionAsync(CancellationToken cancellationToken)
        {
            if (_connection is not null && _connection.IsOpen && _channel is not null && _channel.IsOpen)
            {
                return;
            }

            await _lock.WaitAsync(cancellationToken);

            try
            {
                if (_connection is not null && _connection.IsOpen && _channel is not null && _channel.IsOpen)
                {
                    return;
                }

                _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

                var channelOptions = new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: true);

                _channel = await _connection.CreateChannelAsync(channelOptions, cancellationToken);

                await _channel.ExchangeDeclareAsync(exchange: ExchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: cancellationToken);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
            {
                await _channel.DisposeAsync();
            }

            if (_connection is not null)
            {
                await _connection.DisposeAsync();
            }

            _lock.Dispose();
        }
    }
}
