using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager
{
    interface IOutputProvider
    {
        /// <summary>
        /// Send data to the output
        /// </summary>
        /// <param name="data">The data you want sent to the output</param>
        void Output(string data);
    }
}
