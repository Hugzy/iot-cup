using System;
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
        public static void Main(string[] args)
        {
            SubMqtt();
            //PubMqtt();
        }

        private static void PubMqtt()
        {
            // create client instance 
            MqttClient client = new MqttClient("167.172.184.103"); 
             
            string clientId = Guid.NewGuid().ToString(); 
            client.Connect(clientId); 
             
            string strValue = Convert.ToString("{\"Id\":\"macMyAss\"}"); 
             
            // publish a message on "/home/temperature" topic with QoS 2 
            client.Publish("/cup/connect", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        private static void SubMqtt()
        {
            MqttClient client = new MqttClient("167.172.184.103");
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            client.Subscribe(new string[] {"/cup/connect"}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
        {
            switch (e.Topic)
            {
                case "/cup/connect":
                    var jsonStr = Encoding.UTF8.GetString(e.Message);
                    ConnectCup(jsonStr);
                    break;
                case "/test/mytopic":
                    Console.WriteLine("Some donkey is doing testing");
                    break;
                default:
                    break;
            }
           
        }

        private static string sqlCupInsert =
            "INSERT INTO tcup (id,display_name,connected) Values (@Id,'SomeRandomCupName',true)";

        private static string sqlCheckCup = "SELECT * FROM tcup WHERE id = @Id";
        private static string sqlCupConnected = "UPDATE tcup SET connected = true WHERE id = @Id";
        private static void ConnectCup(string jsonStr)
        {
            var cup = JsonSerializer.Deserialize<Cup>(jsonStr);
            using (var connection = GetDbConnection())
            {
                var existingCup = connection.QueryFirstOrDefault<Cup>(sqlCheckCup, cup);
                if (existingCup != null)
                {
                    var affectedRows = connection.Execute(sqlCupConnected, cup);
                }
                else
                {
                    var affectedRows = connection.Execute(sqlCupInsert, cup);
                }
            }
        }

        private static NpgsqlConnection GetDbConnection()
        {
            return new NpgsqlConnection("User ID=postgres;Password=dininfo1;Host=167.172.184.103;Database=postgres;Port=5432");
        }
        
        private class Cup
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public int MinTemp { get; set; }
            public int MaxTemp { get; set; }
            public bool Connected { get; set; }
        }
        
    }
}