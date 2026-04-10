using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Pages;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class AdminAccountTest : BaseTest
    {
        private LoginPage _loginPage;
        private AdminAccountPage _adminAccountPage;
        private JsonDataProvider _jsonProvider;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _adminAccountPage = new AdminAccountPage(Driver, Config, ExcelProvider);
            _jsonProvider = new JsonDataProvider(Config);
        }

        /// <summary>
        /// TC37: Kiểm tra Thêm tài khoản mới với thông tin hợp lệ và tìm kiếm để xác nhận
        /// </summary>
        [Test]
        public void TC37_AdminAccount()
        {
            string testCaseId = "TC37";

            try
            {
                // ===== 1. Lấy dữ liệu =====
                var adminAccount = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(adminAccount, $"[{testCaseId}] Không tìm thấy account ID 0 (Admin)");

                var newAccount = _jsonProvider.GetAdminAccountByIndex(0);
                Assert.IsNotNull(newAccount, $"[{testCaseId}] Không tìm thấy admin account index 0");

                // ===== 2. Mở trang =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                // ===== 3. Login =====
                _loginPage.LoginWithAccount(adminAccount);
                Thread.Sleep(1000);

                // ===== 4. Vào trang quản lý =====
                _adminAccountPage.NavigateToAccountManagement();
                Thread.Sleep(1000);

                // ===== 5. Thêm tài khoản =====
                _adminAccountPage.ClickAddButton();
                Thread.Sleep(1000);

                _adminAccountPage.FillAccountForm(
                    newAccount.FullName,
                    newAccount.Email,
                    newAccount.PhoneNumber,
                    newAccount.Address,
                    newAccount.Role.ToString(),
                    newAccount.IsActive.ToString()
                );
                Thread.Sleep(1000);

                _adminAccountPage.ClickSaveButton();
                Thread.Sleep(1000);

                // ===== 6. SEARCH + VERIFY (SMART) =====
                bool found = false;

                // 🔍 Tab Customer
                _adminAccountPage.ClickTabCustomer();
                Thread.Sleep(1000);

                _adminAccountPage.SearchAccount(newAccount.Email);
                Thread.Sleep(1000);

                found = _adminAccountPage.IsAccountFound(newAccount.Email, "tbodyCus");

                // 🔍 Tab Employee
                if (!found)
                {
                    _adminAccountPage.ClickTabEmployee();
                    Thread.Sleep(1000);

                    _adminAccountPage.SearchAccount(newAccount.Email);
                    Thread.Sleep(1000);

                    found = _adminAccountPage.IsAccountFound(newAccount.Email, "tbodyEmp");
                }

                // 🔍 Tab Manager
                if (!found)
                {
                    _adminAccountPage.ClickTabManager();
                    Thread.Sleep(1000);

                    _adminAccountPage.SearchAccount(newAccount.Email);
                    Thread.Sleep(1000);

                    found = _adminAccountPage.IsAccountFound(newAccount.Email, "tbodyManager");
                }

                // ===== 7. ASSERT =====
                if (!found)
                {
                    Assert.Fail($"[{testCaseId}] Không tìm thấy tài khoản email '{newAccount.Email}' ở bất kỳ tab nào");
                }

                // ===== 8. PASS =====
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Thêm tài khoản '{newAccount.Email}' thành công",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
        }

        [Test]
        public void TC38_AdminAccount()
        {
            string testCaseId = "TC38";

            try
            {
                // ===== 1. Lấy dữ liệu =====
                var adminAccount = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(adminAccount, $"[{testCaseId}] Không tìm thấy account Admin");

                var existAccount = _jsonProvider.GetAdminAccountByIndex(0);
                Assert.IsNotNull(existAccount, $"[{testCaseId}] Không tìm thấy account để test trùng");

                // ===== 2. Mở trang =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                // ===== 3. Login =====
                _loginPage.LoginWithAccount(adminAccount);
                Thread.Sleep(2000);

                // ===== 4. Vào trang quản lý =====
                _adminAccountPage.NavigateToAccountManagement();
                Thread.Sleep(1000);

                // ===== 5. Click thêm =====
                _adminAccountPage.ClickAddButton();
                Thread.Sleep(1000);

                // ===== 6. Nhập lại EMAIL đã tồn tại =====
                _adminAccountPage.FillAccountForm(
                    existAccount.FullName,
                    existAccount.Email, // 🔥 email trùng
                    existAccount.PhoneNumber,
                    existAccount.Address,
                    existAccount.Role.ToString(),
                    existAccount.IsActive.ToString()
                );
                Thread.Sleep(1000);

                _adminAccountPage.ClickSaveButton();
                Thread.Sleep(2000);

                // ===== 7. VERIFY (HANDLE ALERT) =====
                bool isDuplicateError = false;

                try
                {
                    WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));

                    IAlert alert = null;

                    // Tự wait alert (loop)
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            alert = Driver.SwitchTo().Alert();
                            break;
                        }
                        catch (NoAlertPresentException)
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    if (alert != null)
                    {
                        string alertText = alert.Text.ToLower();
                        Console.WriteLine($"[DEBUG] Alert: {alertText}");

                        if (alertText.Contains("trùng") || alertText.Contains("tồn tại"))
                        {
                            isDuplicateError = true;
                        }

                        alert.Accept();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] {ex.Message}");
                }

                // ===== 8. ASSERT =====
                if (!isDuplicateError)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị lỗi trùng email");
                }

                // ===== 9. PASS =====
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Không cho phép thêm email trùng: {existAccount.Email}",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
        }

        [Test]
        public void TC40_AdminAccount()
        {
            string testCaseId = "TC40";

            try
            {
                var adminAccount = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(adminAccount, $"[{testCaseId}] Không tìm thấy account Admin");

                var existAccount = _jsonProvider.GetAdminAccountByIndex(1);
                Assert.IsNotNull(existAccount, $"[{testCaseId}] Không tìm thấy account index 1");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(adminAccount);
                Thread.Sleep(1000);

                _adminAccountPage.NavigateToAccountManagement();
                Thread.Sleep(1000);

                _adminAccountPage.ClickAddButton();
                Thread.Sleep(1000);

                _adminAccountPage.FillAccountForm(
                    existAccount.FullName,
                    existAccount.Email,
                    existAccount.PhoneNumber,
                    existAccount.Address,
                    existAccount.Role.ToString(),
                    existAccount.IsActive.ToString()
                );

                Thread.Sleep(1000);
                _adminAccountPage.ClickSaveButton();
                Thread.Sleep(1000);

                // ===== CHECK ALERT =====
                try
                {
                    IAlert alert = null;

                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            alert = Driver.SwitchTo().Alert();
                            break;
                        }
                        catch (NoAlertPresentException)
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    if (alert != null)
                    {
                        string text = alert.Text.ToLower();
                        alert.Accept();

                        if (text.Contains("trùng") || text.Contains("tồn tại"))
                        {
                            ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                                "Hệ thống chặn trùng SĐT", "PASS", "");

                            Console.WriteLine($"[{testCaseId}] Passed (Có alert)");
                            return;
                        }
                    }
                }
                catch { }

                // ===== TAB CUSTOMER =====
                _adminAccountPage.ClickTabCustomer();
                Thread.Sleep(1000);

                _adminAccountPage.SearchAccount(existAccount.PhoneNumber);
                Thread.Sleep(1000);

                int countCus = _adminAccountPage.CountAccountByKeyword(existAccount.PhoneNumber, "tbodyCus");

                if (countCus > 1)
                {
                    CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                    Assert.Fail($"[{testCaseId}] Trùng SĐT ở tab Khách hàng");
                }

                // ===== TAB EMPLOYEE =====
                _adminAccountPage.ClickTabEmployee();
                Thread.Sleep(1000);

                _adminAccountPage.SearchAccount(existAccount.PhoneNumber);
                Thread.Sleep(1000);

                int countEmp = _adminAccountPage.CountAccountByKeyword(existAccount.PhoneNumber, "tbodyEmp");

                if (countEmp > 1)
                {
                    CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                    Assert.Fail($"[{testCaseId}] Trùng SĐT ở tab Nhân viên");
                }

                // ===== TAB MANAGER =====
                _adminAccountPage.ClickTabManager();
                Thread.Sleep(1000);

                _adminAccountPage.SearchAccount(existAccount.PhoneNumber);
                Thread.Sleep(1000);

                int countMng = _adminAccountPage.CountAccountByKeyword(existAccount.PhoneNumber, "tbodyManager");

                if (countMng > 1)
                {
                    CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                    Assert.Fail($"[{testCaseId}] Trùng SĐT ở tab Quản lý");
                }

                // ===== PASS =====
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    "Không bị trùng SĐT", "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
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