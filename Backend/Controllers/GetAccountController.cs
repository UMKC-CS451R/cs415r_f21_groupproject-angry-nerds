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

namespace Backend.Controllers.API
{
    [Authorize]
    [ApiController]
    public class GetAccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IUserService _userService;

        public GetAccountController(
            IConfiguration configuration,
            ILogger<GetAccountController> logger,
            IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }
        [HttpPost]
        [Route("api/getAccount")]
        public async Task<ActionResult> GetAccount([FromBody] RequestAccount body)
        {
            string connString = this._configuration.GetConnectionString("localDB");
            AccountInfo account = new AccountInfo();

            try
            {
                //sql connection object
                using var conn = new MySqlConnection(connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar,
                    new List<string> { "SQL", "getAccount.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", body.AccountId);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving Account for AccountId={0} from database.", body.AccountId));

                //check if there are records
                while (await reader.ReadAsync())
                {
                    account.AccountId = reader.GetInt32(0);
                    account.TypeDescription = reader.GetString(1);
                    account.EndBalanceDollars = reader.GetInt32(2);
                    account.EndBalanceCents = reader.GetInt32(3);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                // StatusCode: 500
                return Problem("Error accessing database, contact site admin for more info");
            }

            try
            {
                //sql connection object
                using var conn = new MySqlConnection(connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar,
                    new List<string> { "SQL", "getAccountUsers.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", body.AccountId);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving Users for AccountId={0} from database.", body.AccountId));

                //check if there are records
                while (await reader.ReadAsync())
                {
                    account.Users.Add(new User()
                    {
                        UserId = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3)
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                // StatusCode: 500
                return Problem("Error accessing database, contact site admin for more info");
            }

            // if there are no users, simply say that
            if (account.Users.Count == 0)
            {
                // Account does not exist (StatusCode: 204)
                return NoContent();
            }

            // verify user can see this transaction
            User user = (User)HttpContext.Items["User"];
            if (!(await _userService.VerifyAccount(user, body.AccountId)))
            {
                return Unauthorized();
            }

            // StatusCode: 200
            return Ok(account);
        }
    }
}

