namespace InventoryManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentialData = @"C:\Users\v-dohoop\Desktop\credential.csv";
            var inventoryData = @"C:\Users\v-dohoop\Desktop\inventory.csv";
            var output = new OutputConsole();
            var input = new InputConsole();
            var credentialManager = new CredentialManager(credentialData, output);
            var inventoryManager = new InventoryDataManager(inventoryData, output);
            var logic = new AppLogic(output, input, credentialManager, inventoryManager);
            logic.Run();
        }
    }
}
