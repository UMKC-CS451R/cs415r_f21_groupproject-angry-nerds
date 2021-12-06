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
    public class AddAccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IUserService _userService;

        public AddAccountController(
            IConfiguration configuration,
            ILogger<AddAccountController> logger,
            IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }
        [HttpPost]
        [Route("api/addAccount")]
        public async Task<ActionResult> AddAccount([FromBody] RequestAddAccount body)
        {
            string connString = this._configuration.GetConnectionString("localDB");
            int AccountId = -1;

            // verify user is admin
            User user = (User)HttpContext.Items["User"];
            if (!_userService.VerifyAdmin(user))
            {
                return Unauthorized();
            }

            // verify data? - none
            try
            {
                //sql connection object
                using var conn = new MySqlConnection(connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar,
                    new List<string> { "SQL", "addAccount.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TypeDescription", body.AccountType);
                cmd.Parameters.AddWithValue("@PrimaryUser", body.Users[0]);
                cmd.Parameters.AddWithValue("@AmountDollars", body.InitBalanceDollars);
                cmd.Parameters.AddWithValue("@AmountCents", body.InitBalanceCents);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Adding account to database."));

                //check if there are records
                while (await reader.ReadAsync())
                {
                    AccountId = reader.GetInt32(0);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                // StatusCode: 500
                return Problem("Error accessing database, contact site admin for more info");
            }

            // add other users to account
            for (int i = 1; i < body.Users.Count; i++)
            {
                try
                {
                    //sql connection object
                    using var conn = new MySqlConnection(connString);

                    //retrieve the SQL Server instance version
                    string filePath = string.Join(
                        Path.DirectorySeparatorChar,
                        new List<string> { "SQL", "addAccountUser.sql" }
                    );
                    string query = System.IO.File.ReadAllText(filePath);

                    //open connection
                    await conn.OpenAsync();

                    //define the SqlCommand object and execute
                    using var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AccountId", AccountId);
                    cmd.Parameters.AddWithValue("@UserId", body.Users[i]);

                    // using var cmd = new MySqlCommand(query, conn);
                    using var reader = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation(string.Format("Adding account user to database."));

                    //check if there are records
                    while (await reader.ReadAsync())
                    {
                        AccountId = reader.GetInt32(0);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    // StatusCode: 500
                    return Problem("Error accessing database, contact site admin for more info");
                }
            }
            // if the transaction does not exist, simply say that
            if (AccountId == -1)
            {
                // Account does not exist (StatusCode: 204)
                return NoContent();
            }

            // ----------------------------------------
            // Get Account Info and Format for Response

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
                cmd.Parameters.AddWithValue("@ID", AccountId);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving Account for AccountId={0} from database.", AccountId));

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
                cmd.Parameters.AddWithValue("@ID", AccountId);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving Users for AccountId={0} from database.", AccountId));

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
                // Account does not exist (StatusCode: 404)
                return NotFound();
            }

            // StatusCode: 200
            return Ok(account);
        }
    }
}
