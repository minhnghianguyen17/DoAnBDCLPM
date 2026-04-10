namespace SeleniumNUnitExcelAutomation.Models
{
    /// <summary>
    /// Model quản lý Loại sản phẩm (Admin)
    /// Map với bảng ProductType trong database
    /// </summary>
    public class AdminProductTypeModel
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductTypeName { get; set; }
        public int IsActive { get; set; }
    }
}