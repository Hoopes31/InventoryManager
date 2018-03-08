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
        public void Create(string name, int quantity)
        {
            var id = Int32.Parse(GetAllInventory().Keys.Max()) + 1;
            var establish = string.Format($"{id},{name},{quantity.ToString()}\r");
            File.AppendAllText(_connectionString, establish);
        }

        public void Add(string id, int quantity)
        {
            var inventory = GetAllInventory();

            if (inventory.ContainsKey(id))
            {
                var exisitingQuantity = Int32.Parse(inventory[id][1]);
                quantity = exisitingQuantity + quantity;
                inventory[id][1] = quantity.ToString();

                WriteAllData(inventory);
                _output.Send($"{id}-{inventory[id][0]} previous quantity: {exisitingQuantity}");
                _output.Send($"{id}-{inventory[id][0]} added quantity: {exisitingQuantity}");
                _output.Send($"{id}-{inventory[id][0]} current quantity: {quantity}");
            }
            else
            {
                _output.Send("Item id does not exist.");
            }
        }

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

                WriteAllData(inventory);
                _output.Send($"{id}-{inventory[id][0]} previous quantity: {exisitingQuantity}");
                _output.Send($"{id}-{inventory[id][0]} removed quantity: {exisitingQuantity}");
                _output.Send($"{id}-{inventory[id][0]} quantity remaining: {quantity}");
            }
            else
            {
                _output.Send("Item id does not exist.");
            }
        }

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

        public IEnumerable<string> GetAllItems()
        {
           return File.ReadAllLines(_connectionString).ToList();
        }

        public void WriteAllData(Dictionary<string, List<string>> inventory)
        {
            var data = new List<string>();

            foreach (var item in inventory)
            {
                var entry = $"{item.Key},{item.Value[0]},{item.Value[0]}";
            }
            File.WriteAllLines(_connectionString, data);
        }

        public void EstablishInventoryData(string connectionString)
        {
            try
            {
                GetAllInventory();
            }
            catch (Exception)
            {
                var establish = string.Format($"0,Item,Quantity\r");
                File.AppendAllText(_connectionString, establish);
            }
        }
    }
}
