using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSC.Common.Configuration
{
   public class IoTAppConfiguration : IAppConfiguration
   {
      public string IoTHubName { get { return GetValue<string>("IoTHubName"); } }
      public string SharedAccessKeyName { get { return GetValue<string>("SharedAccessKeyName"); } }
      public string SharedAccessKey { get { return GetValue<string>("SharedAccessKey"); } }
      public string IotHubEndpoint { get { return GetValue<string>("IotHubEndpoint"); } }
      public string IotDeviceName { get { return GetValue<string>("IotDeviceName"); } }
      public string IotDeviceKey { get { return GetValue<string>("IotDeviceKey"); } }
      public string ConnectionStringFormat { get { return GetValue<string>("ConnectionStringFormat"); } }
      public string UriStringFormat { get { return GetValue<string>("UriStringFormat"); } }
      public string ConnectionString { get { return String.Format(ConnectionStringFormat, IoTHubName, SharedAccessKeyName, SharedAccessKey); } }
      public string UriString { get { return String.Format(UriStringFormat, IoTHubName); } }

      public string ContactIdentifier { get { return GetValue<string>("ContactIdentifier"); } }
      public string EntityServiceBaseUri { get { return GetValue<string>("EntityServiceBaseUri"); } }
      public string EntityServicePostEndpoint { get { return GetValue<string>("EntityServicePostEndpoint"); } }

      private T GetValue<T>(string key)
      {
         var returnValue = default(T);
         var converter = TypeDescriptor.GetConverter(typeof(T));
         if (converter != null)
         {
            object value = ConfigurationManager.AppSettings[key];
            if (value != null)
            {
               try
               {
                  returnValue = (T)converter.ConvertFrom(value);
               }
               catch (Exception)
               {
                  Trace.TraceError(String.Format("Failed trying to convert '{0}' to type '{1}'", value, key));
               }
            }
            else
               Trace.TraceError(String.Format("Could not find the config value '{0}'", key));
         }
         return returnValue;
      }
   }
}
