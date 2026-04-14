using Microsoft.AspNetCore.Mvc;
using Tutorial6.Data;
using Tutorial6.Models;

namespace Tutorial6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll(
        [FromQuery] DateTime? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var query = AppData.Reservations.AsQueryable();

        if (date.HasValue)
            query = query.Where(r => r.Date.Date == date.Value.Date);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

        if (roomId.HasValue)
            query = query.Where(r => r.RoomId == roomId.Value);

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Reservation> GetById(int id)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Create([FromBody] Reservation reservation)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

        if (room == null)
            return BadRequest("Room does not exist.");

        if (!room.IsActive)
            return Conflict("Cannot create reservation for an inactive room.");

        bool overlaps = AppData.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date.Date == reservation.Date.Date &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime);

        if (overlaps)
            return Conflict("Reservation overlaps with an existing reservation.");

        var newId = AppData.Reservations.Any() ? AppData.Reservations.Max(r => r.Id) + 1 : 1;
        reservation.Id = newId;

        AppData.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Reservation updatedReservation)
    {
        var existingReservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

        if (existingReservation == null)
            return NotFound();

        var room = AppData.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);

        if (room == null)
            return BadRequest("Room does not exist.");

        if (!room.IsActive)
            return Conflict("Cannot assign reservation to an inactive room.");

        bool overlaps = AppData.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updatedReservation.RoomId &&
            r.Date.Date == updatedReservation.Date.Date &&
            updatedReservation.StartTime < r.EndTime &&
            updatedReservation.EndTime > r.StartTime);

        if (overlaps)
            return Conflict("Reservation overlaps with an existing reservation.");

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation == null)
            return NotFound();

        AppData.Reservations.Remove(reservation);

        return NoContent();
    }
}