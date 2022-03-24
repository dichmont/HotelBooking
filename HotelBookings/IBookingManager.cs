using System;
namespace HotelBookings
{
    public interface IBookingManager
    {
        bool IsRoomAvailable(int room, DateTime date);

        void AddBooking(string guest, int room, DateTime date);

        IEnumerable<int> getAvailableRooms(DateTime date);
    }
}