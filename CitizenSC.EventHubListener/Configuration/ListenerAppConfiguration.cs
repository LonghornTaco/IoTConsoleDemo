using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Configuration;

namespace CitizenSC.EventHubListener.Configuration
{
   public class ListenerAppConfiguration : AppConfigurationBase, IAppConfiguration
   {
      public string IotHubConnectionString { get { return GetValue<string>("IotHubConnectionString"); } }
      public string IotHubEndpoint { get { return GetValue<string>("IotHubEndpoint"); } }
      
   }
}
