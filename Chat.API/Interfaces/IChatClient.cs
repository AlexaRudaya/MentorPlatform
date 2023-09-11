namespace Chat.API.Interfaces
{
    public interface IChatClient
    {
        Task ReceiveMessage(string userName, string content);
    }
}