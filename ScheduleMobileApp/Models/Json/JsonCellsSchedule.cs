using System.Collections.Generic;

namespace ScheduleMobileApp.Models.Json
{
    public class JsonCellsSchedule
    {
        public class Root
        {
            public List<Models.CellSchedule> CellsSchedule { get; set; }
        }
    }
}
