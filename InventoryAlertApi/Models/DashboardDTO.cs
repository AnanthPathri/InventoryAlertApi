namespace InventoryAlertApi.Models
{
    public class DashboardDTO
    {
        public string LowStockItems { get; set; }

        public string OutOfStockItems { get; set; }
        public string OverStockItems { get; set; }

        public string ExpiringItems { get; set; }

        public string TotalProducts { get; set; }

        public string ExpiredItems { get; set; }
    }
}
