namespace MentorPlatform.Tests.UnitTests.Mentors.API.BogusData
{
    public class MentorshipSubjectGenerator
    {
        public MentorshipSubject GenerateFakeMentorshipSubject() 
        {
            return new Faker<MentorshipSubject>()
                .RuleFor(subject => subject.Id, faker => faker.Random.String2(24))
                .RuleFor(subject => subject.Name, faker => faker.Commerce.Department())
                .Generate();
        }

        public MentorshipSubjectDto GenerateFakeDto()
        {
            return new Faker<MentorshipSubjectDto>()
                .RuleFor(subject => subject.Name, faker => faker.Lorem.Sentence(5, 15))
                .Generate();
        }
    }
}