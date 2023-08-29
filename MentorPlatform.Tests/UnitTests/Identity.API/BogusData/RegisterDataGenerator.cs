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
                .RuleFor(registerDto => registerDto.FirstName, faker => faker.Person.FirstName)
                .RuleFor(registerDto => registerDto.LastName, faker => faker.Person.LastName)
                .RuleFor(registerDto => registerDto.Email, faker => faker.Person.Email)
                .RuleFor(registerDto => registerDto.Password, faker => faker.Internet.GenerateCustomPassword());
        }

        public RegisterDto GenerateFakeData()
        {
            return _fakerRegisterDto.Generate();
        }
    }
}