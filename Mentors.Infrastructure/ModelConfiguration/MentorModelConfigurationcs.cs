namespace Mentors.Infrastructure.ModelConfiguration
{
    public class MentorModelConfiguration : IEntityTypeConfiguration<Mentor>
    {
        public void Configure(EntityTypeBuilder<Mentor> builder)
        {
            builder
                .HasMany(mentor => mentor.Availabilities)
                .WithOne(availability => availability.Mentor)
                .HasForeignKey(availability => availability.MentorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}