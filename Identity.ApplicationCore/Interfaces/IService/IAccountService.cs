namespace Identity.ApplicationCore.Interfaces.IService
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);

        Task LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    }
}