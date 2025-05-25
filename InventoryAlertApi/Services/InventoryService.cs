using InventoryAlertApi.Data;
using InventoryAlertApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InventoryAlertApi.Services
{
    
    public class InventoryService
    {
        private readonly InventoryDbContext _context;
        public InventoryService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryDTO>> GetInventoryListAsync()
        {
            try
            {
                List<InventoryDTO> result = new List<InventoryDTO>();
                using var connection = _context.Database.GetDbConnection();
                await using var command = connection.CreateCommand();
                command.CommandText = "USP_GETAVAIALBLEPRODUCTS";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Input parameter
                //command.Parameters.Add(new SqlParameter("@Department", department));
                // Output parameter
                //var outputParam = new SqlParameter
                //{
                //    ParameterName = "@TotalEmployees",
                //    SqlDbType = System.Data.SqlDbType.Int,
                //    Direction = System.Data.ParameterDirection.Output
                //};
                //command.Parameters.Add(outputParam);
                // Open connection and execute
                if (connection.State != System.Data.ConnectionState.Open)
                    await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(new InventoryDTO
                    {
                        ProductID = reader["PRODUCT_ID"].ToString(),
                        ProductName = reader["PRODUCT_NAME"].ToString(),
                        Category = reader["CATEGORY"].ToString(),
                        Quantity = reader["TOTAL_REMAINING_QTY"].ToString(),
                        Description = reader["DESCRIPTION"].ToString(),
                        WarehouseName = reader["WAREHOUSE_NAME"].ToString(),
                        WarehouseID = reader["WAREHOUSE_ID"].ToString()

                    });
                }
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task InsertUpdateInventory(InventoryDTO stock, string action)
        {
            using var connection = _context.Database.GetDbConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = "USP_MANAGESTOCKTRANSACTION";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            // Input parameter
            command.Parameters.Add(new SqlParameter("@PRODUCT_ID", stock.ProductID));
            command.Parameters.Add(new SqlParameter("@QUANTITY", stock.Quantity));
            command.Parameters.Add(new SqlParameter("@WAREHOUSE_ID", stock.WarehouseID));
            command.Parameters.Add(new SqlParameter("@ACTION", action));
            // Output parameter
            //var outputParam = new SqlParameter
            //{
            //    ParameterName = "@TotalEmployees",
            //    SqlDbType = System.Data.SqlDbType.Int,
            //    Direction = System.Data.ParameterDirection.Output
            //};
            //command.Parameters.Add(outputParam);
            // Open connection and execute
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();
        }

        public async Task<DashboardDTO> DashboardMetrics()
        {
            DashboardDTO result = new DashboardDTO();
            using var connection = _context.Database.GetDbConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = "usp_GetAlertMetrics";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            // Input parameter
            //command.Parameters.Add(new SqlParameter("@Department", department));
            // Output parameter
            //var outputParam = new SqlParameter
            //{
            //    ParameterName = "@TotalEmployees",
            //    SqlDbType = System.Data.SqlDbType.Int,
            //    Direction = System.Data.ParameterDirection.Output
            //};
            //command.Parameters.Add(outputParam);
            // Open connection and execute
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.ExpiredItems = reader["ExpiredItems"].ToString();
                result.LowStockItems = reader["LowStockItems"].ToString();
                result.OutOfStockItems = reader["OutOfStockItems"].ToString();
                result.ExpiringItems = reader["ExpiringItems"].ToString();
                result.TotalProducts = reader["TotalProducts"].ToString();
                result.OverStockItems = reader["OverStockItems"].ToString();
            }
            return result;
        }

        public async Task<LookupDTO> GetLookupData()
        {
            var products = await _context.PRODUCTS.Where(p => p.ISACTIVE == true).ToListAsync();
            var categories = await _context.CATEGORIES.ToListAsync();
            var warehouses = await _context.WAREHOUSES.ToListAsync();

            var result = new LookupDTO
            {
                Products = products,
                Categories = categories,
                Warehouses = warehouses
            };
            return result;
        }

        public async Task<List<NOTIFICATIONS>> GetNotifications()
        {

            return await _context.NOTIFICATIONS.ToListAsync();

        }
    }
}
