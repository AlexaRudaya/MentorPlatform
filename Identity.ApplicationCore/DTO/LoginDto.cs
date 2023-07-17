namespace Identity.ApplicationCore.DTO
{
    public sealed class LoginDto
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}