using System;
namespace HotelBookings
{
	public readonly struct Booking
	{
		public Booking(DateTime date, int room)
        {
			Date = date;
			Room = room;
        }

		public DateTime Date { get; init; }
		public int Room { get; init; }
	}
}

