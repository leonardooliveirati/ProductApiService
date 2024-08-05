using Confluent.Kafka;

namespace Product.Tests;

public class TestKafkaProducer
{
    private readonly string _bootstrapServers = "localhost:9092";
    private readonly string _topic = "Produtos";

    [Fact]
    public async Task TestProduceMessage()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var message = new Message<Null, string> { Value = "Test message" };

        try
        {
            var deliveryResult = await producer.ProduceAsync(_topic, message);
            Assert.Equal(_topic, deliveryResult.Topic);
            Assert.Equal(PersistenceStatus.Persisted, deliveryResult.Status);
        }
        catch (Exception ex)
        {
            Assert.True(false, $"Exception occurred: {ex.Message}");
        }
    }
}
