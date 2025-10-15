using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Picker.Models
{
    public class GraphicsCard : Component
    {
        public int Memory { get; set; }
        public string MemoryType { get; set; }
        public string GPU { get; set; }
        public string Interface { get; set; }
    }
}
