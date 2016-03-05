using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Configuration;
using CitizenSC.WPF.Models;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace CitizenSC.WPF.Services
{
   public class IotEventHubService : IEventHubService
   {
      private readonly IAppConfiguration _config;

      public IotEventHubService()
      {
         _config = new IoTAppConfiguration();
      }

      public void SendMessage(SportData data)
      {
         var deviceClient = DeviceClient.Create(_config.UriString, new DeviceAuthenticationWithRegistrySymmetricKey(_config.IotDeviceName, _config.IotDeviceKey));

         var messageString = JsonConvert.SerializeObject(data);
         var message = new Message(Encoding.ASCII.GetBytes(messageString));

         try
         {
            deviceClient.SendEventAsync(message).Wait();
         }
         catch (Exception ex)
         {
         }
      }
   }
}
