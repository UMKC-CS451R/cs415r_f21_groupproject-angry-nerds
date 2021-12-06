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
    public class AddTransactionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IUserService _userService;

        public AddTransactionController(
            IConfiguration configuration,
            ILogger<AddTransactionController> logger,
            IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }

        private class Notifications
        {
            private readonly IUserService _localUserService;
            private readonly string _localConnString;
            public Notifications(IUserService userService, string connString)
            {
                _localUserService = userService;
                _localConnString = connString;
            }
            public async void SendNotifications(User user, TransactionInfo transaction)
            {
                // Get list of user enabled notifications and apply them to this transaction
                List<string> fnList = new List<string>();

                //sql connection object
                using var conn = new MySqlConnection(_localConnString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar,
                    new List<string> { "SQL", "getUserNotifications.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", user.UserId);

                using var reader = await cmd.ExecuteReaderAsync();

                //check if there are records
                try
                {
                    while (await reader.ReadAsync())
                    {
                        string temp = reader.GetString(0);
                        fnList.Add(temp);
                    }
                }
                catch
                {
                    return;
                }
                

                for (int i = 0; i < fnList.Count; i++)
                {
                    switch (fnList[i])
                    {
                        case "OutOfState":
                            OutOfState(user, transaction);
                            break;
                        case "Over50":
                            Over50(user, transaction);
                            break;
                        case "Deposit":
                            Deposit(user, transaction);
                            break;
                    }
                }
            }
            private async void OutOfState(User user, TransactionInfo transaction)
            {
                User full_user = await _localUserService.GetFullUser(user.UserId);
                if (full_user.PostalState != transaction.LocationStCd)
                {
                    string note = string.Format("A purchase was made out of state at {0} in {1}, {2}", transaction.Vendor, transaction.LocationStCd, transaction.CountryCd);
                    _localUserService.Notify(user, note);
                }
            }
            private void Over50(User user, TransactionInfo transaction) {
                if (transaction.AmountDollars <= -50)
                {
                    string note = string.Format("Spent ${0}.{1} at {2}", transaction.AmountDollars, transaction.AmountCents, transaction.Vendor);
                    _localUserService.Notify(user, note);
                }
            }
            private void Deposit(User user, TransactionInfo transaction)
            {
                if (transaction.AmountDollars > 0 || (transaction.AmountCents > 0 && transaction.AmountDollars >= 0))
                {
                    string note = string.Format("Deposit of ${0}.{1}", transaction.AmountDollars, transaction.AmountCents);
                    _localUserService.Notify(user, note);
                }
            }
        }


        [HttpPost]
        [Route("api/addTransaction")]
        public async Task<ActionResult> AddTransaction([FromBody] RequestAddTransaction body)
        {
            string connString = this._configuration.GetConnectionString("localDB");

            // verify user is admin
            User user = (User)HttpContext.Items["User"];
            if (!_userService.VerifyAdmin(user))
            {
                return Unauthorized();
            }

            // verify data? - none
            List<TransactionInfo> transactions = new List<TransactionInfo>();
            try
            {
                //sql connection object
                using var conn = new MySqlConnection(connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar,
                    new List<string> { "SQL", "addTransaction.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountId", body.AccountId);
                cmd.Parameters.AddWithValue("@TimeMonth", body.TimeMonth);
                cmd.Parameters.AddWithValue("@TimeDay", body.TimeDay);
                cmd.Parameters.AddWithValue("@TimeYear", body.TimeYear);
                cmd.Parameters.AddWithValue("@AmountDollars", body.AmountDollars);
                cmd.Parameters.AddWithValue("@AmountCents", body.AmountCents);
                cmd.Parameters.AddWithValue("@LocationStCd", body.LocationStCd);
                cmd.Parameters.AddWithValue("@CountryCd", body.CountryCd);
                cmd.Parameters.AddWithValue("@Vendor", body.Vendor);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Adding account to database."));

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
                        Vendor = reader.GetString(11),
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                // StatusCode: 500
                return Problem("Error accessing database, contact site admin for more info");
            }


            // if there are no transactions, also a problem - simply say that
            if (transactions.Count == 0)
            {
                // Account does not exist (StatusCode: 500)
                return Problem("Could not get transaction after database entry");
            }
            TransactionInfo transaction = transactions.SingleOrDefault();

            Notifications NoteService = new Notifications(_userService, connString);

            List<User> users = new List<User>();
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
                    users.Add(new User()
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

            for (int i = 0; i < users.Count; i++)
            {
                NoteService.SendNotifications(users[i], transaction);
            }

            // StatusCode: 200
            return Ok(transaction);
        }
    }
}
