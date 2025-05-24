using InventoryAlertApi.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAlertApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryAlertJob _inventoryAlertJob;
        public InventoryController(InventoryAlertJob inventoryAlertJob)
        {
            _inventoryAlertJob = inventoryAlertJob;
        }
        [HttpGet("status")]
        public ActionResult GetStatus() 
        { 
            return Ok("Inventory alert system is running."); 
        }
        [HttpPost("run-alert")]
        public async Task<IActionResult> RunAlertJob()
        {
            await _inventoryAlertJob.Execute(null);
            return Ok("Inventory alert executed and email sent.");
        }
    }
}
