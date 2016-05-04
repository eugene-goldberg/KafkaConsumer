using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using RdKafka;
using KafkaNet;
using KafkaNet.Model;

namespace KafkaConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = new Config() { GroupId = "example-csharp-consumer" };
            //using (var consumer = new EventConsumer(config, "event-ora-dev.cloudapp.net"))
            //{
            //    consumer.OnMessage += (obj, msg) =>
            //    {
            //        string text = Encoding.UTF8.GetString(msg.Payload, 0, msg.Payload.Length);
            //        Console.WriteLine("Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {text}");
            //    };

            //    List<string> topics = new List<string>();
            //    topics.Add("topic1");

            //    consumer.Subscribe(topics);
            //    consumer.Start();

            //    Console.WriteLine("Started consumer, press enter to stop consuming");
            //    Console.ReadLine();
            //}

            var options = new KafkaOptions(new Uri("http://40.86.81.216:9092"), new Uri("http://40.86.81.216:9092"));
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions("topic1", router));

            //Consume returns a blocking IEnumerable (ie: never ending stream)
            foreach (var message in consumer.Consume())
            {
                Console.WriteLine("Response: P{0},O{1} : {2}",
                    message.Meta.PartitionId, message.Meta.Offset, System.Text.Encoding.Default.GetString(message.Value));

                //Console.WriteLine(message.Value.Length);
            }

        }
    }
}
