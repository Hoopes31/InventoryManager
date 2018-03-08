namespace InventoryManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentialData = @"C:\Users\v-dohoop\Desktop\credential.csv";
            var inventoryData = @"C:\Users\v-dohoop\Desktop\inventory.csv";

            var logic = new AppLogic(credentialData, inventoryData);
            logic.Run();
        }
    }
}
