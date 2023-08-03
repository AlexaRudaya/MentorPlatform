namespace Mentors.Infrastructure.Data
{
    public class MentorDbContext : DbContext
    {
        public DbSet<Mentor> Mentors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Availability> Availabilities { get; set; }


        public MentorDbContext(DbContextOptions<MentorDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}