using System.Threading.Channels;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class CupService : ICupService
    {
        private readonly IDbService _dbService;
        private readonly ChannelWriter<Config> _mqttConfigChannelWriter;

        public CupService(IDbService dbService, Channel<Config> mqttConfigChannel)
        {
            _dbService = dbService;
            _mqttConfigChannelWriter = mqttConfigChannel.Writer;

        }

        public void UpdateCup(string id, CupFormData cupUpdateData)
        {
            _dbService.UpdateCup(id, cupUpdateData);
            var cupConfig = Transform(id, cupUpdateData);
            _mqttConfigChannelWriter.WriteAsync(cupConfig);
        }

        public void LocateCup(string id)
        {
            var locateCup = new LocateCup {Id = id};
            _mqttConfigChannelWriter.WriteAsync(locateCup);
        }


        private CupConfig Transform(string id, CupFormData cupFormData)
        {
            const double a = 0.0627918;
            const double b = -20.9698;
            var maxTempTransformed = cupFormData.MaxTemp / a - b;
            var minTempTransformed = cupFormData.MinTemp / a - b;
            return new CupConfig {Id = id, MaxTemp = (int) maxTempTransformed, MinTemp = (int) minTempTransformed};

        }
    }
}