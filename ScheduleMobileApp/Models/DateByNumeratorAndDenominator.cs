using System;

namespace ScheduleMobileApp.Models
{
    public class DateByNumeratorAndDenominator
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Types.Enums.CellScheduleType TypeCellSchedule { get; set; }
    }
}
