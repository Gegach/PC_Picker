using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Picker.Models
{
    public class Storage : Component
    {
        public int Capacity { get; set; }
        public string StorageType { get; set; }
        public string Interface { get; set; }
    }
}
