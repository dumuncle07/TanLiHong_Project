using Microsoft.EntityFrameworkCore;
using TanLiHong_Project.Models;

namespace TanLiHong_Project.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }
    }
}
