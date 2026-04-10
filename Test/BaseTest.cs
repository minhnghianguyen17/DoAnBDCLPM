using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using NUnit.Framework;
using System;

namespace SeleniumNUnitExcelAutomation.Tests
{
    public class BaseTest
    {
        protected IWebDriver Driver;
        protected ExcelConfig Config;
        protected ExcelDataProvider ExcelProvider;
        // ✅ Biến để lưu đường dẫn screenshot trong test (nếu test tự chụp)
        protected string CurrentTestScreenshot = "";

        [SetUp]
        public void Setup()
        {
            // Khởi tạo ChromeDriver
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            Driver = new ChromeDriver(options);

            Config = new ExcelConfig();
            ExcelProvider = new ExcelDataProvider();
            
            // Reset screenshot path mỗi test
            CurrentTestScreenshot = "";
        }

        /// <summary>
        /// [TearDown] sẽ chạy sau mỗi Test, dù Pass hay Fail
        /// ✅ Chụp ảnh tự động + Ghi vào Excel nếu test fail
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (Driver != null)
            {
                try
                {
                    // Kiểm tra test có fail không
                    if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                    {
                        string testName = TestContext.CurrentContext.Test.Name;
                        
                        // ✅ Nếu test đã tự chụp ảnh (CurrentTestScreenshot không rỗng) thì KHÔNG chụp lại
                        if (string.IsNullOrEmpty(CurrentTestScreenshot))
                        {
                            // Chỉ chụp ảnh nếu test chưa tự chụp
                            CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testName);
                            Console.WriteLine($"[BaseTest] Auto Screenshot: {CurrentTestScreenshot}");
                        }
                        else
                        {
                            Console.WriteLine($"[BaseTest] Screenshot đã được lưu từ test: {CurrentTestScreenshot}");
                        }

                        // ✅ Ghi đường dẫn ảnh vào Excel
                        try
                        {
                            ExcelProvider.UpdateTestResult(Config, testName, "All", 
                                "Test failed - xem screenshot", "FAIL", CurrentTestScreenshot);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[BaseTest] Không thể ghi Excel: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[BaseTest] Lỗi trong TearDown: {ex.Message}");
                }

                // Đóng và giải phóng driver
                try
                {
                    Driver.Quit();
                }
                catch { }

                try
                {
                    Driver.Dispose();
                }
                catch { }
            }
        }
    }
}