using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOAN_DBCLPM.Models
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }

        // Nếu sản phẩm KHÔNG có size (vd: bánh)
        public double? BasePrice { get; set; }

        public int Quantity { get; set; }

        // Nếu có size (coffee)
        public List<ProductSizeModel> Sizes { get; set; }
    }

    public class ProductSizeModel
    {
        public string Size { get; set; } // M, L, XL
        public double Price { get; set; }
    }
}
