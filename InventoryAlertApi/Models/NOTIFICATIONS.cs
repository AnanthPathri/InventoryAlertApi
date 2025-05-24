using System.ComponentModel.DataAnnotations;

namespace InventoryAlertApi.Models
{
    public class NOTIFICATIONS
    {
        [Key]
        public string ID { get; set; }
        public string ALERTRULETYPE { get; set; }
        public string MESSAGE { get; set; }
        public string CHANNEL { get; set; }
        public DateTime SENTAT { get; set; }
        public string STATUS { get; set; }
    }
}
