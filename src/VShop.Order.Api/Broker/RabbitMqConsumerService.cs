using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    // Can't inject a scoped service in a singleton service:
    //private readonly InventoryDbContext dbContext;

    private readonly IServiceProvider _serviceProvider;

    public RabbitMqConsumerService(IConnection connection, IServiceProvider serviceProvider)
    {
        // Tenho que usar a conexão do container de serviços pq se tentar criar outro com o factory, ele não vai se comunicar com a porta do container
        // Use this instead of a service Provider:
        _connection = connection;
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "product_queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        _serviceProvider = serviceProvider;        
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(message);

            var productId = data["ProductId"];
            // Process the message, e.g., fetch product details from the database
            // Use your existing context to get product details
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
                var product = await dbContext.Products.FindAsync(int.Parse(productId));

                if (product != null)
                {
                    Console.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Description: {product.Description}");
                }
                else
                {
                    Console.WriteLine($"Product with ID {productId} not found.");
                }
            }
        };

        _channel.BasicConsume(queue: "product_queue",
                             autoAck: true,
                             consumer: consumer);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
