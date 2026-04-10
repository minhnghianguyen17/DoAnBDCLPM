namespace SeleniumNUnitExcelAutomation.Models
{
    /// <summary>
    /// Model quản lý Giá sản phẩm (Admin)
    /// Map với bảng ProductPrice trong database
    /// </summary>
    public class AdminProductPriceModel
    {
        public int ProductPriceId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public string Price { get; set; }
        public string Unit { get; set; }
    }
}