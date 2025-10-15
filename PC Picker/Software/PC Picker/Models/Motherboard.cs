using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Picker.Models
{
    public class Motherboard : Component
    {
        public string Chipset { get; set; }
        public string Socket { get; set; }
        public string FormFactor { get; set; }
        public int RamSlots { get; set; }
    }
}
