using Booking.Domain.Entities;

namespace MentorPlatform.Tests.UnitTests.Booking.API.BogusData
{
    public class BookingGenerator
    {
        public MentorBooking GenerateFakeBooking()
        {
            return new Faker<MentorBooking>()
                .RuleFor(booking => booking.Id, faker => Guid.NewGuid())
                .RuleFor(booking => booking.StartTimeBooking, faker => faker.Date.Future().Date.AddHours(faker.Random.Int(7, 10)))
                .RuleFor(booking => booking.EndTimeBooking, (faker, booking) => booking.StartTimeBooking.AddHours(faker.Random.Int(3, 4)))
                .RuleFor(booking => booking.StudentId, faker => Guid.NewGuid())
                .RuleFor(booking => booking.Student, faker => GenerateFakeStudent())
                .RuleFor(booking => booking.MentorId, faker => faker.Random.String2(36))
                .Generate();
        }

        private Student GenerateFakeStudent()
        {
            return new Faker<Student>()
                .RuleFor(student => student.Id, faker => Guid.NewGuid())
                .RuleFor(student => student.Email, faker => faker.Person.Email)
                .Generate();
        }
    }
}