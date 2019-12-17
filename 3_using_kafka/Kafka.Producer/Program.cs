using System;
using System.Collections.Generic;
using Confluent.Kafka;
using System.Text;
using System.Threading.Tasks;

namespace HelloServerless.Producer
{
    class Program
    {
        static string address = Environment.GetEnvironmentVariable("ADDRESS") ?? "kafka";
        static string port = Environment.GetEnvironmentVariable("PORT") ?? "9092";
        static string topicName = Environment.GetEnvironmentVariable("TOPICNAME") ?? "test-topic";

        static async Task Main(string[] args)
        {
            var tasks = new List<Task<bool>>();
            for (int i = 0; i < 1000; i++) {
                var text = "Message No: " + i;
                tasks.Add(SendMessage(i, text));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Finished 100 messages");

        }
        public static async Task<bool> SendMessage(int key, string message)
        {
            var result = false;
            var config = new ProducerConfig { BootstrapServers = address + ":" + port ,  };

            using (var p = new ProducerBuilder<int, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(topicName, new Message<int, string> { Key=key, Value=message });
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                    result = true;
                }
                catch (ProduceException<int, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
            return result;
        }
    }
}
