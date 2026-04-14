using Tutorial6.Models;

namespace Tutorial6.Data;

public static class AppData
{
    public static List<Room> Rooms { get; set; } = new()
    {
        new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Room 202", BuildingCode = "A", Floor = 2, Capacity = 30, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "Conference 1", BuildingCode = "B", Floor = 1, Capacity = 50, HasProjector = true, IsActive = true },
        new Room { Id = 4, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = false },
        new Room { Id = 5, Name = "Workshop Hall", BuildingCode = "C", Floor = 0, Capacity = 80, HasProjector = true, IsActive = true }
    };

    public static List<Reservation> Reservations { get; set; } = new()
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Anna Kowalska",
            Topic = "C# Basics",
            Date = new DateTime(2026, 5, 10),
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(11, 0, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "John Smith",
            Topic = "ASP.NET Workshop",
            Date = new DateTime(2026, 5, 10),
            StartTime = new TimeSpan(12, 0, 0),
            EndTime = new TimeSpan(14, 0, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Maria Nowak",
            Topic = "REST API Consultation",
            Date = new DateTime(2026, 5, 11),
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(12, 0, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 1,
            OrganizerName = "Piotr Zielinski",
            Topic = "Testing in Postman",
            Date = new DateTime(2026, 5, 12),
            StartTime = new TimeSpan(13, 0, 0),
            EndTime = new TimeSpan(15, 0, 0),
            Status = "cancelled"
        },
        new Reservation
        {
            Id = 5,
            RoomId = 5,
            OrganizerName = "Emily Brown",
            Topic = "Microservices Intro",
            Date = new DateTime(2026, 5, 13),
            StartTime = new TimeSpan(8, 30, 0),
            EndTime = new TimeSpan(10, 30, 0),
            Status = "confirmed"
        }
    };
}