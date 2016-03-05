using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenSC.WPF.Models
{
   public class SportData
   {
      public string Identifier { get; set; }
      public string Sport { get; set; }
      public double Distance { get; set; }
      public int Duration { get; set; }
      public int AverageSpeed { get; set; }
      public string Timestamp { get; set; }

      public override string ToString()
      {
         var builder = new StringBuilder();
         builder.AppendFormat("\tIdentifier:\t{0}\n", Identifier);
         builder.AppendFormat("\tSport:\t\t{0}\n", Sport);
         builder.AppendFormat("\tDistance:\t\t{0}\n", Distance);
         builder.AppendFormat("\tDuration:\t{0}\n", Duration);
         builder.AppendFormat("\tAverage Speed:\t{0}\n", AverageSpeed);
         builder.AppendFormat("\tTime Stamp:\t{0}", Timestamp);
         return builder.ToString();
      }
   }
}
