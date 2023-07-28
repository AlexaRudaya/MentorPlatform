namespace Mentors.Infrastructure.ModelConfiguration
{
    public class CategoryModelConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
               .HasMany(category => category.Mentors)
               .WithOne(mentor => mentor.Category);
        }
    }
}