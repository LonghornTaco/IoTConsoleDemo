using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.EventHubListener.Configuration;
using CitizenSC.Model;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace CitizenSC.EventHubListener
{
   class Program
   {
      private static IAppConfiguration _config;
      private static EventHubClient _eventHubClient;

      static void Main(string[] args)
      {
         Print("###############################################");
         Print("##                                           ##");
         Print("##   Welcome to my IoT Event Hub Listener!   ##");
         Print("##                                           ##");
         Print("###############################################");

         Print("\nPlease wait while the Receivers are initialized...\n");

         _config = new ListenerAppConfiguration();
         _eventHubClient = EventHubClient.CreateFromConnectionString(_config.IotHubConnectionString, _config.IotHubEndpoint);
         var partitions = _eventHubClient.GetRuntimeInformation().PartitionIds;
         foreach (string partition in partitions)
         {
            ReceiveMessagesFromDeviceAsync(partition);
         }

         Print("Receivers are ready!\n");
         Print("Press any key to stop the IoT Event Hub Listener.\n\n");
         Console.ReadLine();
      }

      private async static Task ReceiveMessagesFromDeviceAsync(string partition)
      {
         var eventHubReceiver = _eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.Now);
         while (true)
         {
            EventData eventData = await eventHubReceiver.ReceiveAsync();
            if (eventData == null)
               continue;

            string data = Encoding.UTF8.GetString(eventData.GetBytes());
            var statistics = JsonConvert.DeserializeObject<RunStatistics>(data);

            Print(string.Format("Message received.\n\tRun Time:\t{0} hours\n\tDistance:\t{1} miles\n", statistics.RunTime, statistics.Distance));
         }
      }

      private static void Print(string message)
      {
         Console.WriteLine(message);
      }
   }
}
