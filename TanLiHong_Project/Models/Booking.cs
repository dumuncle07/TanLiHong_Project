using System.ComponentModel.DataAnnotations;

namespace TanLiHong_Project.Models
{
    public class Booking
    {
        [Key]

        public int BookingId { get; set; }

        public string FacilityDesc { get; set; }

        public string BookingDateFrom { get; set; }

        public string BookingDateTo { get; set; }

        public string BookedBy { get; set; }

        public string BookingStatus { get; set; }
    }
}
