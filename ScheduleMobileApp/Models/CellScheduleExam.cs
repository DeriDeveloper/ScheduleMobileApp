using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleMobileApp.Models
{
	public class CellScheduleExam
	{
		public int Id { get; set; }
		public Types.Enums.CellScheduleExamType CellScheduleExamType { get; set; }
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public string Title { get; set; }
		public int TeacherId { get; set; }
		public Teacher Teacher { get; set; }
		public int AudienceId { get; set; }
		public Audience Audience { get; set; }
		public DateTime Time { get; set; }
		public DateTime Date { get; set; }
	}
}
