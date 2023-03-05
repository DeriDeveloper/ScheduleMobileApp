using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleMobileApp.Models.Json
{
    public class JsonScheduleCellsAdditionalInformation
    {
        public class Root
        {
            public List<Models.ScheduleCellAdditionalInformation> CellsScheduleAdditionalInformation { get; set; }
        }
    }
}
