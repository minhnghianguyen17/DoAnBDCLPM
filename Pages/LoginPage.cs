using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Models;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;
using NUnit.Framework;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;

        // Locators
        private readonly By DropdownToggle = By.CssSelector("i.bi.bi-person-fill.dropdown-toggle");
        private readonly By LoginLink = By.XPath("//a[contains(text(),'Đăng nhập')]");
        private readonly By EmailField = By.Id("EmailForm");
        private readonly By PasswordField = By.Id("PassWordForm");
        private readonly By LoginButton = By.XPath("//button[contains(text(),'Đăng nhập')]");
        private readonly By RegisterLink = By.XPath("//a[contains(@class,'link-danger') and contains(text(),'Đăng ký')]");

        public LoginPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        public void NavigateToLogin()
        {
            try
            {
                _driver.FindElement(DropdownToggle).Click();
                Thread.Sleep(1000);                    // Dừng 2 giây quan sát dropdown

                _driver.FindElement(LoginLink).Click();
                Thread.Sleep(1000);                    // Dừng 2 giây để trang Login load
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể mở menu đăng nhập: " + ex.Message);
            }
        }

        /// <summary>
        /// ✅ THAY ĐỔI: Dùng ILoginCredentials thay vì AccountModel
        /// Vậy có thể dùng cả AccountModel lẫn LogoutModel
        /// </summary>
        public void LoginWithAccount(ILoginCredentials credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            NavigateToLogin();

            try
            {
                _driver.FindElement(EmailField).Clear();
                _driver.FindElement(EmailField).SendKeys(credentials.Email);
                Thread.Sleep(1000);                    // Quan sát nhập Email

                _driver.FindElement(PasswordField).Clear();
                _driver.FindElement(PasswordField).SendKeys(credentials.Password);
                Thread.Sleep(1000);                    // Quan sát nhập Password

                _driver.FindElement(LoginButton).Click();
                Thread.Sleep(1000);                    // Quan sát sau khi click Đăng nhập

                Console.WriteLine($"Đã nhập Email: {credentials.Email} và Password");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thực hiện LoginWithAccount: {ex.Message}");
            }
        }

        public void VerifyElementExists(By locator, string testCaseId, string stepNumber, string expectedMessage)
        {
            string actualResult = "";
            string status = "FAIL";
            string notes = "";

            try
            {
                var element = _driver.FindElement(locator);

                Assert.IsTrue(element.Displayed, 
                    $"[{testCaseId}-{stepNumber}] {expectedMessage} KHÔNG hiển thị");

                actualResult = expectedMessage + " - Tồn tại và hiển thị";
                status = "PASS";
                
                // ✅ Ghi kết quả PASS vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber, actualResult, status, notes);
            }
            catch (Exception ex)
            {
                actualResult = ex.Message;
                notes = ScreenshotHelper.TakeScreenshot(_driver, testCaseId);

                // ✅ Ghi kết quả FAIL vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber, actualResult, status, notes);

                // ✅ Assert fail để test runner báo lỗi
                Assert.Fail($"[{testCaseId}-{stepNumber}] FAIL: {ex.Message}");
            }
        }

        public void VerifyEmailFieldExists(string testCaseId, string stepNumber)
            => VerifyElementExists(EmailField, testCaseId, stepNumber, "Có ô Email / Tên đăng nhập");

        public void VerifyPasswordFieldExists(string testCaseId, string stepNumber)
            => VerifyElementExists(PasswordField, testCaseId, stepNumber, "Có ô Mật khẩu");

        public void VerifyLoginButtonExists(string testCaseId, string stepNumber)
            => VerifyElementExists(LoginButton, testCaseId, stepNumber, "Có button Đăng nhập");

        public void VerifyRegisterLinkExists(string testCaseId, string stepNumber)
            => VerifyElementExists(RegisterLink, testCaseId, stepNumber, "Có link Đăng ký");
    }
}