using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class AdminAccountPage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;

        // ✅ Locators cho menu dropdown

        private readonly By AccountMenuParent = By.XPath("//a[@data-bs-target='#account-nav']");

        // ✅ Locators cho menu
        private readonly By AccountMenuItem = By.XPath("//a[@href='/Admin/Account']");

        // ✅ Locators cho Tabs
        private readonly By TabCustomer = By.Id("tab-customer-account");
        private readonly By TabEmployee = By.Id("tab-employee-account");
        private readonly By TabManager = By.Id("tab-manager-account");

        // ✅ Locators cho Search & Buttons
        private readonly By SearchInput = By.Id("request");
        private readonly By SearchButton = By.Id("btn_search");
        private readonly By AddButton = By.XPath("//button[@class='btn btn-success add_new']");
        private readonly By EditButton = By.XPath("//button[contains(@onclick, 'GetById')]");
        private readonly By SaveButton = By.XPath("//button[@onclick='CreateOrUpdate()']");
        private readonly By DeleteButton = By.Id("btn_deleteModal");
        private readonly By CloseButton = By.XPath("//button[@onclick='CloseModal()']");

        // ✅ Locators cho Form fields
        private readonly By FormName = By.Id("formName");
        private readonly By FormRole = By.Id("formRole");
        private readonly By FormIsActive = By.Id("formIsActive");
        private readonly By FormPhoneNumber = By.Id("formPhoneNumber");
        private readonly By FormEmail = By.Id("formEmail");
        private readonly By FormAddress = By.Id("formAddress");

        // ✅ Locators cho Table
        private readonly By TableRows = By.XPath("//table//tbody//tr");
        private readonly By TableEmails = By.XPath("//table//tbody//tr//td");

        public AdminAccountPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        /// <summary>
        /// ✅ SỬA: Mở menu sidebar rồi điều hướng đến trang Quản lý tài khoản
        /// </summary>
        public void NavigateToAccountManagement()
        {
            try
            {
                // B1: Mở menu "Tài khoản"
                var parentMenu = _driver.FindElement(AccountMenuParent);

                if (parentMenu != null && parentMenu.Displayed)
                {
                    parentMenu.Click();
                    Thread.Sleep(500);
                    Console.WriteLine("Đã mở menu Tài khoản");
                }

                // B2: Click vào "Tài khoản"
                var accountItem = _driver.FindElement(AccountMenuItem);

                if (accountItem == null || !accountItem.Displayed)
                {
                    throw new Exception("Không tìm thấy menu Tài khoản");
                }

                accountItem.Click();
                Thread.Sleep(2000);

                Console.WriteLine("✅ Đã điều hướng đến trang Quản lý tài khoản");
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể điều hướng đến trang Quản lý tài khoản: {ex.Message}");
            }
        }

        /// <summary>
        /// Nhấp vào tab Khách hàng
        /// </summary>
        public void ClickTabCustomer()
        {
            try
            {
                _driver.FindElement(TabCustomer).Click();
                Thread.Sleep(1000);
                Console.WriteLine("✅ Đã nhấp tab Khách hàng");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhấp tab Khách hàng: {ex.Message}");
            }
        }

        /// <summary>
        /// Nhấp vào tab Nhân viên
        /// </summary>
        public void ClickTabEmployee()
        {
            try
            {
                _driver.FindElement(TabEmployee).Click();
                Thread.Sleep(1000);
                Console.WriteLine("✅ Đã nhấp tab Nhân viên");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhấp tab Nhân viên: {ex.Message}");
            }
        }

        /// <summary>
        /// Nhấp vào tab Quản lý
        /// </summary>
        public void ClickTabManager()
        {
            try
            {
                _driver.FindElement(TabManager).Click();
                Thread.Sleep(1000);
                Console.WriteLine("✅ Đã nhấp tab Quản lý");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhấp tab Quản lý: {ex.Message}");
            }
        }

        /// <summary>
        /// Tìm kiếm tài khoản
        /// </summary>
        public void SearchAccount(string searchText)
        {
            try
            {
                _driver.FindElement(SearchInput).Clear();
                _driver.FindElement(SearchInput).SendKeys(searchText);
                Thread.Sleep(500);
                _driver.FindElement(SearchButton).Click();
                Thread.Sleep(1000);
                Console.WriteLine($"✅ Đã tìm kiếm: {searchText}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm: {ex.Message}");
            }
        }

        /// <summary>
        /// Nhấp nút Thêm
        /// </summary>
        public void ClickAddButton()
        {
            try
            {
                _driver.FindElement(AddButton).Click();
                Thread.Sleep(1000);
                Console.WriteLine("✅ Đã nhấp nút Thêm");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhấp nút Thêm: {ex.Message}");
            }
        }

        /// <summary>
        /// Nhấp nút Lưu
        /// </summary>
        public void ClickSaveButton()
        {
            try
            {
                _driver.FindElement(SaveButton).Click();
                Thread.Sleep(1000);
                Console.WriteLine("✅ Đã nhấp nút Lưu");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhấp nút Lưu: {ex.Message}");
            }
        }

        /// <summary>
        /// Điền thông tin form
        /// </summary>
        public void FillAccountForm(string name, string email, string phone, string address, string role, string isActive)
        {
            try
            {
                _driver.FindElement(FormName).Clear();
                _driver.FindElement(FormName).SendKeys(name);
                Thread.Sleep(300);

                _driver.FindElement(FormEmail).Clear();
                _driver.FindElement(FormEmail).SendKeys(email);
                Thread.Sleep(300);

                _driver.FindElement(FormPhoneNumber).Clear();
                _driver.FindElement(FormPhoneNumber).SendKeys(phone);
                Thread.Sleep(300);

                _driver.FindElement(FormAddress).Clear();
                _driver.FindElement(FormAddress).SendKeys(address);
                Thread.Sleep(300);

                // Chọn Role
                var roleSelect = new SelectElement(_driver.FindElement(FormRole));
                roleSelect.SelectByValue(role);
                Thread.Sleep(300);

                // Chọn IsActive
                var isActiveSelect = new SelectElement(_driver.FindElement(FormIsActive));
                isActiveSelect.SelectByValue(isActive);
                Thread.Sleep(300);

                Console.WriteLine($"✅ Đã điền form với Email: {email}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi điền form: {ex.Message}");
            }
        }

        /// <summary>
        /// Kiểm tra tài khoản có trong bảng không
        /// </summary>
        public bool IsAccountFound(string keyword, string tableId)
        {
            try
            {
                // Lấy tất cả row trong đúng bảng (theo tab)
                var rows = _driver.FindElements(By.XPath($"//tbody[@id='{tableId}']//tr"));

                if (rows.Count == 0)
                {
                    Console.WriteLine($"[Warning] Bảng {tableId} không có dữ liệu");
                    return false;
                }

                foreach (var row in rows)
                {
                    string rowText = row.Text.Trim();

                    Console.WriteLine($"[DEBUG] Row: {rowText}");

                    // 🔥 KEY: chỉ cần chứa keyword là OK
                    if (rowText.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"✅ Tìm thấy '{keyword}' trong {tableId}");
                        return true;
                    }
                }

                Console.WriteLine($"❌ Không tìm thấy '{keyword}' trong {tableId}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
                return false;
            }
        }

        public int CountAccountByKeyword(string keyword, string tbodyId)
        {
            try
            {
                var rows = _driver.FindElements(By.XPath($"//tbody[@id='{tbodyId}']//tr"));

                int count = 0;

                foreach (var row in rows)
                {
                    string rowText = row.Text;
                    Console.WriteLine($"[DEBUG] Row: {rowText}");

                    if (rowText.Contains(keyword))
                    {
                        count++;
                    }
                }

                Console.WriteLine($"[DEBUG] Found {count} record(s) with '{keyword}'");

                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return 0;
            }
        }
    }
}