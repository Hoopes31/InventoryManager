using System.Collections.Generic;

namespace InventoryManager
{
    public interface IDataManager
    {
        /// <summary>
        /// Creates a new item in the database
        /// </summary>
        /// <param name="name">The name of the item you would like to add to the database</param>
        /// <param name="quantity">The quantity of the item you would like to add to the database</param>
        void Create(string name, int quantity);

        /// <summary>
        /// Adds to the quantity of an item in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity">The quantity of the item you would like to add to the database</param>
        void Add(string id, int quantity);

        /// <summary>
        /// Removes from the quantity of an item in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity">The quantity of the item you would like to add to the database</param>
        void Remove(string id, int quantity);

        /// <summary>
        /// Returns all items in the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllItems();
    }
}
