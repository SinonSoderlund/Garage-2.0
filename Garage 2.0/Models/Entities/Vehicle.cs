using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public uint Wheels { get; set; }

        public DateTime ArriveTime { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(20)]
        public string RegNr { get; set; }

        [StringLength(20)]
        public string Model {  get; set; }

        [StringLength(20)]
        public string Brand { get; set; }

        public VehicleType VehicleType { get; set; }
        
    }
}
