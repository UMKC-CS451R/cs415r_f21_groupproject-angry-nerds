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
    public class GetUserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public GetUserController(
            IConfiguration configuration, 
            ILogger<GetUserController> logger, 
            IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/getUser")]
        public async Task<ActionResult> GetUser([FromBody] RequestUser body) 
        {
            User user = (User)HttpContext.Items["User"];
            if (user.UserId != body.UserId)
            {
                return Unauthorized();
            }

            string connString = this._configuration.GetConnectionString("localDB");
            List<Account> accounts = new List<Account>();
            try
            {
                //sql connection object
                using var conn = new MySqlConnection(connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar,
                    new List<string> { "SQL", "getUserAccounts.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", user.UserId);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving Accounts for UserID={0} from database.", user.UserId));

                //check if there are records
                while (await reader.ReadAsync())
                {
                    accounts.Add(new Account()
                    {
                        AccountId = reader.GetInt32(0),
                        TypeDescription = reader.GetString(1),
                        EndBalanceDollars = reader.GetInt32(2),
                        EndBalanceCents = reader.GetInt32(3)
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            if (Helpers.IsEmpty(accounts))
            {
                // Account does not exist (StatusCode: 204)
                return NoContent();
            }

            // StatusCode: 200
            return Ok(new UserInfo(user, accounts));
        }
    }
}
