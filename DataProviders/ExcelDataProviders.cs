using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SeleniumNUnitExcelAutomation.Models;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace SeleniumNUnitExcelAutomation.DataProviders
{
    public class ExcelDataProvider
    {
        public List<TestCase> ReadTestCases(ExcelConfig config)
        {
            var testCases = new Dictionary<string, TestCase>();

            using (var fileStream = new FileStream(config.ExcelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheet(config.SheetName);

                if (sheet == null)
                    throw new Exception($"Không tìm thấy sheet: {config.SheetName}");

                for (int i = config.StartRow; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    string testCaseId = GetCellValue(row, config.TestCaseIdColumn);
                    if (string.IsNullOrWhiteSpace(testCaseId)) continue;

                    var step = new TestStep
                    {
                        TestCaseId = testCaseId,
                        Step = GetCellValue(row, config.StepColumn),
                        StepAction = GetCellValue(row, config.StepActionColumn),
                        TestData = GetCellValue(row, config.TestDataColumn),
                        ExpectedResult = GetCellValue(row, config.ExpectedResultColumn),
                        ExcelRowIndex = i
                    };

                    if (!testCases.ContainsKey(testCaseId))
                        testCases[testCaseId] = new TestCase { TestCaseId = testCaseId };

                    testCases[testCaseId].Steps.Add(step);
                }
            }

            return new List<TestCase>(testCases.Values);
        }

        /// <summary>
        /// Ghi kết quả vào Excel - Phiên bản GỐC (dùng cho Login, Logout, Register, AdminAccount, AdminProduct)
        /// </summary>
        public void UpdateTestResult(ExcelConfig config, string testCaseId, string stepNumber,
            string actualResult, string status, string notes)
        {
            try
            {
                // Đọc file trước
                IWorkbook workbook;
                using (var readStream = new FileStream(config.ExcelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    workbook = new XSSFWorkbook(readStream);
                }

                // Sau đó ghi
                ISheet sheet = workbook.GetSheet(config.SheetName);
                if (sheet == null)
                    throw new Exception($"Không tìm thấy sheet: {config.SheetName}");

                bool updated = false;

                for (int i = config.StartRow; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    string currentTcId = GetCellValue(row, config.TestCaseIdColumn);
                    string currentStep = GetCellValue(row, config.StepColumn);

                    if (currentTcId == testCaseId && currentStep == stepNumber)
                    {
                        SetCellValue(row, config.ActualResultColumn, actualResult);
                        SetCellValue(row, config.StatusColumn, status);
                        SetCellValue(row, config.NotesColumn, notes);
                        updated = true;
                        break;
                    }
                }

                if (updated)
                {
                    // Ghi lại file bằng cách mở stream mới
                    using (var writeStream = new FileStream(config.ExcelFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        workbook.Write(writeStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }

        /// <summary>
        /// 🔥 Ghi kết quả vào Excel - PHIÊN BẢN MỚI (dùng cho AccountDetailsPage)
        /// Overload hỗ trợ thêm parameter testData
        /// </summary>
        public void UpdateTestResult(ExcelConfig config, string testCaseId, string stepNumber,
            string actualResult, string status, string notes, string testData)
        {
            try
            {
                IWorkbook workbook;
                using (var readStream = new FileStream(config.ExcelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    workbook = new XSSFWorkbook(readStream);
                }

                ISheet sheet = workbook.GetSheet(config.SheetName);
                bool updated = false;
                string lastTestCaseId = "";

                for (int i = config.StartRow; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    string currentTcId = GetCellValue(row, config.TestCaseIdColumn);
                    if (!string.IsNullOrWhiteSpace(currentTcId)) lastTestCaseId = currentTcId;

                    string currentStep = GetCellValue(row, config.StepColumn);

                    if (lastTestCaseId == testCaseId && currentStep == stepNumber)
                    {
                        // 🔥 Cột J: Test Data (Index 9)
                        SetCellValue(row, config.TestDataColumn, testData);

                        // 🔥 Cột K: Actual Result (Index 10)
                        SetCellValue(row, config.ActualResultColumn, actualResult);

                        // 🔥 Cột M: Status (Index 12)
                        SetCellValue(row, config.StatusColumn, status);

                        // 🔥 Cột N: Screenshot/Notes (Index 13)
                        SetCellValue(row, config.NotesColumn, notes);

                        updated = true;
                        break;
                    }
                }

                if (updated)
                {
                    using (var writeStream = new FileStream(config.ExcelFilePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(writeStream);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); }
        }

        private string GetCellValue(IRow row, int columnIndex)
        {
            var cell = row.GetCell(columnIndex);
            return cell?.ToString()?.Trim() ?? "";
        }

        private void SetCellValue(IRow row, int columnIndex, string value)
        {
            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            cell.SetCellValue(value ?? "");
        }
    }
}