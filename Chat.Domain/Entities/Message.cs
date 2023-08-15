namespace Chat.Domain.Entities
{
    public class Message : BaseEntity
    {
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}