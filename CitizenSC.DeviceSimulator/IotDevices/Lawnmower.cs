using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Logging;

namespace CitizenSC.DeviceSimulator.IotDevices
{
   public class Lawnmower : IDiagnosticController
   {
      private readonly ILogger _log;

      public event EventHandler<DeviceStoppedEventArgs> DeviceStopped;

      public bool IsDeviceRunning { get; private set; } = false;

      private DateTime _lastStartTime = DateTime.MinValue;
      private DateTime _lastStopTime = DateTime.MinValue;

      public Lawnmower(ILogger log)
      {
         _log = log;
      }

      public void Start()
      {
         ToggleState();
      }

      public void Stop()
      {
         _log.Log("Device Stopped");
         ToggleState();
      }

      private void OnDeviceStopped()
      {
         if (DeviceStopped != null)
            DeviceStopped(this, new DeviceStoppedEventArgs(_lastStopTime - _lastStartTime));
      }

      private void ToggleState()
      {
         IsDeviceRunning = !IsDeviceRunning;
         if (IsDeviceRunning)
            _lastStartTime = DateTime.Now;
         else
         {
            _lastStopTime = DateTime.Now;
            OnDeviceStopped();
         }
      }
   }
}
