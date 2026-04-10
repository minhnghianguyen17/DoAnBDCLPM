using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOAN_DBCLPM.Models
{
    public class HomeModel
    {
        public int ID { get; set; } // ID để lấy theo GetHomeById
        public string BannerText { get; set; }
        public string[] Products { get; set; }
        public string SearchProduct { get; set; }
    }
}
