using System;
using System.Collections.Generic;
using System.Globalization;
using HotelBookings;
using NUnit.Framework;

namespace HotelBookingsPractice;

public class Tests
{
    private readonly DateTime middayDate = DateTime.Parse("01 Jan 2020, 12:00", new CultureInfo("en-GB"));

    private BookingManager getBookingManagerWithOneRoom()
    {
        var rooms = new HashSet<int> { 1 };
        return new BookingManager(rooms);
    }

    private void addOneBooking(BookingManager bookingManager)
    {
        var guestSurname = "Surname";
        var room = 1;

        bookingManager.AddBooking(guestSurname, room, middayDate);
    }

    [Test]
    public void CanMakeBooking()
    {
        var bookingManager = getBookingManagerWithOneRoom();

        addOneBooking(bookingManager);

        Assert.Pass();
    }

    [Test]
    public void CannotMakeDoubleBooking()
    {
        var bookingManager = getBookingManagerWithOneRoom();

        addOneBooking(bookingManager);

        Assert.Throws<Exception>(() => addOneBooking(bookingManager));
    }

    [Test]
    public void RoomsAvailableByDefault()
    {
        var bookingManager = getBookingManagerWithOneRoom();
        var room = 1;
        var date = DateTime.Parse("01 Jan 2020, 12:00", new CultureInfo("en-GB"));

        var booked = bookingManager.IsRoomAvailable(room, date);

        Assert.IsFalse(booked);
    }

    [Test]
    public void BookedRoomIsNotAvailable()
    {
        var bookingManager = getBookingManagerWithOneRoom();
        addOneBooking(bookingManager);

        var booked = bookingManager.IsRoomAvailable(1, middayDate);

        Assert.IsTrue(booked);
    }

    [Test]
    public void MiddayDatesAreValid()
    {
        var bookingManager = new BookingManager(new HashSet<int>());
        var date = DateTime.Parse("01 Jan 2020, 12:00", new CultureInfo("en-GB"));

        bookingManager.ThrowIfTimeNotMidday(date);

        Assert.Pass();
    }

    [Test]
    public void NonMiddayDatesAreInvalid()
    {
        var bookingManager = new BookingManager(new HashSet<int>());

        var date = DateTime.Parse("01 Jan 2020, 14:30", new CultureInfo("en-GB"));

        Assert.Throws<ArgumentException>(() => bookingManager.ThrowIfTimeNotMidday(date));
    }

    [Test]
    public void AllRoomsAvailableWhenNoBookings()
    {
        var rooms = new HashSet<int>();
        rooms.Add(1);
        rooms.Add(2);
        var bookingManager = new BookingManager(rooms);

        var availableRooms = bookingManager.getAvailableRooms(middayDate);

        Assert.AreEqual(availableRooms, rooms);
    }

    [Test]
    public void BookedRoomsUnavailable()
    {
        var rooms = new HashSet<int>();
        rooms.Add(1);
        rooms.Add(2);
        var bookingManager = new BookingManager(rooms);
        addOneBooking(bookingManager);

        var availableRooms = bookingManager.getAvailableRooms(middayDate);

        Assert.AreEqual(availableRooms, new HashSet<int>{2});
    }

    [Test]
    public void ExistentRoomsValid()
    {
        var bookingManager = getBookingManagerWithOneRoom();

        bookingManager.ThrowIfRoomInvalid(1);

        Assert.Pass();
    }

    [Test]
    public void NonexistentRoomsInvalid()
    {
        var bookingManager = getBookingManagerWithOneRoom();

        Assert.Throws<ArgumentException>(() => bookingManager.ThrowIfRoomInvalid(2));
    }
}
