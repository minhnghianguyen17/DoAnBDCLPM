using System;

namespace SeleniumNUnitExcelAutomation.Utilities
{
    /// <summary>
    /// Class cấu hình Excel - Dễ dàng thay đổi đường dẫn file
    /// </summary>
    public class ExcelConfig
    {
        // === Excel Config ===
        public string ExcelFilePath { get; set; }
            = @"C:\Users\Admin\Documents\My Web Sites\DBCLPM\Nhom2112-DBCLPM.xlsx";

        public string SheetName { get; set; } = "TC-Duy Khanh";

        public int TestCaseIdColumn { get; set; } = 1;
        public int StepColumn { get; set; } = 7;
        public int StepActionColumn { get; set; } = 8;
        public int TestDataColumn { get; set; } = 9;
        public int ExpectedResultColumn { get; set; } = 10;
        public int ActualResultColumn { get; set; } = 11;
        public int StatusColumn { get; set; } = 12;
        public int NotesColumn { get; set; } = 13;

        public int StartRow { get; set; } = 1;

        // === JSON Config (MỚI) ===
        public string AccountsJsonPath { get; set; }
            = @"TestData\Json\Accounts.json";     
    }
}