using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KafkaConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new KafkaOptions(new Uri("http://40.86.81.216:9092"), new Uri("http://40.86.81.216:9092"));
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions("topic4", router));

            //Consume returns a blocking IEnumerable (ie: never ending stream)
            foreach (var message in consumer.Consume())
            {
                //Console.WriteLine("Partition: {0},  Offset:  {1} : Data:   {2}",
                //    message.Meta.PartitionId, message.Meta.Offset, System.Text.Encoding.Default.GetString(message.Value));
                //Console.WriteLine("\n");

                string stringValue = System.Text.Encoding.Default.GetString(message.Value);

                var jsonValue = JsonConvert.DeserializeObject(stringValue);

                //Debug.WriteLine(stringValue);
                Debug.WriteLine(jsonValue);
                Console.WriteLine(jsonValue);
                Console.WriteLine("\n\n");
                Console.WriteLine(DateTime.Now);
            }
        }
    }
}
