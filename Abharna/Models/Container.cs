using System.ComponentModel.DataAnnotations;

namespace Abharna.Models
{
    public class Container
    {
        public int Id { get; set; }

        [Required, StringLength(11)]
        public string ContainerNumber { get; set; } = "";

        [Required]
        public string Cargo { get; set; } = "";

        // 1 = Registered, 2 = In transit, 3 = At port, 4 = Customs hold, 5 = Released
        public int Stage { get; set; }

        public string Location { get; set; } = "";
        public string Vessel { get; set; } = "";
        public string Eta { get; set; } = "";
    }
}