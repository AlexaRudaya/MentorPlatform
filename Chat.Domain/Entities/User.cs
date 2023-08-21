namespace Chat.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public List<Message> Messages { get; set; }
    }
}