namespace Mentors.Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string? Name { get; set; }

        public List<Mentor>? Mentors { get; set; } = new();
    }
}