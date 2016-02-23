using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace CitizenSC.DeviceSimulator
{
   class Program
   {
      static void Main(string[] args)
      {
         Console.WriteLine("###########################################");
         Console.WriteLine("##   Welcome to my Simulated IoT Device! ##");
         Console.WriteLine("##                                       ##");
         Console.WriteLine("##   Inputs:                             ##");
         Console.WriteLine("##      s - Start/Stop the device        ##");
         Console.WriteLine("##      x - Close the application        ##");
         Console.WriteLine("###########################################");

         ConsoleKeyInfo keyInfo;
         var keepApplicationRunning = true;
         var isDeviceOn = false;
         var lastStartTime = DateTime.MinValue;
         var lastStopTime = DateTime.MinValue;
         var yardsPerMinute = 176;

         do
         {
            keyInfo = Console.ReadKey();
            Console.WriteLine();

            switch (keyInfo.Key)
            {
               case ConsoleKey.S:
                  isDeviceOn = !isDeviceOn;
                  if (isDeviceOn)
                  {
                     Console.WriteLine("Device started");
                     lastStartTime = DateTime.Now;
                  }
                  else
                  {
                     Console.WriteLine("Device stopped");
                     lastStopTime = DateTime.Now;

                     var report = new RunReport();
                     if (lastStartTime < lastStopTime)
                        report.RunTime = lastStopTime - lastStartTime;
                     report.Distance = report.RunTime.Seconds * yardsPerMinute;

                     Console.WriteLine("Device ran for " + report.RunTime.Seconds + " minutes");
                     Console.WriteLine("Device traveled " + report.Distance + " yards");

                     SendReport(report);
                  }
                  break;
               case ConsoleKey.X:
                  if (isDeviceOn)
                     Console.WriteLine("You cannot close the application while the device is running.  Please stop the device first.");
                  else
                     keepApplicationRunning = false;
                  break;
               default:
                  Console.WriteLine("Invalid user input");
                  break;
            }

         } while (keepApplicationRunning);
      }

      static void SendReport(RunReport report)
      {
         var iotHubUri = "SitecoreIoTDemo.azure-devices.net";
         var deviceKey = "n5NR+pHWeEXN3IV0+acWoa72AtnRibSnYKKaHbgDMn4=";
         var deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("beau-Duino", deviceKey));

         var messageString = JsonConvert.SerializeObject(new { RunTime = report.RunTime.Seconds, Distance = report.Distance });
         var message = new Message(Encoding.ASCII.GetBytes(messageString));

         try
         {
            Console.WriteLine("Sending event to Azure");
            deviceClient.SendEventAsync(message).Wait();
            Console.WriteLine("Message sent successfully!");
         }
         catch (Exception ex)
         {
            Console.WriteLine("There was a problem sending the event to Azure:\n" + ex.Message);
         }
      }
   }

   public class RunReport
   {
      public TimeSpan RunTime { get; set; }
      public int Distance { get; set; }
   }
}
