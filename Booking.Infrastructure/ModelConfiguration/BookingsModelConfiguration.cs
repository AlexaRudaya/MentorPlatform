﻿namespace Booking.Infrastructure.ModelConfiguration
{
    public class BookingsModelConfiguration : IEntityTypeConfiguration<MentorBooking>
    {
        public void Configure(EntityTypeBuilder<MentorBooking> builder)
        {
            builder
                .HasOne(bookings => bookings.Student)
                .WithMany(student => student.Bookings)
                .HasForeignKey(booking => booking.StudentId);
        }
    }
}