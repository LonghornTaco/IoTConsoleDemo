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
   public abstract class AppConfigurationBase
   {
      protected T GetValue<T>(string key)
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
