using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TanLiHong_Project.Data;
using TanLiHong_Project.Models;

namespace TanLiHong_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Bookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(e => e.BookingId == id);
            if (booking == null)
            {
                return Problem(detail: "Booking with id " + id + "does not exist", statusCode:404);
            }
            return Ok(booking);
        }
         [HttpPost]
         public IActionResult Post(Booking booking)
         {
             _context.Bookings.Add(booking);
             _context.SaveChanges();

             return CreatedAtAction("GetAll", new {id = booking.BookingId}, booking);
        }

        [HttpPut]
        public IActionResult Put(Booking booking)
        {
            var entity = _context.Bookings.FirstOrDefault(e => e.BookingId == booking.BookingId);
            if (entity == null)
            {
                return Problem(detail: "Booking with id " + booking.BookingId + "does not exist", statusCode:404);
            }
            entity.FacilityDesc = booking.FacilityDesc;
            entity.BookingDateFrom = booking.BookingDateFrom;
            entity.BookingDateTo = booking.BookingDateTo;
            entity.BookedBy = booking.BookedBy;
            entity.BookingStatus = booking.BookingStatus;

            _context.SaveChanges();

            return Ok(entity);
         }
         [HttpDelete]
         public IActionResult Delete(int? id, Booking booking)
         {
             var entity = _context.Bookings.FirstOrDefault(e => e.BookingId == id);
             if (entity == null)
             {
                 return Problem(detail: "Booking with id " + id + "does not exist", statusCode:404);
             }
             _context.Bookings.Remove(entity);

             _context.SaveChanges();

             return Ok(entity);
        }
    }
}
