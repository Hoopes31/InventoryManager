using System;
using System.Threading;

namespace InventoryManager
{
    public class AppLogic
    {
        enum AppStatus { Welcome, Register, Login, UserActions, CloseApp };
        private IOutputProvider _output;
        private IInputProvider _input;
        private ICredentialManager _credentialManager;
        private AppStatus _status;
        private IDataManager _inventoryManager;

        public AppLogic(string credentialData, string inventoryData)
        {
            _output = new OutputConsole();
            _input = new InputConsole();
            _credentialManager = new CredentialManager(credentialData, _output);
            _inventoryManager = new InventoryDataManager(inventoryData, _output);
        }

        public void Run()
        {
            // Welcome
            _status = AppStatus.Welcome;
            _output.Send("INVENTORY MANAGER");

            // SignUp or Login
            while (_status == AppStatus.Welcome)
            {
                _output.Send("1 Register");
                _output.Send("2 Login");
                _output.Send("Please select one of the two given options.");
                LoginOrRegister();
            }

            // Register
            while (_status == AppStatus.Register)
            {
                _output.Send("INVENTORY MANAGER REGISTRATION");
                _output.Send("Please select a username");
                var username = _input.ReadData();
                _output.Send("Please select a password");
                var password = _input.ReadData();

                Register(username, password);
            }

            // Login
            while (_status == AppStatus.Login)
            {
                _output.Send("INVENTORY MANAGER LOGIN");
                _output.Send("Please enter your username");
                var username = _input.ReadData();
                _output.Send("Please enter your password");
                var password = _input.ReadData();

                Login(username, password);
            }

            // Ask for action
            while (_status == AppStatus.UserActions)
            {
                _output.Send("INVENTORY MANAGER ACTIONS");
                _output.Send("1 Create item");
                _output.Send("2 Add to item quantity");
                _output.Send("3 Subtract from item quantity");
                _output.Send("4 Show all inventory");
                _output.Send("5 Close program");
                UserActions();
            }

            // Close application
            if (_status == AppStatus.CloseApp)
            {
                _output.Send("Goodbye");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Directs program to the proper action based off user input
        /// </summary>
        public void UserActions()
        {
            var response = _input.ReadData();
            Console.Clear();

            if (response == "1")
            {
                CreateItem();
                Console.Clear();
            }
            if (response == "2")
            {
                AddQuantity();
            }
            if (response == "3")
            {
                RemoveQuantity();
            }
            if (response == "4")
            {
                ShowAllItems();
                _output.Send("Press enter to continue");
                _input.ReadData();
                Console.Clear();
            }
            if (response == "5")
            {
                _status = AppStatus.CloseApp;
            }
        }

        /// <summary>
        /// Displays all inventory items to the user
        /// </summary>
        public void ShowAllItems()
        {
            var inventory = _inventoryManager.GetAllItems();
            for(var i = 1; i < inventory.Count; i++)
            {
                var data = inventory[i].Split(',');
                _output.Send($"Id: {data[0]} Name: {data[1]} Quantity: {data[2]}");
            }
        }

        /// <summary>
        /// Allows user to add a new item to inventory
        /// </summary>
        public void CreateItem()
        {
            int quantity;
            _output.Send("CREATE ITEM");
            _output.Send("What is the item name?");
            var itemName = _input.ReadData();
            _output.Send("How many would you like to upload into the system?");
            var itemQuantity = _input.ReadData();

            try
            {
                quantity = Int32.Parse(itemQuantity);
            }
            catch (Exception)
            {
                _output.Send("The quantity entered is invalid.");
                Thread.Sleep(1000);
                Console.Clear();
                return;
            }

            _inventoryManager.Create(itemName, quantity);
        }

        /// <summary>
        /// Allows user to add to an existing inventory item
        /// </summary>
        public void AddQuantity()
        {
            int quantity;

            // Display inventory and inventory action
            ShowAllItems();
            _output.Send("ADD TO QUANTITY");

            // Get user input
            _output.Send("What is the item id?");
            var itemId = _input.ReadData();
            _output.Send("How many would you like to upload into the system?");
            var itemQuantity = _input.ReadData();

            try
            {
                quantity = Int32.Parse(itemQuantity);
            }
            catch (Exception)
            {
                _output.Send("The quantity entered is invalid.");
                Thread.Sleep(1000);
                Console.Clear();
                return;
            }
            _inventoryManager.Add(itemId, quantity);
            Thread.Sleep(1000);
            Console.Clear();
        }

        /// <summary>
        /// Allows user to remove quantity from an exisiting inventory item
        /// </summary>
        public void RemoveQuantity()
        {
            ShowAllItems();
            int quantity;
            _output.Send("REMOVE QUANTITY");
            _output.Send("What is the item id?");
            var itemId = _input.ReadData();
            _output.Send("How many would you like to remove from the system?");
            var itemQuantity = _input.ReadData();

            try
            {
                quantity = Int32.Parse(itemQuantity);
            }
            catch (Exception)
            {
                _output.Send("The quantity entered is invalid.");
                Thread.Sleep(1000);
                Console.Clear();
                return;
            }
            _inventoryManager.Remove(itemId, quantity);
            Thread.Sleep(1000);
            Console.Clear();
        }

        /// <summary>
        /// Runs selection loop for the welcome page. Login or Register
        /// </summary>
        public void LoginOrRegister()
        {
            var response = _input.ReadData();
            if (response == "1")
            {
                _status = AppStatus.Register;
            }
            if (response == "2")
            {
                _status = AppStatus.Login;
            }
            Console.Clear();
        }

        /// <summary>
        /// Allows user to regsiter
        /// </summary>
        /// <param name="username">User input for username</param>
        /// <param name="password">User input for password</param>
        public void Register(string username, string password)
        {
            var userSuccess = _credentialManager.CreateUser(username, password);

            if (userSuccess)
            {
                _status = AppStatus.UserActions;
                _output.Send("Registration Successful");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else
            {
                Console.Clear();
                _output.Send("INFO: Your selected username already exists. Please select a different username.");
            }
        }

        /// <summary>
        /// Allow user to login
        /// </summary>
        /// <param name="username">User input for username</param>
        /// <param name="password">User input for password</param>
        public void Login(string username, string password)
        {
            var loginSuccess = _credentialManager.CheckCredential(username, password);

            if (loginSuccess)
            {
                _status = AppStatus.UserActions;
                Console.Clear();
            }
            else
            {
                Console.Clear();
                _output.Send("INFO: Login Failed");
                _output.Send("1 Try again");
                var response = _input.ReadData();
                if (response == "1")
                {
                    Console.Clear();
                    return;
                }
                else
                {
                    Console.Clear();
                    Run();
                }
            }
        }
    }
}
