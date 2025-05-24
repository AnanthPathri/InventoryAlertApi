using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAlertApi.Models
{
    public class WAREHOUSES
    {
        [Key]
        public string WAREHOUSE_ID { get; set; }
        public string WAREHOUSE_NAME { get; set; }
        public string WAREHOUSE_LOCATION { get; set; }
        public ICollection<STOCKBATCHES> STOCKBATCHES { get; set; }
    }
}
