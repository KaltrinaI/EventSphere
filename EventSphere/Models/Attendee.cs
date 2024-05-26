using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public class Attendee
    {
        [Key]
        public int AttendeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
