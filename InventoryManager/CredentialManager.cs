using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InventoryManager
{
    public class CredentialManager : ICredentialManager
    {
        private string _connectionString;
        private IOutputProvider _output;

        public CredentialManager(string connectionString, IOutputProvider output)
        {
            _connectionString = connectionString;
            _output = output;
            EstablishCredentialData(_connectionString);
        }
        public bool CreateUser(string username, string password)
        {
            var users = GetAllUsers();

            if (users.ContainsKey(username))
            {
                return false;
            }
            else
            {
                var newUser = string.Format($"{username}, {password}\r");
                File.AppendAllText(_connectionString, newUser);
                return true;
            }
        }

        public bool CheckCredential(string username, string password)
        {
            var users = GetAllUsers();
            if (users.ContainsKey(username))
            {
                if (users[username] == password)
                {
                    return true;
                }
            }
            return false;
        }

        public Dictionary<string, string> GetAllUsers()
        {
            var userData = new Dictionary<string, string>();
            var data = File.ReadAllLines(_connectionString).ToList();
            data.ForEach(users =>
            {
                var namePass = users.Split(',');

                if (namePass.Any())
                {
                    userData[namePass[0]] = namePass[1];
                }
            });
            
            return userData;
        }

        public void EstablishCredentialData(string connectionString)
        {
            try
            {
                GetAllUsers();
            }
            catch (Exception)
            {
                var establish = string.Format($"Username, Password\r");
                File.AppendAllText(_connectionString, establish);
            }
        }
    }
}
