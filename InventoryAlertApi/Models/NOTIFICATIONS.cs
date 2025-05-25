using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAlertApi.Models
{
    public class NOTIFICATIONS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ALERTRULETYPE { get; set; }
        public string MESSAGE { get; set; }
        public string CHANNEL { get; set; }
        public DateTime SENTAT { get; set; }
        public string STATUS { get; set; }
    }
}
