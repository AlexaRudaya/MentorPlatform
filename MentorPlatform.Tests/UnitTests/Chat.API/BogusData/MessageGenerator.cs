namespace MentorPlatform.Tests.UnitTests.Chat.API.BogusData
{
    public class MessageGenerator
    {
        public Message GenerateFakeMessage()
        {
            return new Faker<Message>()
                .RuleFor(message => message.Id, user => Guid.NewGuid())
                .RuleFor(message => message.Content, faker => faker.Person.FullName)
                .RuleFor(message => message.Content, faker => faker.Lorem.Sentence())
                .Generate();
        }
    }
}