namespace Mentors.ApplicationCore.DTO
{
    public abstract class BaseDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public Guid Id { get; set; }
    }
}