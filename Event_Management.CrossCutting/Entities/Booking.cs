using Event_Management.CrossCutting.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Event_Management.CrossCutting.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public DateTime? CancelledAt { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;
        [JsonIgnore]
        public Event Event { get; set; } = null!;
        [JsonIgnore]
        public Ticket Ticket { get; set; } = null!;
        [JsonIgnore]
        public Payment? Payment { get; set; }
    }
}
