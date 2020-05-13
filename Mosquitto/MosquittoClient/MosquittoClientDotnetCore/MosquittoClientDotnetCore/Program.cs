using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Dapper;
using Npgsql;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MosquittoClientDotnetCore
{
    class Program
    {
        private const int UPPER = 3600;
        private static  JsonSerializerOptions _jsonOptions = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
        private static List<long> results;
        private static MqttClient subClient;
        private static MqttClient pubClient;
         
        public static void Main(string[] args)
        {
            results = new List<long>();
            SubMqtt();
            PubMqtt();
            //Wait for results to completely fill
            Thread.Sleep(2000);
            using var writer = new StreamWriter("results.txt");
            foreach (var result in results)
            {
                writer.WriteLine(result);
            }
            subClient.Disconnect();
            pubClient.Disconnect();
        }

        private static void PubMqtt()
        {
            // create client instance 
            pubClient = new MqttClient("167.172.184.103");

            string clientId = Guid.NewGuid().ToString();
            pubClient.Connect(clientId);
            for (int i = 0; i < UPPER; i++)
            {
                var nowTicks = DateTime.Now.Ticks;
                string strValue = Convert.ToString("{\"Id\":\"30:AE:A4:DD:C9:18\", \"StartTicks\":\"" + nowTicks + "\"}");
                // publish a message on "/home/temperature" topic with QoS 2 
                pubClient.Publish("/cup/benchmark", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                Thread.Sleep(1000);
            }
            
        }

        private static void SubMqtt()
        {
            subClient = new MqttClient("167.172.184.103");
            subClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            subClient.Connect(clientId);
            
            subClient.Subscribe(new string[] {"/cup/benchmark/return"}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            switch (e.Topic)
            {
                case "/cup/locate":
                    Console.WriteLine(Encoding.UTF8.GetString(e.Message));
                    break;
                case "/cup/benchmark/return":
                    var nowTicks = DateTime.Now.Ticks;
                    var benchmark = JsonSerializer.Deserialize<Benchmark>(e.Message, _jsonOptions);
                    var elapsedTicks = nowTicks - Convert.ToInt64(benchmark.StartTicks);
                    results.Add(elapsedTicks);
                    break;
                default:
                    break;
            }

        }
    }
}