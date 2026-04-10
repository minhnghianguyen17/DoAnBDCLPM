using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class LogoutPage
    {
        private readonly IWebDriver _driver;

        public LogoutPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // ===== LOCATORS =====

        private readonly By DropdownUser = By.CssSelector("i.bi.bi-person-fill.dropdown-toggle");
        private readonly By DropdownAdmin = By.XPath("//a[contains(@class,'nav-profile')]");

        // ✅ FIX: Logout đúng (không dùng /Home/Privacy nữa)
        private readonly By LogoutLink = By.XPath(
            "//a[@class='dropdown-item' and @href='/Home/Privacy'] | " +
            "//a[@class='dropdown-item d-flex align-items-center' and contains(@href, '/Home/Privacy')]"
        );

        private readonly By LoginButton = By.XPath("//a[contains(@href,'Login')]");

        // ===== ACTIONS =====

        public void OpenDropdown()
        {
            if (IsElementExists(DropdownUser))
            {
                _driver.FindElement(DropdownUser).Click();
                Console.WriteLine("Mở dropdown user");
            }
            else if (IsElementExists(DropdownAdmin))
            {
                _driver.FindElement(DropdownAdmin).Click();
                Console.WriteLine("Mở dropdown admin");
            }
            else
            {
                throw new Exception("Không tìm thấy dropdown");
            }

            Thread.Sleep(1000);
        }

        public void ClickLogout()
        {
            OpenDropdown();
            Thread.Sleep(500);

            if (!IsElementExists(LogoutLink))
                throw new Exception("Không tìm thấy nút Đăng xuất");

            _driver.FindElement(LogoutLink).Click();
            Console.WriteLine("Click Đăng xuất");

            Thread.Sleep(2000);
        }

        public void PerformLogout()
        {
            ClickLogout();
            Console.WriteLine("Đăng xuất thành công");
        }

        // ===== VERIFY =====

        public bool IsLoginButtonDisplayed()
        {
            try
            {
                return _driver.FindElement(LoginButton).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool IsUserLoggedIn()
        {
            return IsElementExists(DropdownUser) || IsElementExists(DropdownAdmin);
        }

        // ===== HELPER =====

        private bool IsElementExists(By locator)
        {
            try
            {
                return _driver.FindElement(locator).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}