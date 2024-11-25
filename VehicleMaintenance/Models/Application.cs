namespace VehicleMaintenance.Models
{
    public class Application
    {
        public Guid Id { get; set; }
        public DateTime ApplicationDate { get; set; }= DateTime.Now;
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? VehicleTracking { get; set; }  
        public bool? StockTracking { get; set; }    
        public bool? Ecommerce { get; set; }      
        public bool? SeoOptimization { get; set; }
        public string Message { get; set; }
        public string? SalesStatus { get; set; } // Örn. "Satıldı", "Satılmadı"
        public string? Reason { get; set; } // Eğer satılmadıysa neden
    }
}
