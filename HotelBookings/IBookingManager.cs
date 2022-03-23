using System;
namespace HotelBookings
{
    public interface IBookingManager
    {
        private bool IsRoomAvailable(int room, DateTime date);

        void AddBooking(string guest, int room, DateTime date);
    }
}