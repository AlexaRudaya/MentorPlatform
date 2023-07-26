namespace Mentors.ApplicationCore.Exceptions
{
    public class AvailabilityNotFoundException : ObjectNotFoundException
    {
        private static readonly string AvailabilitiesNotFoundMessage = "No availabilities were found";
        private static readonly string AvailabilityNotFoundMessage = "Availability with such Id {0} was not found";
        public Guid AvailabilityId { get; }

        public AvailabilityNotFoundException() : base(AvailabilitiesNotFoundMessage)
        {
        }

        public AvailabilityNotFoundException(Guid availabilityId) : base(string.Format(AvailabilityNotFoundMessage, availabilityId))
        {
            AvailabilityId = availabilityId;
        }

        public AvailabilityNotFoundException(string message) : base(message)
        {
        }
    }
}