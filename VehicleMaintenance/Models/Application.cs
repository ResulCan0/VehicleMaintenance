namespace VehicleMaintenance.Models
{
    public class Application
    {
        public Guid Id { get; set; }
        public DateTime ApplicationDate { get; set; }= DateTime.Now;
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? VehicleTracking { get; set; }  // For checkbox VehicleTracking
        public bool? StockTracking { get; set; }    // For checkbox StockTracking
        public bool? Ecommerce { get; set; }        // For checkbox Ecommerce
        public bool? SeoOptimization { get; set; }
        public string Message { get; set; }
    }
}
