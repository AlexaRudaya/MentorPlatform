using Mentors.Domain.Entities;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.BogusData
{
    public class CategoryGenerator
    {
        public Category GenerateFakeCategory()
        {
            return new Faker<Category>()
                .RuleFor(category => category.Id, faker => Guid.NewGuid())
                .RuleFor(category => category.Name, faker => faker.Commerce.Department())
                .RuleFor(category => category.Mentors, faker => GenerateFakeMentorsList())
                .Generate();
        }

        public Mentor GenerateFakeMentor()
        {
            return new Faker<Mentor>()
                .RuleFor(mentor => mentor.Id, faker => Guid.NewGuid())
                .RuleFor(mentor => mentor.Name, faker => faker.Person.FullName)
                .Generate();
        }

        private List<Mentor> GenerateFakeMentorsList()
        {
            var mentorsCount = new Random().Next(1, 5); 
            var mentors = new List<Mentor>();

            for (int i = 0; i < mentorsCount; i++)
            {
                mentors.Add(GenerateFakeMentor());
            }

            return mentors;
        }
    }
}