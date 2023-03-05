using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleMobileApp.Types
{
    public class Enums
    {
        public enum CellScheduleType : int
        {
            common,
            numerator,
            denominator,
        }
        public enum CellScheduleExamType : int
        {
			Сonsultation,
			Exam
		}
        public enum TypeMobileApp : int
        {
			Android,
			Ios
		}
    }
}
