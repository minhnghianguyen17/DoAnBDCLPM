using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class OrderHistoryPage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;

        // --- Locators ---
        private readonly By dateFrom = By.XPath("/html/body/div[1]/main/div[2]/div/div[1]/div[2]/div[1]/div/input");
        private readonly By dateTo = By.XPath("/html/body/div[1]/main/div[2]/div/div[1]/div[2]/div[2]/div/input");
        private readonly By keywordInput = By.XPath("/html/body/div[1]/main/div[2]/div/div[1]/div[2]/div[3]/div/input");
        private readonly By searchButton = By.XPath("/html/body/div[1]/main/div[2]/div/div[1]/div[2]/div[3]/div/button");

        private readonly By firstOrderCode = By.XPath("/html/body/div[1]/main/div[2]/div/div[2]/div[1]/div/div/div/div[1]/div[1]/ul/li[1]/span/a");
        private readonly By firstOrderTime = By.XPath("/html/body/div[1]/main/div[2]/div/div[2]/div[1]/div/div/div/div[1]/div[1]/ul/li[2]");
        private readonly By firstOrderStatusButton = By.XPath("/html/body/div[1]/main/div[2]/div/div[2]/div[1]/div/div/div/div[1]/div[1]/ul/li[3]/span");
        private readonly By firstOrderCancelButton = By.XPath("/html/body/div[1]/main/div[2]/div/div[2]/div[1]/div/div/div/div[2]/div[3]/div/button");

        private readonly By userIcon = By.XPath("/html/body/header/nav/div/div/div[2]/div[2]/div/ul/li/i");
        private readonly By orderHistoryLink = By.XPath("/html/body/header/nav/div/div/div[2]/div[2]/div/ul/li/ul/li[2]/a");

        public OrderHistoryPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        public void ExecuteTC61_CheckOrderHistory(string testCaseId)
        {
            string stepNumber = "1";
            try
            {
                // ===== 1. Click user icon -> Order history =====
                _driver.FindElement(userIcon).Click();
                Thread.Sleep(1000);
                _driver.FindElement(orderHistoryLink).Click();
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra các field tìm kiếm =====
                Assert.IsTrue(_driver.FindElement(dateFrom).Displayed, "From date không hiển thị");
                Assert.IsTrue(_driver.FindElement(dateTo).Displayed, "To date không hiển thị");
                Assert.IsTrue(_driver.FindElement(keywordInput).Displayed, "Keyword input không hiển thị");
                Assert.IsTrue(_driver.FindElement(searchButton).Displayed, "Search button không hiển thị");

               

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Tất cả element lịch sử đơn hàng hiển thị đúng", "PASS", "", "");

                Console.WriteLine($"[{testCaseId}] PASS: Tất cả element lịch sử đơn hàng hiển thị đầy đủ");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }

        public void ExecuteTC62_CheckDateRange(string testCaseId)
        {
            string stepNumber = "2";
            try
            {
                // ===== 1. Click user icon -> Order history =====
                _driver.FindElement(userIcon).Click();
                Thread.Sleep(1000);
                _driver.FindElement(orderHistoryLink).Click();
                Thread.Sleep(2000);

                // ===== 2. Lấy ngày đầu và cuối tháng hiện tại để làm mốc so sánh =====
                DateTime now = DateTime.Now;
                DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                // Định dạng yyyy-MM-dd để so sánh với giá trị của input type="date"
                string expectedFirstDay = firstDay.ToString("yyyy-MM-dd");
                string expectedLastDay = lastDay.ToString("yyyy-MM-dd");

                // ===== 3. Lấy giá trị đang hiển thị sẵn trong ô Input (Chỉ xem, không nhập) =====
                string actualFrom = _driver.FindElement(dateFrom).GetAttribute("value");
                string actualTo = _driver.FindElement(dateTo).GetAttribute("value");

                // ===== 4. Kiểm tra so sánh =====
                bool isFromDateCorrect = (actualFrom == expectedFirstDay);
                bool isToDateCorrect = (actualTo == expectedLastDay);

                if (!isFromDateCorrect || !isToDateCorrect)
                {
                    throw new Exception($"Ngày hiển thị mặc định không đúng! " +
                                        $"Mong đợi: {expectedFirstDay} tới {expectedLastDay}. " +
                                        $"Thực tế: {actualFrom} tới {actualTo}");
                }

                // ===== 5. Ghi kết quả Excel =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    $"Kiểm tra xem ngày mặc định: Từ ngày {actualFrom} và Đến ngày {actualTo} là chính xác.",
                    "PASS", "", "");

                Console.WriteLine($"[{testCaseId}] PASS: Ngày mặc định hiển thị đúng đầu tháng và cuối tháng.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
    }
}