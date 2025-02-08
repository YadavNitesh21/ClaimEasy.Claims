using System.ComponentModel.DataAnnotations;

namespace Claims.Models
{
    public class VehicleRequest
    {
        [Required]
        public string VehicleNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string Model { get; set; }
        public string Color { get; set; }
    }
}
