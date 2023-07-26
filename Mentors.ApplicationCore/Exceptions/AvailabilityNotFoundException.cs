namespace Mentors.ApplicationCore.Exceptions
{
    public class AvailabilityNotFoundException : ObjectNotFoundException
    {
        private static readonly string _availabilitiesNotFoundMessage = "No availabilities were found";
        private static readonly string _availabilityNotFoundMessage = "Availability with such Id {0} was not found";
        public Guid AvailabilityId { get; }

        public AvailabilityNotFoundException() : base(_availabilitiesNotFoundMessage)
        {
        }

        public AvailabilityNotFoundException(Guid availabilityId) : base(string.Format(_availabilityNotFoundMessage, availabilityId))
        {
            AvailabilityId = availabilityId;
        }
    }
}