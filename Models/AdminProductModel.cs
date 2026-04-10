namespace SeleniumNUnitExcelAutomation.Models
{
    /// <summary>
    /// Model quản lý Sản phẩm (Admin)
    /// Map với bảng Product trong database
    /// </summary>
    public class AdminProductModel
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public string Note { get; set; }
        public int IsActive { get; set; }
    }
}