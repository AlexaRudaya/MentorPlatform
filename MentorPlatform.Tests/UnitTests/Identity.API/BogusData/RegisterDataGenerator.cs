using Bogus;
using Identity.ApplicationCore.DTO;

namespace MentorPlatform.Tests.UnitTests.Identity.API.BogusData
{
    public class RegisterDataGenerator
    {
        private Faker<RegisterDto> _fakerRegisterDto;

        public RegisterDataGenerator()
        {
            _fakerRegisterDto = new Faker<RegisterDto>()
                .RuleFor(registerDto => registerDto.FirstName, registerDto => registerDto.Person.FirstName)
                .RuleFor(registerDto => registerDto.LastName, registerDto => registerDto.Person.LastName)
                .RuleFor(registerDto => registerDto.Email, registerDto => registerDto.Internet.Email())
                .RuleFor(registerDto => registerDto.Password, registerDto => registerDto.Internet.Password(5, false, @"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}"));
        }

        public RegisterDto GenerateFakeData()
        {
            return _fakerRegisterDto.Generate();
        }
    }
}