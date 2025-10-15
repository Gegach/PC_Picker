using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Picker.Models
{
    public class Processor : Component
    {
        public int CoreCount { get; set; }
        public float Frequency { get; set; }
        public string Socket { get; set; }
        public int Cache {  get; set; }
    }
}
