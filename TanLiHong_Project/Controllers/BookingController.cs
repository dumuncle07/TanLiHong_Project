using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TanLiHong_Project.Data;
using TanLiHong_Project.Models;

namespace TanLiHong_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        //admin: get all bookings
        [HttpGet]
        [Authorize(Roles = "Admin, Member")]
        public IActionResult GetAll()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role != "Admin")
                return StatusCode(403, new
                {
                    message = "Only Admin can see all bookings only."
                });

            return Ok(_context.Bookings);
        }

        // admin: update status only
        /*[Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public IActionResult AdminUpdateStatus(int id, [FromBody] Booking booking)
        {
            var entity = _context.Bookings.FirstOrDefault(e => e.BookingId == id);
            if (entity == null)
                return NotFound($"Booking Id {id} not found");

            entity.BookingStatus = booking.BookingStatus;
            _context.SaveChanges();

            return Ok(entity);
        }*/


        // user: get bookings with current user
        [Authorize(Roles = "Member")]
        [HttpGet("mybookings")]
        public IActionResult GetUserBookings()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not identified.");

            var userBookings = _context.Bookings
                .Where(b => b.BookedBy == username)
                .ToList();

            return Ok(userBookings);
        }

        // user: get booking by id (for current user)
        [Authorize(Roles = "Member")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not identified.");

            var booking = _context.Bookings.FirstOrDefault(e => e.BookingId == id && e.BookedBy == username);
            if (booking == null)
            {
                return Problem(detail: "Booking with id " + id + "does not exist", statusCode:404);
            }
            return Ok(booking);
        }

        // user: create booking (for current user)
        [Authorize(Roles = "Admin, Member")]
         [HttpPost]
         public IActionResult Post([FromBody] Booking booking)
         {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not identified.");

            // Always set Booked_By from logged-in user
            booking.BookedBy = username;

            _context.Bookings.Add(booking);
             _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
        }

        // admin and user: updating booking
        // user can only update own booking (admin can update all)
        [Authorize(Roles = "Admin, Member")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Booking booking)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not identified.");

            var entity = _context.Bookings.FirstOrDefault(e => e.BookingId == id);
            if (entity == null)
                return NotFound($"Booking Id {id} not found");

            if (entity.BookedBy != username)
                return Forbid("You can only update your own bookings.");

            entity.FacilityDesc = booking.FacilityDesc;
            entity.BookingDateFrom = booking.BookingDateFrom;
            entity.BookingDateTo = booking.BookingDateTo;
            entity.BookingStatus = booking.BookingStatus;

            _context.SaveChanges();

            return Ok(entity);
        }

        // user: delete booking (for current user)
        [Authorize(Roles = "Admin, Member")]
        [HttpDelete("{id}")]
         public IActionResult Delete(int? id, Booking booking)
         {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not identified.");

            var entity = _context.Bookings.FirstOrDefault(e => e.BookingId == id);
             if (entity == null)
             {
                 return Problem(detail: "Booking with id " + id + "does not exist", statusCode:404);
             }

            if (entity.BookedBy != username)
                return Forbid("You can only delete your own bookings.");

            _context.Bookings.Remove(entity);

             _context.SaveChanges();

             return Ok(entity);
        }
    }
}
