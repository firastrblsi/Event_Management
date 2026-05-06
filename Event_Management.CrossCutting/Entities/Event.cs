using Event_Management.CrossCutting.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Event_Management.CrossCutting.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Draft;
        public int OrganizerId { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public Organizer Organizer { get; set; } = null!;
        [JsonIgnore]
        public Category Category { get; set; } = null!;
        [JsonIgnore]
        public Location Location { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        [JsonIgnore]
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
