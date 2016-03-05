using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSC.DeviceSimulator.IotDevices
{
   public class DeviceStoppedEventArgs : EventArgs
   {
      public string Identifier { get; private set; }
      public TimeSpan RunTime { get; private set; }
      public int Distance { get; private set; }
      public string TimeStamp { get; private set; }

      private int _milesPerHour = 6;

      public DeviceStoppedEventArgs(TimeSpan runTime, string identifier, string timeStamp)
      {
         Identifier = identifier;
         RunTime = runTime;
         Distance = runTime.Seconds * _milesPerHour;
         TimeStamp = timeStamp;
      }
   }
}
