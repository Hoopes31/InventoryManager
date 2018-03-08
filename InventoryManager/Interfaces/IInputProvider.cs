namespace InventoryManager
{
    public interface IInputProvider
    {
        /// <summary>
        /// Reads data from the input provider
        /// </summary>
        /// <returns>Data provided by the input</returns>
        string ReadData();
    }
}
