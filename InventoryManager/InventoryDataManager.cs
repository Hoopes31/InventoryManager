using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager
{
    public class InventoryDataManager : IDataManager
    {
        private readonly string _connectionString;
        private readonly IOutputProvider _output;

        public InventoryDataManager(string connectionString, IOutputProvider output)
        {
            _connectionString = connectionString;
            _output = output;
            EstablishInventoryData(_connectionString);
        }

        /// <summary>
        /// Creates a new item and saves it to the database
        /// </summary>
        /// <param name="name">Name of the item to be created</param>
        /// <param name="quantity">Quantity of the newly created item that should be saved</param>
        public void Create(string name, int quantity)
        {
            var id = Int32.Parse(GetAllInventory().Keys.Max()) + 1;
            var establish = string.Format($"{id},{name},{quantity.ToString()}\r");
            File.AppendAllText(_connectionString, establish);
        }

        /// <summary>
        /// Adds quantity to an exisiting item
        /// </summary>
        /// <param name="id">Id of the existing item</param>
        /// <param name="quantity">Quantity to add to the existing item</param>
        public void Add(string id, int quantity)
        {
            var inventory = GetAllInventory();

            if (inventory.ContainsKey(id))
            {
                var exisitingQuantity = Int32.Parse(inventory[id][1]);
                quantity = exisitingQuantity + quantity;
                inventory[id][1] = quantity.ToString();

                _output.Send($"{id}-{inventory[id][0]} previous quantity: {exisitingQuantity}");
                _output.Send($"{id}-{inventory[id][0]} added quantity: {exisitingQuantity}");
                _output.Send($"{id}-{inventory[id][0]} current quantity: {quantity}");

                WriteAllData(inventory);
            }
            else
            {
                _output.Send("Item id does not exist.");
            }
        }

        /// <summary>
        /// Removes quantity from an exisiting item
        /// </summary>
        /// <param name="id">Id of the existing item</param>
        /// <param name="quantity">Quantity to remove from the exisiting item</param>
        public void Remove(string id, int quantity)
        {
            var inventory = GetAllInventory();

            if (inventory.ContainsKey(id))
            {
                var exisitingQuantity = Int32.Parse(inventory[id][1]);
                quantity = exisitingQuantity - quantity;
                inventory[id][1] = quantity.ToString();

                if (quantity < 0)
                {
                    _output.Send("You cannot remove more items than are present in inventory.");
                    return;
                }

                _output.Send($"{id}-{inventory[id][0]} previous quantity: {exisitingQuantity}");
                _output.Send($"{id}-{inventory[id][0]} removed quantity: {exisitingQuantity}");
                _output.Send($"{id}-{inventory[id][0]} quantity remaining: {quantity}");

                WriteAllData(inventory);
            }
            else
            {
                _output.Send("Item id does not exist.");
            }
        }

        /// <summary>
        /// Creates an inventory dictionary with ids as keys
        /// </summary>
        /// <returns>Dictionary with inventory item ids as keys and a list with name and quantity as values</returns>
        private Dictionary<string, List<string>> GetAllInventory()
        {
            var inventoryData = new Dictionary<string, List<string>>();
            var data = File.ReadAllLines(_connectionString).ToList();
            data.ForEach(users =>
            {
                var inventory = users.Split(',');

                if (inventory.Any())
                {
                    inventoryData[inventory[0]] = new List<string> { inventory[1], inventory[2] };
                }
            });

            return inventoryData;
        }

        /// <summary>
        /// Gets all items from inventory
        /// </summary>
        /// <returns>All items in inventory as a list</returns>
        public List<string> GetAllItems()
        {
           return File.ReadAllLines(_connectionString).ToList();
        }

        /// <summary>
        /// Saves all data to the inventory csv file
        /// </summary>
        /// <param name="inventory">Dictionary of inventory items</param>
        public void WriteAllData(Dictionary<string, List<string>> inventory)
        {
            var data = new List<string>();

            foreach (var item in inventory)
            {
                var entry = $"{item.Key},{item.Value[0]},{item.Value[1]}";
                data.Add(entry);
            }
            File.WriteAllLines(_connectionString, data);
        }

        /// <summary>
        /// Creates an inventory csv if one does not yet exist
        /// </summary>
        /// <param name="connectionString">Path location of the inventory csv</param>
        public void EstablishInventoryData(string connectionString)
        {
            if (!File.Exists(connectionString))
            {
                var establish = string.Format($"0,0,0\r");
                File.AppendAllText(_connectionString, establish);
            }
        }
    }
}
