using Event_Management.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.DAL.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Price)
                .HasPrecision(10, 2);

            builder.HasMany(t => t.Bookings)
                .WithOne(b => b.Ticket)
                .HasForeignKey(b => b.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
