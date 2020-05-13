using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
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
        private ChannelReader<Config> _mqttConfigChannelReader;
        
        
        public MqttBackgroundService(IDbService dbService, Channel<Config> mqttConfigChannel)
        {
            _dbService = dbService;
            _mqttConfigChannelReader = mqttConfigChannel.Reader;
            
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new MqttClient("167.172.184.103");
            _client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            _clientId = Guid.NewGuid().ToString();
            _client.Connect(_clientId);
            // subscribe to the topic "/cup/connect" with QoS 2 
            _client.Subscribe(new string[] {Topics.CONNECT}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            _client.Subscribe(new string[] {Topics.DISCONNECT}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            _client.Subscribe(new string[] {Topics.TEMPERATURE}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            
            await foreach (var item in _mqttConfigChannelReader.ReadAllAsync(cancellationToken))
            {
                switch (item)
                {
                    case CupConfig c :
                        var strValue = JsonSerializer.Serialize(c);
                        _client.Publish("/cup/temprange", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                        break;
                    case LocateCup l :
                        var locateValue = JsonSerializer.Serialize(l);
                        _client.Publish("/cup/locate", Encoding.UTF8.GetBytes(locateValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                        break;
                    default:
                        break;
                }
                
            }
            

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
                    var connectedCup = _dbService.ConnectCup(jsonStr);
                    connectedCup = _dbService.GetCup(connectedCup.Id);
                    var cupConfig = Transform(connectedCup.Id, connectedCup.MaxTemp, connectedCup.MinTemp);
                    var strValue = JsonSerializer.Serialize(cupConfig);
                    _client.Publish(Topics.TEMPRANGE, Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                    break;
                case Topics.DISCONNECT:
                    var jsonString = Encoding.UTF8.GetString(e.Message);
                    _dbService.DisconnectCup(jsonString);
                    break;
                case Topics.TEST:
                    Console.WriteLine("Testing Endpoint Hit");
                    break;
                case Topics.TEMPERATURE:
                    var temperature = Encoding.UTF8.GetString(e.Message);
                    _dbService.InsertTemperature(temperature);
                    break;
                default:
                    break;
            }
        }
        
        private CupConfig Transform(string id, int maxtemp, int mintemp)
        {
            const double a = 0.0627918;
            const double b = -20.9698;
            var maxTempTransformed = (maxtemp - b) / a ;
            var minTempTransformed = (mintemp - b) / a ;
            return new CupConfig {Id = id, MaxTemp = (int) maxTempTransformed, MinTemp = (int) minTempTransformed};

        }
    }
}