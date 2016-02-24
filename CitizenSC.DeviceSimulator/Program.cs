using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Configuration;
using CitizenSC.Common.Logging;
using CitizenSC.DeviceSimulator.IotDevices;
using CitizenSC.EventHub;

namespace CitizenSC.DeviceSimulator
{
   class Program
   {
      private static IDiagnosticController _diagnosticController;
      private static IEventHubService _eventHubService;
      private static ILogger _log;

      static void Main(string[] args)
      {
         Initialize();

         ConsoleKeyInfo keyInfo;
         var keepApplicationRunning = true;

         _diagnosticController.DeviceStopped += _diagnosticController_DeviceStopped;

         do
         {
            keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
               case ConsoleKey.S:
                  if (!_diagnosticController.IsDeviceRunning)
                  {
                     Print("\nDevice Started");
                     _diagnosticController.Start();
                  }
                  else
                  {
                     Print("Device Stopped");
                     _diagnosticController.Stop();
                  }
                  break;
               case ConsoleKey.X:
                  if (_diagnosticController.IsDeviceRunning)
                     Print("You cannot close the application while the device is running.  Please stop the device first.");
                  else
                     keepApplicationRunning = false;
                  break;
               default:
                  Print("Invalid user input");
                  break;
            }

         } while (keepApplicationRunning);
      }

      private static void _diagnosticController_DeviceStopped(object sender, DeviceStoppedEventArgs e)
      {
         Print("\tRun Time:\t" + e.RunTime.Seconds + " hours");
         Print("\tDistance:\t" + e.Distance + " miles");
         _eventHubService.SendMessage(e.RunTime.Seconds, e.Distance);
      }

      private static void Initialize()
      {
         _log = new IotLogger();
         _diagnosticController = new Lawnmower(_log);
         _eventHubService = new IotEventHubService(_log);

         Print("#############################################");
         Print("##   Welcome to my Simulated IoT Device!   ##");
         Print("##                                         ##");
         Print("##   Inputs:                               ##");
         Print("##      s - Start/Stop the device          ##");
         Print("##      x - Close the application          ##");
         Print("#############################################");
      }

      private static void Print(string message)
      {
         Console.WriteLine(message);
      }
   }
}
