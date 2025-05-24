using System.ComponentModel.DataAnnotations;

namespace InventoryAlertApi.Models
{
    public class ALERTRULES
    {
        [Key]
        public int ALERT_ID { get; set; }
        public string RULE_TYPE { get; set; }
        public string ALERT_CONDITION { get; set; }
        public string COMMUNICATION_CHANNAL { get; set; }
        public string RECIPIENT_EMAIL_ID { get; set; }
        public bool IS_ACTIVE { get; set; }
    }
}
