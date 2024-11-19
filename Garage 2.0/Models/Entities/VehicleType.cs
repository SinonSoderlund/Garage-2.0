namespace Garage_2._0.Models.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SpotSize { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } // nav prop
    }
}
