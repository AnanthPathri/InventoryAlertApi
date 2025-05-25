namespace InventoryAlertApi.Models
{
    public class LookupDTO
    {
        public List<PRODUCTS> Products { get; set; }
        public List<CATEGORIES> Categories{ get; set; }
        public List<WAREHOUSES> Warehouses { get; set; }
    }
}
