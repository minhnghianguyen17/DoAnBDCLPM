using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Models;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class RegisterPage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;

        // Locators
        private readonly By DropdownToggle = By.CssSelector("i.bi.bi-person-fill.dropdown-toggle");
        private readonly By RegisterLink = By.XPath("//a[@class='dropdown-item' and contains(text(),'Đăng ký')]");
        private readonly By FullNameInput = By.Id("formFullName");
        private readonly By EmailInput = By.Id("formEmail");
        private readonly By PasswordInput = By.Id("formPassword");
        private readonly By ConfirmPasswordInput = By.Id("formPasswordConfirm");
        private readonly By TermsCheckbox = By.Id("formCheck");
        private readonly By RegisterButton = By.XPath("//button[contains(text(),'Đăng ký')]");
        private readonly By LoginButton = By.XPath("//button[contains(text(),'Đăng nhập')]");

        // ? Constructor v?i 3 parameters
        public RegisterPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        // ? Constructor ??n gi?n (ch? driver)
        public RegisterPage(IWebDriver driver)
        {
            _driver = driver;
            _config = null;
            _excelProvider = null;
        }

        /// <summary>
        /// M? trang ??ng ký t? trang ch?
        /// </summary>
        public void NavigateToRegister()
        {
            try
            {
                Thread.Sleep(1500);

                // ? M? dropdown
                _driver.FindElement(By.CssSelector("i.bi.bi-person-fill.dropdown-toggle")).Click();
                Thread.Sleep(1500);

                // ? Click "??ng ký" - dùng href attribute
                _driver.FindElement(By.XPath("//a[@href='/Account/Register']")).Click();
                Thread.Sleep(2500);

                Console.WriteLine("? ?ã m? trang ??ng ký thành công");
            }
            catch (Exception ex)
            {
                throw new Exception("? Không th? m? trang ??ng ký: " + ex.Message);
            }
        }

        /// <summary>
        /// ?i?n form ??ng ký
        /// </summary>
        public void FillRegistrationForm(RegisterModel register)
        {
            if (register == null)
                throw new ArgumentNullException(nameof(register));

            try
            {
                // ?i?n H? và Tên
                _driver.FindElement(FullNameInput).Clear();
                _driver.FindElement(FullNameInput).SendKeys(register.FullName ?? "");
                Thread.Sleep(500);

                // ?i?n Email
                _driver.FindElement(EmailInput).Clear();
                _driver.FindElement(EmailInput).SendKeys(register.Email ?? "");
                Thread.Sleep(500);

                // ?i?n M?t kh?u
                _driver.FindElement(PasswordInput).Clear();
                _driver.FindElement(PasswordInput).SendKeys(register.Password ?? "");
                Thread.Sleep(500);

                // ?i?n Nh?p l?i M?t kh?u
                _driver.FindElement(ConfirmPasswordInput).Clear();
                _driver.FindElement(ConfirmPasswordInput).SendKeys(register.ConfirmPassword ?? "");
                Thread.Sleep(500);

                // Tích vào checkbox ?i?u kho?n n?u agreeTerms = true
                if (register.AgreeTerms)
                {
                    var checkbox = _driver.FindElement(TermsCheckbox);
                    if (!checkbox.Selected)
                    {
                        checkbox.Click();
                    }
                    Thread.Sleep(500);
                }

                Console.WriteLine($"?ã ?i?n form ??ng ký v?i Email: {register.Email}");
            }
            catch (Exception ex)
            {
                throw new Exception($"L?i khi ?i?n form ??ng ký: {ex.Message}");
            }
        }

        /// <summary>
        /// Click nút ??ng ký
        /// </summary>
        public void ClickRegisterButton()
        {
            try
            {
                _driver.FindElement(RegisterButton).Click();
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể click nút đăng ký: " + ex.Message);
            }
        }

        /// <summary>
        /// ??ng ký tài kho?n (?i?n form + click button)
        /// </summary>
        public void RegisterWithData(RegisterModel register)
        {
            FillRegistrationForm(register);
            ClickRegisterButton();
        }

        /// <summary>
        /// Ki?m tra element có t?n t?i và hi?n th? không
        /// </summary>
        public void VerifyElementExists(By locator, string testCaseId, string stepNumber, string expectedMessage)
        {
            string actualResult = "";
            string status = "FAIL";
            string notes = "";

            try
            {
                var element = _driver.FindElement(locator);

                NUnit.Framework.Assert.IsTrue(element.Displayed, 
                    $"[{testCaseId}-{stepNumber}] {expectedMessage} KHÔNG hi?n th?");

                actualResult = expectedMessage + " - T?n t?i và hi?n th?";
                status = "PASS";
                
                // ? Ghi k?t qu? PASS vào Excel
                if (_config != null && _excelProvider != null)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber, actualResult, status, notes);
                }
            }
            catch (Exception ex)
            {
                actualResult = ex.Message;
                
                // ? Ch?p ?nh khi fail
                if (_config != null && _excelProvider != null)
                {
                    notes = ScreenshotHelper.TakeScreenshot(_driver, testCaseId);
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber, actualResult, status, notes);
                }

                // ? Throw exception ?? test runner báo fail
                throw;
            }
        }
    }
}