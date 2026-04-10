using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOAN_DBCLPM.Models
{
    public class OrderHistoryModel
    {
        public int ID { get; set; }
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string Total { get; set; }
        public string Status { get; set; }
    }
}
