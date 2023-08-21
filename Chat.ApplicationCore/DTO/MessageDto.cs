using Chat.Domain.Entities;

namespace Chat.ApplicationCore.DTO
{
    public class MessageDto : BaseDto
    {
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}