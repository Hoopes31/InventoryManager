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

        /// <summary>
        /// Creates and saves a new user to the credential csv
        /// </summary>
        /// <param name="username">Users selected username</param>
        /// <param name="password">Users selected password</param>
        /// <returns>True if the user was created, False if an error occured</returns>
        public bool CreateUser(string username, string password)
        {
            var users = GetAllUsers();

            if (users.ContainsKey(username))
            {
                return false;
            }
            else
            {
                var newUser = string.Format($"{username},{password}\r");
                File.AppendAllText(_connectionString, newUser);
                return true;
            }
        }

        /// <summary>
        /// Checks if a users credentials match csv data
        /// </summary>
        /// <param name="username">Users supplied username</param>
        /// <param name="password">Users supplied password</param>
        /// <returns>True if credentials match the csv, False if they do not.</returns>
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

        /// <summary>
        /// Creates a dictionary of all users in the csv
        /// </summary>
        /// <returns>Dictionary with usernames as keys and passwords as values</returns>
        private Dictionary<string, string> GetAllUsers()
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

        /// <summary>
        /// Creates an credential csv if one does not yet exist
        /// </summary>
        /// <param name="connectionString">Path location of the credential csv</param>
        public void EstablishCredentialData(string connectionString)
        {
            if (!File.Exists(connectionString))
            {
                var establish = string.Format($"Username,Password\r");
                File.AppendAllText(_connectionString, establish);
            }
        }
    }
}
