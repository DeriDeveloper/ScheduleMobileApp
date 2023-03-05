using System;
using System.Collections.Generic;
using ScheduleMobileApp.Interfaces;

namespace ScheduleMobileApp.Models
{
    public class CellSchedule
    {
        public int Id { get; set; }
        public int NumberPair { get; set; }
        public int TimesPairId { get; set; }
        public TimesPair TimesPair { get; set; }
        public ICollection<AcademicSubject> AcademicSubjects { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Audience> Audiences { get; set; }
        public ICollection<Group> Groups { get; set; }
        public Types.Enums.CellScheduleType TypeCell { get; set; } = Types.Enums.CellScheduleType.common;
        public bool IsChange { get; set; }


        // change cell
        public DateTime? Date { get; set; }
    }
}
