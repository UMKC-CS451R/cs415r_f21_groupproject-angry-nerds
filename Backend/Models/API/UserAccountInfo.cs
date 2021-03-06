using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Models.API
{
    public class Account
    {
        public int AccountId { get; set; }
        public string TypeDescription { get; set; }
        public int EndBalanceDollars { get; set; }
        public int EndBalanceCents { get; set; }
    }
    public class AccountInfo
    {
        public int AccountId { get; set; }
        public string TypeDescription { get; set; }
        public int EndBalanceDollars { get; set; }
        public int EndBalanceCents { get; set; }
        public List<User> Users { get; set; }
        public AccountInfo()
        {
            Users = new List<User>();
        }
    }
    public class UserInfo
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Account> Accounts { get; set; }
        public UserInfo()
        {
            Accounts = new List<Account>();
        }
        public UserInfo(User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            
            Accounts = new List<Account>();
        }
        public UserInfo(User user, List<Account> accountList)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;

            Accounts = accountList;
        }
    }
}
