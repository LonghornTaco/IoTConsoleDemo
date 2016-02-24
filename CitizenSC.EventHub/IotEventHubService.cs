using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Configuration;
using CitizenSC.Common.Logging;
using CitizenSC.EventHub.Configuration;
using CitizenSC.Model;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace CitizenSC.EventHub
{
   public class IotEventHubService : IEventHubService
   {
      private readonly ILogger _log;
      private readonly IAppConfiguration _config;

      public IotEventHubService(ILogger log)
      {
         _config = new IoTAppConfiguration();
         _log = log;
      }

      public void SendMessage(int runTime, int distance)
      {
         var deviceClient = DeviceClient.Create(_config.IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(_config.IotDeviceName, _config.IotDeviceKey));

         var messageString = JsonConvert.SerializeObject(new RunStatistics() { RunTime = runTime, Distance = distance });
         var message = new Message(Encoding.ASCII.GetBytes(messageString));

         try
         {
            _log.Log("Sending event to Azure");
            deviceClient.SendEventAsync(message).Wait();
            _log.Log("Message sent successfully!");
         }
         catch (Exception ex)
         {
            _log.Log("There was a problem sending the event to Azure:\n" + ex.Message);
         }
      }
   }
}
