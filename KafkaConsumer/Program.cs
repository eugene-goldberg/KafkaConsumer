using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using Newtonsoft.Json;
using System.Diagnostics;
using KafkaNet.Common;  
using KafkaNet.Protocol; 


namespace KafkaConsumer
{
    class Program
    {

        public class MyModel
        {
            public int LOCATION_ID { get; set; }
            public string CITY { get; set; }
        }
        static void Main(string[] args)
        {
            var options = new KafkaOptions(new Uri("http://13.89.38.33:9092"), new Uri("http://13.89.38.33:9092"));
            var router = new BrokerRouter(options);

            
            var consumerOffset = new Consumer(new ConsumerOptions("ora1", router));

            //var topic = consumerOffset.GetTopic("topic1");



            var offsets = consumerOffset.GetTopicOffsetAsync("ora1").Result
                    .Select(x => new OffsetPosition(x.PartitionId, x.Offsets.Max())).ToArray();

            

            
            OffsetPosition position = new OffsetPosition();
            //in real life, we should store the last used offset in a database, retrieve it for each subsecquent restart of the consumer,
            //and update upon consumer exit, so the next time consumer starts, it will begin reading where it has left off
            position.Offset = 10;
            //using this will force the consumer to start reading from a specific explicitly defined position
            //var consumer = new Consumer(new ConsumerOptions("topic4", router), position);

            //using offsets will cause the consumer to start reading only newly sent messages
            var consumer = new Consumer(new ConsumerOptions("ora1", router),offsets);



            //Consume returns a blocking IEnumerable (ie: never ending stream)
            foreach (var message in consumer.Consume())
            {
                Console.WriteLine("Partition: {0},  Offset:  {1} : Data:   {2}",
                    message.Meta.PartitionId, message.Meta.Offset, System.Text.Encoding.Default.GetString(message.Value));
                Console.WriteLine("\n");

                string stringValue = System.Text.Encoding.Default.GetString(message.Value);

                var models = JsonConvert.DeserializeObject<IList<MyModel>>(stringValue);

                foreach (MyModel model in models)
                {
                    Console.WriteLine(model.LOCATION_ID);
                    Console.WriteLine(model.CITY);
                    Debug.WriteLine("MODEL:");
                    Debug.WriteLine(model.LOCATION_ID);
                    Debug.WriteLine(model.CITY);
                    Debug.WriteLine("MODEL END:");
                    Debug.WriteLine("\n\n");
                }

                //var jsonValue = JsonConvert.DeserializeObject(stringValue);


               
                //Debug.WriteLine(jsonValue);
                //Console.WriteLine(jsonValue);
                Console.WriteLine("\n\n");
                Console.WriteLine(DateTime.Now);
            }
        }
    }
}
