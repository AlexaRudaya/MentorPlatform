namespace Booking.Infrastructure.ModelConfiguration
{
    public class StudentModelConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                .HasMany(student => student.Bookings)
                .WithOne(booking => booking.Student);
        }
    }
}