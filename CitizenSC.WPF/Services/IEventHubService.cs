using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenSC.WPF.Models;

namespace CitizenSC.WPF.Services
{
   public interface IEventHubService
   {
      void SendMessage(SportData data);
   }
}
