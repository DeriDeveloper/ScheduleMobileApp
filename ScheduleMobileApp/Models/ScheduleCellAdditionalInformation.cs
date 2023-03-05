using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleMobileApp.Models
{
    public class ScheduleCellAdditionalInformation
    {
        public int AfterNumberOfPair { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string Name { get; set; }
        public TimesPair TimesPair { get; set; }


    }
}
