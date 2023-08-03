namespace Booking.ApplicationCore.Exceptions
{
    public class StudentNotFoundException : ObjectNotFoundException
    {
        private static readonly string _studentsNotFoundMessage = "No students were found";
        private static readonly string _studentNotFoundMessage = "Student with such Id {0} was not found";
        public Guid StudentId { get; }

        public StudentNotFoundException() : base(_studentsNotFoundMessage)
        {
        }

        public StudentNotFoundException(Guid studentId) : base(string.Format(_studentNotFoundMessage, studentId))
        {
            StudentId = studentId;
        }
    }
}