using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleMobileApp.Models
{
	public class MobileApp
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Types.Enums.TypeMobileApp Type { get; set; }
		public double Version { get; set; }
	}
}
