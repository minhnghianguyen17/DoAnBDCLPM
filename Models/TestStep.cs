namespace SeleniumNUnitExcelAutomation.Models
{
    public class TestStep
    {
        public string TestCaseId { get; set; }
        public string Step { get; set; }           // Có thể là "1", "2" hoặc "TC01-Step1"
        public string StepAction { get; set; }
        public string TestData { get; set; }
        public string ExpectedResult { get; set; }

        // Dùng để ghi kết quả sau này (không cần sửa)
        public int ExcelRowIndex { get; set; }     // Lưu vị trí dòng trong Excel để ghi nhanh
    }
}