namespace Booking.Infrastructure.ModelConfiguration
{
    public class BookingsModelConfiguration : IEntityTypeConfiguration<Bookings>
    {
        public void Configure(EntityTypeBuilder<Bookings> builder)
        {
            builder
                .HasOne(bookings => bookings.Student)
                .WithMany(student => student.Bookings)
                .HasForeignKey(bookings => bookings.StudentId);
        }
    }
}