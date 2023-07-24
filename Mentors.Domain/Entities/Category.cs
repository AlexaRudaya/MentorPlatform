namespace Mentors.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string? Name { get; set; }

        public List<Mentor>? Mentors { get; set; } = new();
    }
}