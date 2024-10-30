using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models.ViewModels
{
    public class DetailViewModel
    {
        public int Id { get; set; }
        public uint Wheels { get; set; }

        [DisplayName("Time of Arrival")]
        public DateTime ArriveTime { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(20)]
        [DisplayName("Registration Number")]
        public string RegNr { get; set; }

        [StringLength(20)]
        public string Model { get; set; }

        [StringLength(20)]
        public string Brand { get; set; }

        [DisplayName("Type of Vehicle")]
        public VehicleType VehicleType { get; set; }

    }
}
