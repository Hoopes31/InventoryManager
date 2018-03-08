using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager
{
    public interface ICredentialManager
    {
        /// <summary>
        /// Creates a user in the credential database
        /// </summary>
        /// <param name="username">The users login name</param>
        /// <param name="password">The users password</param>
        void CreateUser(string username, string password);

        /// <summary>
        /// Checks the users credentials against the database
        /// </summary>
        /// <param name="username">The users login name</param>
        /// <param name="password">The users password</param>
        /// <returns>A boolean indicating if credentials match</returns>
        bool CheckCredential(string username, string password);
    }
}
