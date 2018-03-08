namespace InventoryManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentialData = @"C:\Users\v-dohoop\Desktop\credential.csv";
            var logic = new AppLogic(credentialData);
            logic.Run();
        }
    }
}
