namespace MentorPlatform.Tests.UnitTests.Chat.API.BogusData
{
    public class UserGenerator
    {
        public User GenerateFakeUser()
        {
            return new Faker<User>()
                .RuleFor(user => user.Id, user => Guid.NewGuid())
                .RuleFor(user => user.Name, faker => faker.Person.FullName)
                .Generate();
        }
    }
}