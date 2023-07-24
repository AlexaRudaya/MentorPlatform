namespace Mentors.Infrastructure.Data
{
    public class MentorDbContext : DbContext
    {
        public DbSet<Mentor> Mentors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Availability> Availabilities { get; set; }


        public MentorDbContext(DbContextOptions<MentorDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new MentorModelConfiguration())
                .ApplyConfiguration(new CategoryModelConfiguration());
        }
    }
}