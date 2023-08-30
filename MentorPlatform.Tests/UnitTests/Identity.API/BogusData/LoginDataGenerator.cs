using Bogus;
using Identity.ApplicationCore.DTO;

namespace MentorPlatform.Tests.UnitTests.Identity.API.BogusData
{
    public class LoginDataGenerator
    {
        private Faker<LoginDto> _fakerLoginDto;

        public LoginDataGenerator()
        {
            _fakerLoginDto = new Faker<LoginDto>()
                .RuleFor(loginDto => loginDto.Email, faker => faker.Person.Email)
                .RuleFor(loginDto => loginDto.Password, faker => faker.Internet.GenerateCustomPassword());
        }

        public LoginDto GenerateFakeData()
        {
            return _fakerLoginDto.Generate();
        }
    }
}