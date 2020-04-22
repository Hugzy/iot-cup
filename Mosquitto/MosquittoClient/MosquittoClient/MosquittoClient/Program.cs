using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MosquittoClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SubMqtt();
            PubMqtt();
        }

        private static void PubMqtt()
        {
            // create client instance 
            MqttClient client = new MqttClient("localhost"); 
             
            string clientId = Guid.NewGuid().ToString(); 
            client.Connect(clientId); 
             
            string strValue = Convert.ToString(205); 
             
            // publish a message on "/home/temperature" topic with QoS 2 
            client.Publish("/home/temperature", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false); 
        }

        private static void SubMqtt()
        {
            MqttClient client = new MqttClient("localhost");
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            client.Subscribe(new string[] {"/home/temperature"}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
        {
            Console.WriteLine("Received: " + Encoding.UTF8.GetString(e.Message));
        } 
    }
}