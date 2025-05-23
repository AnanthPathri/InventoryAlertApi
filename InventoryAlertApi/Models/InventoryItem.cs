namespace InventoryAlertApi.Models
{
    public class InventoryItem
    {
        public string name { get; set; }=string.Empty;
        public int quantity { get; set; }
        public DateTime expiryDate { get; set; }
        public int overStockThreshold { get; set; } = 100;
    }
}
