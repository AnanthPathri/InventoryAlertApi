using Microsoft.AspNetCore.Mvc;

namespace InventoryAlertApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetStatus() 
        { 
            return Ok("Inventory alert system is running."); 
        }
    }
}
