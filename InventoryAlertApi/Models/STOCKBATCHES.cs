using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAlertApi.Models
{
    public class STOCKBATCHES
    {
        [Key]
        public string BATCH_ID { get; set; }
        [ForeignKey(nameof(PRODUCTS))]
        public string PRODUCT_ID { get; set; }
        public PRODUCTS PRODUCTS { get; set; }
        public int QUANTITY { get; set; }
        public int REMAINING_QTY { get; set; }
        public decimal? COST_PER_UNIT { get; set; }
        public DateTime RECEIVED_DATE { get; set; }
        public DateTime EXPIRY_DATE { get; set; }
        [ForeignKey(nameof(WAREHOUSES))]
        public string WAREHOUSE_ID { get; set; }
        public WAREHOUSES WAREHOUSES { get; set; }
        public int DAYS_TO_EXPIRE { get; set; }
    }
}
