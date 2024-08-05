using Confluent.Kafka;
using System;
using System.Threading.Tasks;

public class KafkaTest
{
    public static async Task Main(string[] args)
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        try
        {
            var deliveryResult = await producer.ProduceAsync("Produtos", new Message<Null, string> { Value = "Test message" });
            Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}
