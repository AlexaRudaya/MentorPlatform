namespace Booking.Infrastructure.Data
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Bookings> Bookings { get; set; }

        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}