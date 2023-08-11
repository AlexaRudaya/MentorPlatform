﻿namespace Booking.ApplicationCore.Interfaces.IService
{
    public interface IBookingForMentorService
    {
        Task<IEnumerable<BookingDto>> GetBookingsForMentorAsync(string mentorId, CancellationToken cancellationToken = default);

        Task GetAvailabilitiesOfMentor(string mentorId, CancellationToken cancellationToken = default);
    }
}