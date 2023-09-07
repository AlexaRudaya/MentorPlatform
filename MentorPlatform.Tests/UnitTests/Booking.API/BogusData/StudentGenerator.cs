using Booking.ApplicationCore.DTO;
using Booking.Domain.Entities;

namespace MentorPlatform.Tests.UnitTests.Booking.API.BogusData
{
    public class StudentGenerator
    {
        public Student GenerateFakeStudent()
        {
            return new Faker<Student>()
                .RuleFor(student => student.Id, faker => Guid.NewGuid())
                .RuleFor(student => student.Name, faker => faker.Person.FullName)
                .RuleFor(student => student.Email, faker => faker.Person.Email)
                .RuleFor(student => student.Bookings, faker => GenerateFakeBookingsList())
                .Generate();
        }

        public StudentDto GenerateFakeDto()
        {
            return new Faker<StudentDto>()
                .RuleFor(student => student.Id, faker => Guid.NewGuid())
                .RuleFor(student => student.Name, faker => faker.Person.FullName)
                .RuleFor(student => student.Email, faker => faker.Person.Email)
                .Generate();
        }

        public StudentCreateDto GenerateFakeStudentCreateDto()
        {
            return new Faker<StudentCreateDto>()
                .RuleFor(student => student.Id, faker => Guid.NewGuid())
                .RuleFor(student => student.Name, faker => faker.Person.FullName)
                .RuleFor(student => student.Email, faker => faker.Person.Email)
                .Generate();
        }

        public MentorBooking GenerateFakeBooking()
        {
            return new Faker<MentorBooking>()
                .RuleFor(booking => booking.Id, faker => Guid.NewGuid())
                .RuleFor(booking => booking.StartTimeBooking, faker => faker.Date.Future().Date.AddHours(faker.Random.Int(7, 10)))
                .RuleFor(booking => booking.EndTimeBooking, (faker, booking) => booking.StartTimeBooking.AddHours(faker.Random.Int(3, 4)))
                .RuleFor(booking => booking.StudentId, faker => Guid.NewGuid())
                .RuleFor(booking => booking.MentorId, faker => faker.Random.String2(36))
                .Generate();
        }

        private List<MentorBooking> GenerateFakeBookingsList()
        {
            var bookingsCount = new Random().Next(1, 3);
            var bookings = new List<MentorBooking>();

            for (int i = 0; i < bookingsCount; i++)
            {
                bookings.Add(GenerateFakeBooking());
            }

            return bookings;
        }
    }
}