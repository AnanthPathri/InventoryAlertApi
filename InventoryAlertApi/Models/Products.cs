using System.ComponentModel.DataAnnotations;

namespace InventoryAlertApi.Models
{
    public class PRODUCTS
    {
        [Key]
        public string PRODUCT_ID { get; set; }
        public string NAME { get; set; }
        public string SKU { get; set; }
        public string CATEGORY_ID { get; set; }
        public string UNIT { get; set; }
        public string DESCRIPTION { get; set; }
        public bool ISACTIVE { get; set; }
        public int MIN_THRESHOLD { get; set; }
        public int MAX_THRESHOLD { get; set; }

        public ICollection<STOCKBATCHES> STOCKBATCHES { get; set; }
    }
}
