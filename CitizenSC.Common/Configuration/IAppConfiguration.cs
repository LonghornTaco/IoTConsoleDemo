using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSC.Common.Configuration
{
   public interface IAppConfiguration
   {
      string IoTHubName { get; }
      string SharedAccessKeyName { get; }
      string SharedAccessKey { get; }
      string IotHubEndpoint { get; }

      string IotDeviceName { get; }
      string IotDeviceKey { get; }

      string ConnectionString { get; }
      string UriString { get; }
   }
}
