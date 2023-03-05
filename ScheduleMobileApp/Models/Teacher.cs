using System.Collections.Generic;

namespace ScheduleMobileApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public string NameInitials
        {
            get
            {
                return $"{Surname} {(Name.Length > 0 ? Name[0].ToString().ToUpper() : "")}. {(Patronymic.Length > 0 ? Patronymic[0].ToString().ToUpper() : "")}."; 
            }
        }

    }
}
