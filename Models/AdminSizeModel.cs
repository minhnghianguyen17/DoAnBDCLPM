namespace SeleniumNUnitExcelAutomation.Models
{
    /// <summary>
    /// Model quản lý Kích cỡ (Admin)
    /// Map với bảng Size trong database
    /// </summary>
    public class AdminSizeModel
    {
        public int SizeId { get; set; }
        public string SizeCode { get; set; }
        public string SizeName { get; set; }
        public int IsActive { get; set; }
    }
}