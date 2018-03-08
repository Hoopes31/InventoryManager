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

        public AppLogic(string credentialData)
        {
            _output = new OutputConsole();
            _input = new InputConsole();
            _credentialManager = new CredentialManager(credentialData, _output);
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
                Console.ReadKey();
            }

            if (_status == AppStatus.CloseApp)
            {
                _output.Send("Goodbye");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }

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

        public void Register(string username, string password)
        {
            var userSuccess = _credentialManager.CreateUser(username, password);

            if (userSuccess)
            {
                Console.Clear();
                _status = AppStatus.UserActions;
                _output.Send("Registration Successful");
                Thread.Sleep(1500);
            }
            else
            {
                Console.Clear();
                _output.Send("INFO: Your selected username already exists. Please select a different username.");
            }
        }

        public void Login(string username, string password)
        {
            var loginSuccess = _credentialManager.CheckCredential(username, password);

            if (loginSuccess)
            {
                _status = AppStatus.UserActions;
            }
            else
            {
                Console.Clear();
                _output.Send("INFO: Login Failed");
            }
        }
    }
}
