using System.Collections.Concurrent;

namespace HotelBookings;
public class BookingManager : IBookingManager
{
    public BookingManager(HashSet<int> rooms)
    {
        this.rooms = rooms;
    }

    private readonly ConcurrentDictionary<Booking, string> bookings = new();
    private readonly HashSet<int> rooms;

    public void ThrowIfTimeNotMidday(DateTime date)
    {
        if (date.ToString("H:mm") != "12:00")
        {
            throw new ArgumentException("Date input time component must be midday.");
        }
    }

    public void ThrowIfRoomInvalid(int room)
    {
        if(!rooms.Contains(room))
        {
            throw new ArgumentException("Room doesn't exist.");
        }
    }

    public bool IsRoomAvailable(int room, DateTime date)
    {
        ThrowIfTimeNotMidday(date);
        ThrowIfRoomInvalid(room);
        var booking = new Booking(date, room);
        return bookings.ContainsKey(booking);
    }

    public void AddBooking(string guest, int room, DateTime date)
    {
        ThrowIfTimeNotMidday(date);
        ThrowIfRoomInvalid(room);
        var booking = new Booking(date, room);
        if (!bookings.TryAdd(booking, guest))
        {
            throw new Exception("Booking already exists.");
        }
    }

    public IEnumerable<int> getAvailableRooms(DateTime date)
    {
        ThrowIfTimeNotMidday(date);
        var bookedRooms = bookings.Keys
            .Where(booking => DateTime.Compare(booking.Date, date) == 0)
            .Select(booking => booking.Room)
            .ToHashSet();
        return rooms.Except(bookedRooms);
    }
}