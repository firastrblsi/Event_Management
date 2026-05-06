using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.DTOs.Booking
{
    public class CreateBookingRequest
    {
        public int EventId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
