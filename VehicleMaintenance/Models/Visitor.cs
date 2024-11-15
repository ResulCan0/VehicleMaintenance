namespace VehicleMaintenance.Models
{
    public class Visitor
    {
        public Guid Id { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.Now;
        public string IPAddress { get; set; }
        public string PageUrl { get; set; }
    }
}
