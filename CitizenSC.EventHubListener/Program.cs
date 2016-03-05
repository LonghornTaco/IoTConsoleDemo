using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Configuration;
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

         _config = new IoTAppConfiguration();
         _eventHubClient = EventHubClient.CreateFromConnectionString(_config.ConnectionString, _config.IotHubEndpoint);
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

            Print(string.Format("Message received.\n\tIdentifier:\t{0}\n\tRun Time:\t{1} hours\n\tDistance:\t{2} miles\n\tTime Stamp:\t{3}", statistics.Identifier, statistics.RunTime, statistics.Distance, statistics.TimeStamp));

            Print("Sending data to Sitecore");
            //using (var client = new HttpClient())
            //{
            //   // New code:
            //   client.BaseAddress = new Uri(_config.EntityServiceBaseUri);
            //   client.DefaultRequestHeaders.Accept.Clear();
            //   client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //   var runData = new RunStatistics()
            //   {
            //      Identifier = statistics.Identifier,
            //      RunTime = statistics.RunTime,
            //      Distance = statistics.Distance,
            //      TimeStamp = statistics.TimeStamp
            //   };
            //   var response = await client.PostAsJsonAsync<RunStatistics>(_config.EntityServicePostEndpoint, runData);

            //   if (response.IsSuccessStatusCode)
            //      Print("The message was successfully sent to Sitecore");
            //   else
            //      Print("There was a problem sending the data to Sitecore");
            //}
         }
      }

      private static void Print(string message)
      {
         Console.WriteLine(message);
      }
   }
}
