using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Backend.Entities;
using Backend.Services;
using Backend.Models.API;
using Backend.Helpers;

namespace Backend.Controllers.API
{
    [Authorize]
    [ApiController]
    public class GetTransactionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IUserService _userService;

        public GetTransactionController(
            IConfiguration configuration,
            ILogger<GetTransactionHistoryController> logger,
            IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }
        [HttpPost]
        [Route("api/getTransaction")]
        public async Task<ActionResult> GetTransaction([FromBody] RequestTransaction body)
        {
            string connString = this._configuration.GetConnectionString("localDB");
            List<TransactionInfo> transactions = new List<TransactionInfo>();

            try
            {
                //sql connection object
                using var conn = new MySqlConnection(connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar,
                    new List<string> { "SQL", "getTransaction.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", body.TransactionId);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving Transaction for TransactionId={0} from database.", body.TransactionId));

                //check if there are records
                while (await reader.ReadAsync())
                {

                    transactions.Add(new TransactionInfo()
                    {
                        TransactionId = reader.GetInt32(0),
                        AccountId = reader.GetInt32(1),
                        TimeMonth = reader.GetInt32(2),
                        TimeDay = reader.GetInt32(3),
                        TimeYear = reader.GetInt32(4),
                        AmountDollars = reader.GetInt32(5),
                        AmountCents = reader.GetInt32(6),
                        EndBalanceDollars = reader.GetInt32(7),
                        EndBalanceCents = reader.GetInt32(8),
                        LocationStCd = reader.GetString(9),
                        CountryCd = reader.GetString(10),
                        Vendor = reader.GetString(11)
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                // StatusCode: 500
                return Problem("Error accessing database, contact site admin for more info");
            }

            TransactionInfo transaction = transactions.SingleOrDefault();

            // if the transaction does not exist, simply say that
            if (transaction == null)
            {
                // Account does not exist (StatusCode: 204)
                return NoContent();
            }

            // verify user can see this transaction
            User user = (User)HttpContext.Items["User"];
            if (!(await _userService.VerifyAccount(user, transaction.AccountId)))
            {
                return Unauthorized();
            }
            //test email move to post transactions with rules...
            EmailHelper email= new EmailHelper(_configuration);
            email.send();
            // StatusCode: 200
            return Ok(transaction);
        }
    }
}
