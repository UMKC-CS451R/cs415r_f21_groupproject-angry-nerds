using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace src.Controllers.API
{
    [ApiController]
    public class GetTransactionHistoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public GetTransactionHistoryController(IConfiguration configuration, ILogger<GetTransactionHistoryController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private class Transaction
        {
            public int TransactionID { get; set; }
            public int TimeMonth { get; set; }
            public int TimeDay { get; set; }
            public int TimeYear { get; set; }
            public int AmountDollars { get; set; }
            public int AmountCents { get; set; }
            public int EndBalanceDollars { get; set; }
            public int EndBalanceCents { get; set; }
            public string Vendor { get; set; }
        }

        private class TransactionHistory
        {
            public List<Transaction> Transactions { get; set; }
            public TransactionHistory()
            {
                Transactions = new List<Transaction>();
            }
        }

        public class RequestTransactionHistory
        {
            public string ID { get; set; }
            public int PageSize { get; set; }
            public int PageNumber { get; set; }
        }

        [HttpPost]
        [Route("api/getTransactionHistory")]
        public async Task<ActionResult> GetAsyncTransactionHistory([FromBody] RequestTransactionHistory body) 
        {
            string connString = this._configuration.GetConnectionString("localDB");
            TransactionHistory history = new TransactionHistory();

            try
            {
                //sql connection object
                using var conn = new MySqlConnection(connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(Path.DirectorySeparatorChar, new List<string> { "Controllers", "API", "SQL", "getTransactionHistory.sql" });
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", body.ID);
                cmd.Parameters.AddWithValue("@START_ROW", body.PageSize * body.PageNumber);
                cmd.Parameters.AddWithValue("@NUM_ROWS", body.PageSize);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving TransactionHistory for AccountID={0} from database.", body.ID));

                //check if there are records
                while (await reader.ReadAsync())
                {

                    history.Transactions.Add(new Transaction()
                    {
                        TransactionID = reader.GetInt32(0),
                        TimeMonth = reader.GetInt32(1),
                        TimeDay = reader.GetInt32(2),
                        TimeYear = reader.GetInt32(3),
                        AmountDollars = reader.GetInt32(4),
                        AmountCents = reader.GetInt32(5),
                        EndBalanceDollars = reader.GetInt32(6),
                        EndBalanceCents = reader.GetInt32(7),
                        Vendor = reader.GetString(8),
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                // StatusCode: 500
                return Problem("Error accessing database, contact site admin for more info");
            }

            if (Helpers.IsEmpty(history.Transactions))
            {
                // Account does not exist (StatusCode: 204)
                return NoContent();
            }

            // StatusCode: 200
            return Ok(history);
        }
    }
}
