using System.ComponentModel.DataAnnotations;

namespace InventoryAlertApi.Models
{
    public class NOTIFICATIONS
    {
        [Key]
        public int ID { get; set; }

        public string ALERTRULETYPE { get; set; }
        public string STATUS { get; set; }
        public DateTime SENTAT { get; set; }
        public string CHANNEL { get; set; }
        public string MESSAGE { get; set; }
    }
}
