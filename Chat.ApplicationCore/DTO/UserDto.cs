namespace Chat.ApplicationCore.DTO
{
    public class UserDto : BaseDto
    {
        public string Name { get; set; }

        public List<MessageDto> Messages { get; set; }
    }
}