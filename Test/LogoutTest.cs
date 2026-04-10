using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Pages;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class LogoutTest : BaseTest
    {
        private LoginPage _loginPage;
        private LogoutPage _logoutPage;
        private JsonDataProvider _jsonProvider;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _logoutPage = new LogoutPage(Driver); // ✅ chỉ còn 1 tham số
            _jsonProvider = new JsonDataProvider(Config);
        }

        /// <summary>
        /// TC31: Kiểm tra đăng xuất tài khoản khách hàng thành công
        /// </summary>
        [Test]
        public void TC31_Logout()
        {
            string testCaseId = "TC31";

            var logoutData = _jsonProvider.GetLogoutById(1);
            Assert.IsNotNull(logoutData, $"[{testCaseId}] Không tìm thấy logout data");

            try
            {
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(logoutData);
                Thread.Sleep(1000);

                _logoutPage.PerformLogout();
                Thread.Sleep(1000);

                // ✅ CUSTOM FAIL (gọn)
                if (!Driver.Url.Contains("/Home/Privacy"))
                {
                    Assert.Fail($"[{testCaseId}] Không chuyển về trang đăng nhập sau logout");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    "Đăng xuất tài khoản khách hàng thành công", "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (Exception ex)
            {
                string screenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", screenshot);

                throw;
            }
        }

        /// <summary>
        /// TC32: Thực hiện đăng xuất và kiểm tra chuyển hướng về trang chủ
        /// </summary>
        [Test]
        public void TC32_Logout()
        {
            string testCaseId = "TC32";

            var logoutData = _jsonProvider.GetLogoutById(0);
            Assert.IsNotNull(logoutData, $"[{testCaseId}] Không tìm thấy logout data");

            try
            {
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(logoutData);
                Thread.Sleep(1000);

                _logoutPage.PerformLogout();
                Thread.Sleep(1000);

                // ✅ CUSTOM FAIL (gọn)
                if (!Driver.Url.Contains("/Home/Privacy"))
                {
                    Assert.Fail($"[{testCaseId}] Không chuyển về trang đăng nhập sau logout");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    "Đăng xuất tài khoản Admin thành công", "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (Exception ex)
            {
                string screenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", screenshot);

                throw;
            }
        }

        /// <summary>
        /// TC33: Kiểm tra không thể truy cập tài nguyên bảo vệ sau đăng xuất
        /// </summary>
        [Test]
        public void TC33_Logout()
        {
            string testCaseId = "TC33";

            var logoutData = _jsonProvider.GetLogoutById(2);
            Assert.IsNotNull(logoutData, $"[{testCaseId}] Không tìm thấy logout data");

            try
            {
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(logoutData);
                Thread.Sleep(1000);

                _logoutPage.PerformLogout();
                Thread.Sleep(1000);

                // ✅ CUSTOM FAIL (gọn)
                if (!Driver.Url.Contains("/Home/Privacy"))
                {
                    Assert.Fail($"[{testCaseId}] Không chuyển về trang đăng nhập sau logout");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    "Đăng xuất tài khoản Nhân viên thành công", "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (Exception ex)
            {
                string screenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", screenshot);

                throw;
            }
        }

        [Test]
        public void TC34_Logout()
        {
            string testCaseId = "TC34";

            var logoutData = _jsonProvider.GetLogoutById(0);
            Assert.IsNotNull(logoutData, $"[{testCaseId}] Không tìm thấy logout data");

            try
            {
                Driver.Navigate().GoToUrl("https://localhost:7116/");

                _loginPage.LoginWithAccount(logoutData);

                _logoutPage.PerformLogout();

                // Verify logout
                if (_logoutPage.IsUserLoggedIn())
                {
                    Assert.Fail($"[{testCaseId}] Logout thất bại");
                }

                // 🔥 Back
                Driver.Navigate().Back();
                Thread.Sleep(1000);

                // 🔥 Verify KHÔNG được còn login
                if (_logoutPage.IsUserLoggedIn())
                {
                    Assert.Fail($"[{testCaseId}] Nhấn Back nhưng vẫn còn đăng nhập");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    "Logout + Back OK", "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
        }
    }
}