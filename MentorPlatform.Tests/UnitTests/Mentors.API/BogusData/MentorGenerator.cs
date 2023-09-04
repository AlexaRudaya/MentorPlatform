using Mentors.ApplicationCore.DTO;
using Mentors.Domain.Entities;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.BogusData
{
    public class MentorGenerator
    {
        private readonly CategoryGenerator _categoryGenerator;
        private readonly AvailabilityGenerator _availabilityGenerator;

        public MentorGenerator(
            CategoryGenerator categoryGenerator,
            AvailabilityGenerator availabilityGenerator)
        {
            _categoryGenerator = categoryGenerator;
            _availabilityGenerator = availabilityGenerator;
        }

        public Mentor GenerateFakeMentor()
        {
            return new Faker<Mentor>()
                .RuleFor(mentor => mentor.Id, faker => Guid.NewGuid())
                .RuleFor(mentor => mentor.Name, faker => faker.Person.FullName)
                .RuleFor(mentor => mentor.Biography, faker => faker.Lorem.Paragraph())
                .RuleFor(mentor => mentor.HourlyRate, faker => (double)faker.Finance.Amount(50, 150))
                .RuleFor(mentor => mentor.MeetingDuration, faker => faker.Random.Int(30, 90)) 
                .RuleFor(mentor => mentor.CategoryId, faker => Guid.NewGuid()) 
                .RuleFor(mentor => mentor.Category, faker => _categoryGenerator.GenerateFakeCategory())
                .RuleFor(mentor => mentor.Availabilities, faker => GenerateFakeAvailabilitiesList())
                .Generate();
        }

        public MentorCreateDto GenerateFakeDto()
        {
            return new Faker<MentorCreateDto>()
                .RuleFor(mentor => mentor.Name, faker => faker.Person.FullName)
                .RuleFor(mentor => mentor.Biography, faker => faker.Lorem.Paragraph())
                .RuleFor(mentor => mentor.HourlyRate, faker => (double)faker.Finance.Amount(50, 150))
                .RuleFor(mentor => mentor.MeetingDuration, faker => faker.Random.Int(30, 90))
                .RuleFor(mentor => mentor.CategoryId, faker => Guid.NewGuid())
                .RuleFor(mentor => mentor.Availabilities, faker => GenerateFakeAvailabilityDtosList())
                .Generate();
        }

        private List<Availability> GenerateFakeAvailabilitiesList()
        {
            var availabilitiesCount = new Random().Next(1, 5);
            var availabilities = new List<Availability>();

            for (int i = 0; i < availabilitiesCount; i++)
            {
                availabilities.Add(_availabilityGenerator.GenerateFakeAvailability());
            }

            return availabilities;
        }

        private List<AvailabilityDto> GenerateFakeAvailabilityDtosList()
        {
            var availabilitiesCount = new Random().Next(1, 5);
            var availabilities = new List<AvailabilityDto>();

            for (int i = 0; i < availabilitiesCount; i++)
            {
                availabilities.Add(_availabilityGenerator.GenerateFakeDto());
            }

            return availabilities;
        }
    }
}