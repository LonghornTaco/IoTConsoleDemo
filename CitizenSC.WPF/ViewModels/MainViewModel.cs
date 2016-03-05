using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.Common.Configuration;
using CitizenSC.Common.Logging;
using CitizenSC.Model;
using CitizenSC.WPF.Common;
using CitizenSC.WPF.Models;
using CitizenSC.WPF.Services;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace CitizenSC.WPF.ViewModels
{
   public class MainViewModel : INotifyPropertyChanged
   {
      #region Settings
      private string _IoTHubName;
      public string IoTHubName
      {
         get { return _IoTHubName; }
         set
         {
            _IoTHubName = value;
            OnPropertyChanged("IoTHubName");
         }
      }

      private string _SharedAccessKey;
      public string SharedAccessKey
      {
         get { return _SharedAccessKey; }
         set
         {
            _SharedAccessKey = value;
            OnPropertyChanged("SharedAccessKey");
         }
      }

      private string _IoTDeviceName;
      public string IoTDeviceName
      {
         get { return _IoTDeviceName; }
         set
         {
            _IoTDeviceName = value;
            OnPropertyChanged("IoTDeviceName");
         }
      }

      private string _IoTDeviceKey;
      public string IoTDeviceKey
      {
         get { return _IoTDeviceKey; }
         set
         {
            _IoTDeviceKey = value;
            OnPropertyChanged("IoTDeviceKey");
         }
      }

      private string _SharedAccessKeyName;
      public string SharedAccessKeyName
      {
         get { return _SharedAccessKeyName; }
         set
         {
            _SharedAccessKeyName = value;
            OnPropertyChanged("SharedAccessKeyName");
         }
      }

      private string _IoTHubEndpoint;
      public string IoTHubEndpoint
      {
         get { return _IoTHubEndpoint; }
         set
         {
            _IoTHubEndpoint = value;
            OnPropertyChanged("IoTHubEndpoint");
         }
      }

      private string _ConnectionString;
      public string ConnectionString
      {
         get { return _ConnectionString; }
         set
         {
            _ConnectionString = value;
            OnPropertyChanged("ConnectionString");
         }
      }

      private string _UriString;
      public string UriString
      {
         get { return _UriString; }
         set
         {
            _UriString = value;
            OnPropertyChanged("UriString");
         }
      }

      private string _EntityServiceBaseUri;
      public string EntityServiceBaseUri
      {
         get { return _EntityServiceBaseUri; }
         set
         {
            _EntityServiceBaseUri = value;
            OnPropertyChanged("EntityServiceBaseUri");
         }
      }

      private string _EntityServicePostEndpoint;
      public string EntityServicePostEndpoint
      {
         get { return _EntityServicePostEndpoint; }
         set
         {
            _EntityServicePostEndpoint = value;
            OnPropertyChanged("EntityServicePostEndpoint");
         }
      }
      #endregion

      #region Data
      private int _CyclingAverageSpeed = 10;
      public int CyclingAverageSpeed
      {
         get { return _CyclingAverageSpeed; }
         set
         {
            _CyclingAverageSpeed = value;
            OnPropertyChanged("CyclingAverageSpeed");
         }
      }

      private int _CyclingDistance = 15;
      public int CyclingDistance
      {
         get { return _CyclingDistance; }
         set
         {
            _CyclingDistance = value;
            OnPropertyChanged("CyclingDistance");
         }
      }

      private int _RunningDuration = 45;
      public int RunningDuration
      {
         get { return _RunningDuration; }
         set
         {
            _RunningDuration = value;
            OnPropertyChanged("RunningDuration");
         }
      }

      private int _RunningDistance = 5;
      public int RunningDistance
      {
         get { return _RunningDistance; }
         set
         {
            _RunningDistance = value;
            OnPropertyChanged("RunningDistance");
         }
      }

      private double _SwimmingDistance = 0.5;
      public double SwimmingDistance
      {
         get { return _SwimmingDistance; }
         set
         {
            _SwimmingDistance = value;
            OnPropertyChanged("SwimmingDistance");
         }
      }

      private ObservableCollection<string> _Contacts;
      public ObservableCollection<string> Contacts
      {
         get { return _Contacts; }
         set
         {
            _Contacts = value;
            OnPropertyChanged("Contacts");
         }
      }

      private string _SelectedCyclist;
      public string SelectedCyclist
      {
         get { return _SelectedCyclist; }
         set
         {
            _SelectedCyclist = value;
            OnPropertyChanged("SelectedCyclist");
         }
      }

      private string _SelectedRunner;
      public string SelectedRunner
      {
         get { return _SelectedRunner; }
         set
         {
            _SelectedRunner = value;
            OnPropertyChanged("SelectedRunner");
         }
      }

      private string _SelectedSwimmer;
      public string SelectedSwimmer
      {
         get { return _SelectedSwimmer; }
         set
         {
            _SelectedSwimmer = value;
            OnPropertyChanged("SelectedSwimmer");
         }
      }
      #endregion

      #region Logs
      private bool _AreButtonsActive = true;
      public bool AreButtonsActive
      {
         get { return _AreButtonsActive; }
         set
         {
            _AreButtonsActive = value;
            OnPropertyChanged("AreButtonsActive");
         }
      }

      private ObservableCollection<String> _SendLog = new ObservableCollection<string>();
      public ObservableCollection<String> SendLog
      {
         get { return _SendLog; }
         set
         {
            _SendLog = value;
            OnPropertyChanged("SendLog");
         }
      }

      private ObservableCollection<String> _ReceiveLog = new ObservableCollection<string>();
      public ObservableCollection<String> ReceiveLog
      {
         get { return _ReceiveLog; }
         set
         {
            _ReceiveLog = value;
            OnPropertyChanged("ReceiveLog");
         }
      }
      #endregion

      private IAppConfiguration _config;
      private IEventHubService _eventHubService;
      private EventHubClient _eventHubClient;

      public MainViewModel()
      {
         _config = new IoTAppConfiguration();
         _IoTHubName = _config.IoTHubName;
         _SharedAccessKey = _config.SharedAccessKey;
         _IoTDeviceName = _config.IotDeviceName;
         _IoTDeviceKey = _config.IotDeviceKey;
         _SharedAccessKeyName = _config.SharedAccessKeyName;
         _IoTHubEndpoint = _config.IotHubEndpoint;
         _ConnectionString = _config.ConnectionString;
         _UriString = _config.UriString;
         _EntityServiceBaseUri = _config.EntityServiceBaseUri;
         _EntityServicePostEndpoint = _config.EntityServicePostEndpoint;

         _eventHubService = new IotEventHubService();
         Contacts = new ObservableCollection<string>();
         Contacts.Add("john@runner.com");
         Contacts.Add("jane@swimmer.com");
         Contacts.Add("paul@cyclist.com");
         InitializeReceiver();
      }

      #region Commands
      private RelayCommand _SendCyclingData;
      public RelayCommand SendCyclingData
      {
         get
         {
            if (_SendCyclingData == null)
               _SendCyclingData = new RelayCommand(c => SendCyclingDataHandler());
            return _SendCyclingData;
         }
      }
      private void SendCyclingDataHandler()
      {
         SendData(new SportData()
         {
            Sport = "Cycling",
            Identifier = SelectedCyclist,
            AverageSpeed = CyclingAverageSpeed,
            Distance = CyclingDistance,
            Timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")
         });
      }

      private RelayCommand _SendRunningData;
      public RelayCommand SendRunningData
      {
         get
         {
            if (_SendRunningData == null)
               _SendRunningData = new RelayCommand(c => SendRunningDataHandler());
            return _SendRunningData;
         }
      }
      private void SendRunningDataHandler()
      {
         SendData(new SportData()
         {
            Sport = "Running",
            Identifier = SelectedRunner,
            Duration = RunningDuration,
            Distance = RunningDistance,
            Timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")
         });
      }

      private RelayCommand _SendSwimmingData;
      public RelayCommand SendSwimmingData
      {
         get
         {
            if (_SendSwimmingData == null)
               _SendSwimmingData = new RelayCommand(c => SendSwimmingDataHandler());
            return _SendSwimmingData;
         }
      }
      private void SendSwimmingDataHandler()
      {
         SendData(new SportData()
         {
            Sport = "Swimming",
            Identifier = SelectedSwimmer,
            Distance = SwimmingDistance,
            Timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")
         });
      }

      private RelayCommand _ClearSendLog;
      public RelayCommand ClearSendLog
      {
         get
         {
            if (_ClearSendLog == null)
               _ClearSendLog = new RelayCommand(c => ClearSendLogHandler());
            return _ClearSendLog;
         }
      }
      private void ClearSendLogHandler()
      {
         SendLog.Clear();
      }

      private RelayCommand _ClearReceiveLog;
      public RelayCommand ClearReceiveLog
      {
         get
         {
            if (_ClearReceiveLog == null)
               _ClearReceiveLog = new RelayCommand(c => ClearReceiveLogHandler());
            return _ClearReceiveLog;
         }
      }
      private void ClearReceiveLogHandler()
      {
         ReceiveLog.Clear();
      }
      #endregion

      private void InitializeReceiver()
      {
         _eventHubClient = EventHubClient.CreateFromConnectionString(_config.ConnectionString, _config.IotHubEndpoint);
         var partitions = _eventHubClient.GetRuntimeInformation().PartitionIds;
         foreach (string partition in partitions)
         {
            ReceiveMessagesFromDeviceAsync(partition);
         }
      }
      private async Task ReceiveMessagesFromDeviceAsync(string partition)
      {
         var eventHubReceiver = _eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.Now);
         while (true)
         {
            EventData eventData = await eventHubReceiver.ReceiveAsync();
            if (eventData == null)
               continue;

            string jsonData = Encoding.UTF8.GetString(eventData.GetBytes());
            var sportData = JsonConvert.DeserializeObject<SportData>(jsonData);

            ReceiveLog.Add("Message received...");
            ReceiveLog.Add(sportData.ToString());

            ReceiveLog.Add("Sending data to Sitecore");
            using (var client = new HttpClient())
            {
               client.BaseAddress = new Uri(_config.EntityServiceBaseUri);
               client.DefaultRequestHeaders.Accept.Clear();
               client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

               var response = await client.PostAsJsonAsync<SportData>(_config.EntityServicePostEndpoint, sportData);

               if (response.IsSuccessStatusCode)
                  ReceiveLog.Add("The message was successfully sent to Sitecore");
               else
                  ReceiveLog.Add("There was a problem sending the data to Sitecore");
            }
         }
      }

      private void SendData(SportData data)
      {
         if (String.IsNullOrEmpty(data.Identifier))
            SendLog.Add("You must select a Contact");
         else
         {
            AreButtonsActive = false;
            using (var worker = new BackgroundWorker())
            {
               worker.WorkerReportsProgress = true;
               worker.ProgressChanged += (o, e) => { SendLog.Add(e.UserState.ToString()); };
               worker.RunWorkerCompleted += (o, e) => { AreButtonsActive = true; };
               worker.DoWork += (o, e) =>
               {
                  worker.ReportProgress(0, "Sending the following data to Azure:");
                  worker.ReportProgress(0, data.ToString());
                  _eventHubService.SendMessage(data);
                  worker.ReportProgress(0, "Data sent successfully!");
               };
               worker.RunWorkerAsync();
            }
         }
      }

      #region INotifyPropertyChanged Stuff
      public event PropertyChangedEventHandler PropertyChanged;
      protected virtual void OnPropertyChanged(string propertyName)
      {
         if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      } 
      #endregion
   }
}
