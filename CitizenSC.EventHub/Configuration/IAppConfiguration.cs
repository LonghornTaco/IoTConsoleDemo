using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSC.EventHub.Configuration
{
   public interface IAppConfiguration
   {
      string IotHubUri { get; }
      string IotDeviceName { get; }
      string IotDeviceKey { get; }
   }
}
