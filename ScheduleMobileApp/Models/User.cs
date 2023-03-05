namespace ScheduleMobileApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypePersonId { get; set; }
        public TypePerson  TypePerson { get; set; }
    }
}
