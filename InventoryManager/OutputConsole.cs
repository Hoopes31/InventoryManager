using System;

namespace InventoryManager
{
    public class OutputConsole : IOutputProvider
    {
        public void Send(string data)
        {
            Console.WriteLine(data);
        }
    }
}
