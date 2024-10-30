using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Garage_2._0.Models.ViewModels
{
    public class ReceiptViewModel
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

        public TimeSpan ParkedDuration
        {
            get
            {
                return DateTime.Now - ArriveTime;
            }
        }

        [DisplayName("Time of Departure")]
        public DateTime CheckoutTime { get; set; }


        [DisplayName("Total Price")]
        public Decimal Price { get; set; }
    }
}
