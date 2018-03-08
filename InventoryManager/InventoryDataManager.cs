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

        public void Add(string id, int quantity)
        {
            var inventory = GetAllItems();

            if (inventory.ContainsKey(id))
            {
                var exisitingQuantity = Int32.Parse(inventory[id][1]);
                quantity = exisitingQuantity + quantity;
                inventory[id][1] = quantity.ToString();
            }
            else
            {
                _output.Send("Item id does not exist.");
            }
        }

        public void Create(string name, int quantity)
        {
            throw new NotImplementedException();
        }

        public void Remove(string id, int quantity)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<string>> GetAllItems()
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

        public void EstablishInventoryData(string connectionString)
        {
            try
            {
                GetAllItems();
            }
            catch (Exception)
            {
                var establish = string.Format($"Id, Name, Quantity\r");
                File.AppendAllText(_connectionString, establish);
            }
        }
    }
}
