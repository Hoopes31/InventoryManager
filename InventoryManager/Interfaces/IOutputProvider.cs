namespace InventoryManager
{
    public interface IOutputProvider
    {
        /// <summary>
        /// Send data to the output
        /// </summary>
        /// <param name="data">The data you want sent to the output</param>
        void Send(string data);
    }
}
