using InventoryAlertApi.Jobs;
using InventoryAlertApi.Models;
using InventoryAlertApi.RealHub;
using InventoryAlertApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAlertApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryAlertJob _inventoryAlertJob;
        private readonly InventoryService _inventoryService;
        private readonly IHubContext<RealTimeHub> _hubContext;
        public InventoryController(InventoryAlertJob inventoryAlertJob, InventoryService inventoryService, IHubContext<RealTimeHub> hubContext)
        {
            _inventoryAlertJob = inventoryAlertJob;
            _inventoryService = inventoryService;
            _hubContext = hubContext;
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

        [HttpGet("inventory-list")]
        public async Task<ActionResult<IEnumerable<InventoryDTO>>> InventoryList()
        {
            var result = await _inventoryService.GetInventoryListAsync();
            return Ok(result);
        }

        [HttpPost("inventory")]
        public async Task<ActionResult> AddInventory([FromBody] InventoryDTO inventory )
        {
            
            await _inventoryService.InsertUpdateInventory(inventory,"ADD");
            //var result = await _inventoryService.DashboardMetrics();
            //await _hubContext.Clients.All.SendAsync("ReceiveMessage", result);
            return Ok();
        }

        [HttpPost("update-inventory")]
        public async Task<ActionResult> UpdateInventory([FromBody] InventoryDTO inventory)
        {

            await _inventoryService.InsertUpdateInventory(inventory, "UPDATE");
           // var result = await _inventoryService.DashboardMetrics();
           // await _hubContext.Clients.All.SendAsync("ReceiveMessage", result);
            return Ok();
        }

        [HttpPost("delete-inventory")]
        public async Task<ActionResult> DeleteInventory([FromBody] InventoryDTO inventory)
        {

            await _inventoryService.InsertUpdateInventory(inventory, "DELETE");
            //var result = await _inventoryService.DashboardMetrics();
            //await _hubContext.Clients.All.SendAsync("ReceiveMessage", result);
            return Ok();
        }

        [HttpGet("dashboard-metrics")]
        public async Task<ActionResult<IEnumerable<DashboardDTO>>> DashBoardMetrics()
        {
            var result = await _inventoryService.DashboardMetrics();
            return Ok(result);
        }

        [HttpGet("lookup-data")]
        public async Task<ActionResult<LookupDTO>> LookupData()
        {
            var result = await _inventoryService.GetLookupData();
            return Ok(result);
        }

        [HttpGet("notification-list")]
        public async Task<ActionResult<List<NOTIFICATIONS>>> Notifications()
        {
            var result = await _inventoryService.GetNotifications();
            return Ok(result);
        }

    }
}
