using Microsoft.AspNetCore.Mvc;
using Tutorial6.Data;
using Tutorial6.Models;

namespace Tutorial6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var query = AppData.Rooms.AsQueryable();

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            query = query.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue && activeOnly.Value)
            query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room == null)
            return NotFound();

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetByBuilding(string buildingCode)
    {
        var rooms = AppData.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> Create([FromBody] Room room)
    {
        var newId = AppData.Rooms.Any() ? AppData.Rooms.Max(r => r.Id) + 1 : 1;
        room.Id = newId;

        AppData.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Room updatedRoom)
    {
        var existingRoom = AppData.Rooms.FirstOrDefault(r => r.Id == id);

        if (existingRoom == null)
            return NotFound();

        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;

        return Ok(existingRoom);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room == null)
            return NotFound();

        var hasRelatedReservations = AppData.Reservations.Any(r => r.RoomId == id);

        if (hasRelatedReservations)
            return Conflict("Cannot delete room because related reservations exist.");

        AppData.Rooms.Remove(room);

        return NoContent();
    }
}