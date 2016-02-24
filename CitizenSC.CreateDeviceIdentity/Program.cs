using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Configuration;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client.Exceptions;

namespace CitizenSC.CreateDeviceIdentity
{
   class Program
   {
      private static RegistryManager _registryManager;
      private static IAppConfiguration _config;

      static void Main(string[] args)
      {
         _config = new IoTAppConfiguration();
         _registryManager = RegistryManager.CreateFromConnectionString(_config.ConnectionString);

         string userInput = String.Empty;
         do
         {
            Print("\nEnter a name for the IoT device you'd like to register and press Enter");
            var deviceName = Console.ReadLine();
            if (!String.IsNullOrEmpty(deviceName))
               AddDeviceAsync(deviceName).Wait();
            Console.Write("\nWould you like to add another device? (y/n)\t");
            userInput = Console.ReadLine();
         } while (userInput != "n");
      }

      private async static Task AddDeviceAsync(string deviceId)
      {
         Device device = null;
         try
         {
            device = await _registryManager.GetDeviceAsync(deviceId);
            if (device == null)
               device = await _registryManager.AddDeviceAsync(new Device(deviceId));
         }
         catch (DeviceAlreadyExistsException ex)
         {
            device = await _registryManager.GetDeviceAsync(deviceId);
         }
         if (device != null)
            Print(String.Format("Device key for '{0}': {1}", deviceId, device.Authentication.SymmetricKey.PrimaryKey));
         else
            Print("Device was null");
      }
      private static void Print(string message)
      {
         Console.WriteLine(message);
      }
   }
}
