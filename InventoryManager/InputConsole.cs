using System;

namespace InventoryManager
{
    public class InputConsole : IInputProvider
    {
        public string ReadData()
        {
            var data = Console.ReadLine();
            return data;
        }
    }
}
