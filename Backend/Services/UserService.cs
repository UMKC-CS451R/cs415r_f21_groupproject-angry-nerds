using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using SHA3.Net;
using MySqlConnector;

using Backend.Entities;
using Backend.Helpers;
using Backend.Models;
using Backend.Models.API;

namespace Backend.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        RefreshResponse ReAuthenticate(User user);
        Task<User> GetById(int id);
        Task<User> VerifyUser(string token);
        Task<bool> VerifyAccount(User user, int accountId);
        bool VerifyAdmin(User user);
    }

    public class UserService : IUserService
    {
        private readonly AppSettingsAccessor _appSettings;
        private readonly string _connString;
        private readonly ILogger _logger;
        private readonly double _tokenLength = 15.0;

        public UserService(ILogger<UserService> logger, IOptions<AppSettingsAccessor> appSettings)
        {
            _appSettings = appSettings.Value;
            _connString = _appSettings.ConnectionStrings.LocalDB;
            _logger = logger;
        }

        public byte[] HashPassword(string salt, string password) 
        {
            return Sha3.Sha3256().ComputeHash(Encoding.UTF8.GetBytes(salt + password));
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            List<User> users = new List<User>();
            try
            {
                //sql connection object
                using var conn = new MySqlConnection(_connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar, 
                    new List<string> { "SQL", "getUserByEmail.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EMAIL", model.Email);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving User for Email={0} from database.", model.Email));

                //check if there are records
                while (await reader.ReadAsync())
                {
                    byte[] rawSalt = new byte[24];
                    reader.GetBytes(5, 0, rawSalt, 0, 24);
                    byte[] rawPwd = new byte[32];
                    reader.GetBytes(6, 0, rawPwd, 0, 32);
                    users.Add(new User()
                    {
                        Email = reader.GetString(0),
                        UserId = reader.GetInt32(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Role = reader.GetString(4),
                        Salt = rawSalt,
                        Pwd = rawPwd
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
            var user = users.FirstOrDefault();
            // user does not exist: return nulls
            if (user == null) return null;

            // verify the password is correct
            var hash = HashPassword(Encoding.UTF8.GetString(user.Salt), model.Password);
            if (!hash.SequenceEqual(user.Pwd)) return null;

            // authentication successful so generate jwt token
            long TokenExpires = ((DateTimeOffset)DateTime.UtcNow.AddMinutes(_tokenLength)).ToUnixTimeMilliseconds();
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user) { 
                Token = token,
                TokenExpires = TokenExpires
            };
        }

        public RefreshResponse ReAuthenticate(User user)
        {
            if (user == null) return null;

            // authentication successful so generate jwt token
            long TokenExpires = ((DateTimeOffset)DateTime.UtcNow.AddMinutes(_tokenLength)).ToUnixTimeMilliseconds();
            var newToken = generateJwtToken(user);

            return new RefreshResponse(user)
            {
                Token = newToken,
                TokenExpires = TokenExpires
            };            
        }

        public async Task<User> VerifyUser(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.AppSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                var user = await GetById(userId);
                return user;
            }
            catch
            {
                // JWT verification failed
                return null;
            }
        }

        public async Task<User> GetById(int id)
        {
            List<User> users = new List<User>();
            try
            {
                //sql connection object
                using var conn = new MySqlConnection(_connString);

                //retrieve the SQL Server instance version
                string filePath = string.Join(
                    Path.DirectorySeparatorChar,
                    new List<string> { "SQL", "getUserById.sql" }
                );
                string query = System.IO.File.ReadAllText(filePath);

                //open connection
                await conn.OpenAsync();

                //define the SqlCommand object and execute
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                // using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                _logger.LogInformation(string.Format("Retrieving User for UserID={0} from database.", id));

                //check if there are records
                while (await reader.ReadAsync())
                {
                    users.Add(new User()
                    {
                        Email = reader.GetString(0),
                        UserId = reader.GetInt32(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Role = reader.GetString(4)
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return users.FirstOrDefault();
        }

        public async Task<bool> VerifyAccount(User user, int accountId)
        {
            List<Account> accounts = new List<Account>();
            try
            {
                //sql connection object
                using var conn = new MySqlConnection(_connString);

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
            return accounts.SingleOrDefault(x => x.AccountId == accountId) != null;
        }

        public bool VerifyAdmin(User user) {
            return user.Role == "Admin";
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for one hour
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(_tokenLength),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}