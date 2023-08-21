namespace Chat.ApplicationCore.DTO
{
    public class MessageDto : BaseDto
    {
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public UserDto User { get; set; }
    }
}