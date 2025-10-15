using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Picker.Models
{
    public class Statistic
    {
        public int StatisticId { get; set; }
        public int ComponentId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; }
        public int Quantity { get; set; }
    }
}
