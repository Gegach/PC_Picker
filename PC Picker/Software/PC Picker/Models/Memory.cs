using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Picker.Models
{
    public class Memory : Component
    {
        public int Capacity { get; set; }
        public string MemoryType { get; set; }
        public int Speed { get; set; }
    }
}
