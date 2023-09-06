namespace MentorPlatform.Tests.UnitTests.Mentors.API.BogusData
{
    public class AvailabilityGenerator
    {
        public Availability GenerateFakeAvailability()
        {
            return new Faker<Availability>()
                .RuleFor(availability => availability.Id, faker => Guid.NewGuid())
                .RuleFor(availability => availability.Date, faker => faker.Date.Soon())
                .RuleFor(availability => availability.IsAvailable, faker => true)
                .RuleFor(availability => availability.StartTime, faker => faker.Date.Future().Date.AddHours(faker.Random.Int(9, 11))) 
                .RuleFor(availability => availability.EndTime, (faker, availability) => availability.StartTime.AddHours(faker.Random.Int(2, 5))) // End time is from 2-5 hours after start time
                .RuleFor(availability => availability.MentorId, faker => Guid.NewGuid())
                .RuleFor(category => category.Mentor, faker => GenerateFakeMentor())
                .Generate();
        }

        public Mentor GenerateFakeMentor()
        {
            return new Faker<Mentor>()
                .RuleFor(mentor => mentor.Id, faker => Guid.NewGuid())
                .RuleFor(mentor => mentor.Name, faker => faker.Person.FullName)
                .Generate();
        }

        public AvailabilityDto GenerateFakeDto()
        {
            return new Faker<AvailabilityDto>()
                .RuleFor(availability => availability.Date, faker => faker.Date.Soon())
                .RuleFor(availability => availability.IsAvailable, faker => true)
                .RuleFor(availability => availability.StartTime, faker => faker.Date.Future().Date.AddHours(faker.Random.Int(9, 11)))
                .RuleFor(availability => availability.EndTime, (faker, availability) => availability.StartTime.AddHours(faker.Random.Int(2, 5))) 
                .RuleFor(availability => availability.MentorId, faker => Guid.NewGuid())
                .Generate();
        }
    }
}