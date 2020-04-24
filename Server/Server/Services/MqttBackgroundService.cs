using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Models;
using Server.Services.Interfaces;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Server.Services
{
    public class MqttBackgroundService : IHostedService
    {
        private MqttClient _client;
        private string _clientId;
        private IDbService _dbService;
        
        public MqttBackgroundService(IDbService dbService)
        {
            _dbService = dbService;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new MqttClient("167.172.184.103");
            _client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            _clientId = Guid.NewGuid().ToString();
            _client.Connect(_clientId);
            // subscribe to the topic "/cup/connect" with QoS 2 
            _client.Subscribe(new string[] {Topics.CONNECT}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            _client.Subscribe(new string[] {Topics.DISCONNECT}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Disconnect();
            return Task.CompletedTask;
        }
        
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
        {
            switch (e.Topic)
            {
                case Topics.CONNECT:
                    var jsonStr = Encoding.UTF8.GetString(e.Message);
                    _dbService.ConnectCup(jsonStr);
                    break;
                case Topics.DISCONNECT:
                    var jsonString = Encoding.UTF8.GetString(e.Message);
                    _dbService.DisconnectCup(jsonString);
                    break;
                case Topics.TEST:
                    Console.WriteLine("Some donkey is doing testing");
                    break;
                case Topics.TEMPERATURE:
                    var temperature = Encoding.UTF8.GetString(e.Message);
                    _dbService.Temperature(temperature);
                    break;
                default:
                    break;
            }
        }
    }
}