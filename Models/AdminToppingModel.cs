namespace SeleniumNUnitExcelAutomation.Models
{
    /// <summary>
    /// Model quản lý Topping (Admin)
    /// Map với bảng Topping trong database
    /// </summary>
    public class AdminToppingModel
    {
        public int ToppingId { get; set; }
        public string ToppingCode { get; set; }
        public string ToppingName { get; set; }
        public int Price { get; set; }
        public int IsActive { get; set; }
    }
}