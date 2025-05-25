namespace InventoryAlertApi.Models
{
    public class InventoryDTO
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        //public string SKU { get; set; }
        public string Category { get; set; }
        public string Quantity { get; set; }
        public string Description { get; set; }
        public string WarehouseName { get; set; }

        public string WarehouseID { get; set; }

    }
}
