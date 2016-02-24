using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSC.EventHubListener.Configuration
{
   public interface IAppConfiguration
   {
      string IotHubConnectionString { get; }
      string IotHubEndpoint { get; }
   }
}
