    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Confluent.Kafka;

namespace HelloServerless.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = Environment.GetEnvironmentVariable("ADDRESS") ?? "kafka";
            var port = Environment.GetEnvironmentVariable("PORT") ?? "9092";
            var groupId = Environment.GetEnvironmentVariable("GROUPID") ?? "test-consumers";
            var topicName = Environment.GetEnvironmentVariable("TOPICNAME") ?? "test-topic";

            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = address + ":" + port,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<int, string>(config).Build())
            {
                c.Subscribe(topicName);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Key}':'{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                            Thread.Sleep(500);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
    }
}
