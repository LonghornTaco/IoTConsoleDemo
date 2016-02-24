using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Configuration;

namespace CitizenSC.EventHub.Configuration
{
   public class IoTAppConfiguration : AppConfigurationBase, IAppConfiguration
   {
      public string IotHubUri { get { return GetValue<string>("IotHubUri"); } }
      public string IotDeviceName { get { return GetValue<string>("IotDeviceName"); } }
      public string IotDeviceKey { get { return GetValue<string>("IotDeviceKey"); } }
   }
}
