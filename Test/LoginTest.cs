using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Models;
using SeleniumNUnitExcelAutomation.Pages;
using SeleniumNUnitExcelAutomation.Utilities;
using NUnit.Framework;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class LoginTest : BaseTest
    {
        private LoginPage _loginPage;
        private JsonDataProvider _jsonProvider;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _jsonProvider = new JsonDataProvider(Config);
        }

        [Test]
        public void TC1_Login()
        {
            string testCaseId = "TC1";

            try
            {
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                _loginPage.NavigateToLogin();
                Thread.Sleep(2000);

                _loginPage.VerifyElementExists(By.Id("EmailForm"), testCaseId, "1", "Có ô Email / Tên đăng nhập");
                _loginPage.VerifyElementExists(By.Id("PassWordForm"), testCaseId, "2", "Có ô Mật khẩu");
                _loginPage.VerifyElementExists(By.XPath("//button[contains(text(),'Đăng nhập')]"), testCaseId, "3", "Có button Đăng nhập");
                _loginPage.VerifyElementExists(By.XPath("//a[contains(@class,'link-danger') and contains(text(),'Đăng ký')]"), testCaseId, "4", "Có link Đăng ký");

                Console.WriteLine("TC1 Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh + ghi vào CurrentTestScreenshot để BaseTest không chụp lại
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "All", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }

        [Test]
        public void TC2_Login()
        {
            string testCaseId = "TC2";

            try
            {
                var account = _jsonProvider.GetAccountById(2);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 2");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                string currentUrl = Driver.Url;
                Assert.That(currentUrl, Does.Not.Contain("/Home/Privacy"),
                    $"[{testCaseId}] Vẫn ở trang login → Đăng nhập thất bại");

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Đăng nhập thành công với Email: {account.Email}",
                    "PASS", "");

                Console.WriteLine($"TC2 Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh + ghi vào CurrentTestScreenshot
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }

        [Test]
        public void TC3_Login()
        {
            string testCaseId = "TC3";

            try
            {
                var account = _jsonProvider.GetAccountById(3);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 3");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(2000);

                string currentUrl = Driver.Url;
                Assert.IsTrue(
                    currentUrl.Contains("/Home/Privacy") || currentUrl.Contains("Login"),
                    $"[{testCaseId}] Đã chuyển hướng khỏi trang login"
                );

                var errorElement = Driver.FindElement(By.CssSelector("p.text-danger"));
                Assert.That(errorElement.Displayed,
                    $"[{testCaseId}] Thông báo lỗi không hiển thị");

                string errorMessage = errorElement.Text;
                Assert.That(errorMessage, Does.Contain("Tài khoản hoặc mật khẩu không chính xác"),
                    $"[{testCaseId}] Thông báo lỗi không đúng. Nhận được: {errorMessage}");

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Hiển thị error message khi nhập sai mật khẩu",
                    "PASS", "");

                Console.WriteLine($"TC3 Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh + ghi vào CurrentTestScreenshot
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }

        [Test]
        public void TC4_Login()
        {
            string testCaseId = "TC4";

            try
            {
                var account = _jsonProvider.GetAccountById(4);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 4");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(2000);

                string currentUrl = Driver.Url;
                Assert.IsTrue(
                    currentUrl.Contains("/Home/Privacy") || currentUrl.Contains("Login"),
                    $"[{testCaseId}] Đã chuyển hướng khỏi trang login"
                );

                var errorElement = Driver.FindElement(By.CssSelector("p.text-danger"));
                Assert.That(errorElement.Displayed,
                    $"[{testCaseId}] Thông báo lỗi không hiển thị");

                string errorMessage = errorElement.Text;
                Assert.That(errorMessage, Does.Contain("Tài khoản hoặc mật khẩu không chính xác"),
                    $"[{testCaseId}] Thông báo lỗi không đúng");

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Hiển thị error message khi email không tồn tại",
                    "PASS", "");

                Console.WriteLine($"TC4 Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh + ghi vào CurrentTestScreenshot
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }

        [Test]
        public void TC5_Login()
        {
            string testCaseId = "TC5";

            try
            {
                        var account = _jsonProvider.GetAccountById(5);
                Assert.IsNotNull(account, "Không tìm thấy account ID 5");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                string currentUrl = Driver.Url;
                Assert.That(currentUrl, Does.Contain("/Home/Privacy") | Does.Contain("Login"),
                    $"[{testCaseId}] Đã chuyển hướng khỏi trang login");

                try
                {
                    var validationElement = Driver.FindElement(By.XPath("//*[contains(text(),'Vui lòng nhập email')]"));
                    Assert.That(validationElement.Displayed, 
                        $"[{testCaseId}] Thông báo validation không hiển thị");

                    string validationMessage = validationElement.Text;
                    Assert.That(validationMessage, Does.Contain("Vui lòng nhập email"),
                        $"[{testCaseId}] Thông báo validation không chính xác");
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail($"[{testCaseId}] Không tìm thấy validation message");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Hiển thị validation message khi email trống",
                    "PASS", "");

                Console.WriteLine($"TC5 Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh + ghi vào CurrentTestScreenshot
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }

        [Test]
        public void TC6_Login()
        {
            string testCaseId = "TC6";

            try
            {
                // ✅ Lấy account có ID = 6 (password trống)
                var account = _jsonProvider.GetAccountById(6);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 6");

                // ✅ Mở trang
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                // ✅ ACTION: Đăng nhập với password trống
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ✅ ASSERTIONS: Xác minh validation message hiển thị
                // 1. Kiểm tra vẫn ở trang login
                string currentUrl = Driver.Url;
                Assert.That(currentUrl, Does.Contain("/Home/Privacy") | Does.Contain("Login"),
                    $"[{testCaseId}] Đã chuyển hướng khỏi trang login");

                // 2. Kiểm tra validation message "Vui lòng nhập Mật khẩu" hiển thị
                try
                {
                    var validationElement = Driver.FindElement(By.XPath("//*[contains(text(),'Vui lòng nhập Mật khẩu')]"));
                    Assert.That(validationElement.Displayed,
                        $"[{testCaseId}] Thông báo validation không hiển thị");

                    string validationMessage = validationElement.Text;
                    Assert.That(validationMessage, Does.Contain("Vui lòng nhập Mật khẩu"),
                        $"[{testCaseId}] Thông báo validation không chính xác. Nhận được: {validationMessage}");
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail($"[{testCaseId}] Không tìm thấy validation message 'Vui lòng nhập Mật khẩu'");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Hiển thị validation message khi password trống",
                    "PASS", "");

                Console.WriteLine($"TC6 Passed");
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }

        [Test]
        public void TC7_Login()
        {
            string testCaseId = "TC7";
            try
            {
                // Lấy account ID 7 (email gốc là chữ thường)
                var account = _jsonProvider.GetAccountById(7);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 7");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                string currentUrl = Driver.Url;

                // Kiểm tra đăng nhập PHẢI THẤT BẠI → Vẫn phải đang ở trang Login
                bool isStillOnLoginPage = currentUrl.Contains("Login") ||
                                          currentUrl.Contains("/Home/Privacy");

                // Assertion với message sạch
                if (!isStillOnLoginPage)
                {
                    Assert.Fail($"[{testCaseId}] Đăng nhập thành công - Email với chữ in hoa được chấp nhận");
                }

                // Kiểm tra thông báo lỗi phải hiển thị
                try
                {
                    var errorElement = Driver.FindElement(By.CssSelector("p.text-danger"));

                    Assert.That(errorElement.Displayed, Is.True,
                        $"[{testCaseId}] Không hiển thị thông báo lỗi khi dùng email chữ in hoa");

                    string errorMessage = errorElement.Text.Trim();
                    Assert.That(errorMessage, Does.Contain("Tài khoản hoặc mật khẩu không chính xác"),
                        $"[{testCaseId}] Thông báo lỗi không đúng");
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail($"[{testCaseId}] Không tìm thấy thông báo lỗi khi đăng nhập thất bại");
                }

                // PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Email có chữ in hoa ({account.Email}) bị reject - Hệ thống phân biệt case sensitive",
                    "PASS", "");

                Console.WriteLine($"TC7 Passed");
            }
            catch (AssertionException ex)
            {
                // Chụp ảnh và ghi kết quả FAIL với message sạch
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                // Lấy message sạch (bỏ phần Expected/But was nếu có)
                string failureMessage = ex.Message.Contains("Expected:")
                    ? ex.Message.Split(new[] { "Expected:" }, StringSplitOptions.None)[0].Trim()
                    : ex.Message;

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", failureMessage, "FAIL", CurrentTestScreenshot);
                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }

        [Test]
        public void TC9_Login()
        {
            string testCaseId = "TC9";
            try
            {
                var account = _jsonProvider.GetAccountById(8);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 8");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ✅ Không được đăng nhập thành công
                Assert.IsTrue(
                    currentUrl.Contains("/Home/Privacy") || currentUrl.Contains("Login"),
                    $"[{testCaseId}] Đăng nhập thành công"
                );

                // ✅ Kiểm tra có hiển thị thông báo lỗi
                var errorElement = Driver.FindElement(By.CssSelector("p.text-danger"));
                Assert.That(errorElement.Displayed,
                    $"[{testCaseId}] Không hiển thị thông báo lỗi");

                string errorMessage = errorElement.Text.Trim();

                // ✅ Kiểm tra nội dung thông báo lỗi - Viết lại Assert để thông báo fail ngắn gọn
                if (!errorMessage.Contains("Email không đúng định dạng"))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo không đúng. Nhận được: {errorMessage}");
                }

                // Nếu pass thì cập nhật Excel
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Hiển thị đúng thông báo khi email sai định dạng",
                    "PASS", "");

                Console.WriteLine($"{testCaseId} Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh khi fail
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                // Ghi kết quả FAIL vào Excel
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
        }

        [Test]
        public void TC10_Login()
        {
            string testCaseId = "TC10";
            try
            {
                var account = _jsonProvider.GetAccountById(9);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 9");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ✅ Không được đăng nhập thành công
                Assert.IsTrue(
                    currentUrl.Contains("/Home/Privacy") || currentUrl.Contains("Login"),
                    $"[{testCaseId}] Đã chuyển hướng khỏi trang login → Không đúng với expected"
                );

                // ✅ Kiểm tra có hiển thị thông báo lỗi
                var errorElement = Driver.FindElement(By.CssSelector("p.text-danger"));
                Assert.That(errorElement.Displayed,
                    $"[{testCaseId}] Không hiển thị thông báo lỗi");

                string errorMessage = errorElement.Text.Trim();

                // ✅ Kiểm tra nội dung thông báo lỗi
                if (!errorMessage.Contains("Tài khoản hoặc mật khẩu không chính xác"))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo không đúng. Nhận được: {errorMessage}");
                }

                // ✅ PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Đăng nhập không thành công. Hiển thị đúng thông báo lỗi.",
                    "PASS", "");

                Console.WriteLine($"{testCaseId} Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh khi fail
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                // ✅ Ghi FAIL (message đã ngắn gọn từ Assert.Fail)
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
        }

        [Test]
        public void TC11_Login()
        {
            string testCaseId = "TC11";
            try
            {
                var account = _jsonProvider.GetAccountById(10);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 10");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ✅ Không được đăng nhập thành công
                Assert.IsTrue(
                    currentUrl.Contains("/Home/Privacy") || currentUrl.Contains("Login"),
                    $"[{testCaseId}] Đã chuyển hướng khỏi trang login → Không đúng với expected"
                );

                // ✅ Kiểm tra có hiển thị thông báo lỗi
                var errorElement = Driver.FindElement(By.CssSelector("p.text-danger"));
                Assert.That(errorElement.Displayed,
                    $"[{testCaseId}] Không hiển thị thông báo lỗi");

                string errorMessage = errorElement.Text.Trim();

                // ✅ Kiểm tra nội dung thông báo lỗi
                if (!errorMessage.Contains("Tài khoản hoặc mật khẩu không chính xác"))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo không đúng. Nhận được: {errorMessage}");
                }

                // ✅ PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Đăng nhập không thành công. HIển thị đúng thông báo lỗi.",
                    "PASS", "");

                Console.WriteLine($"{testCaseId} Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh khi fail
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                // ✅ Ghi FAIL (message đã ngắn gọn)
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
        }

        [Test]
        public void TC12_Login()
        {
            string testCaseId = "TC12";
            try
            {
                var account = _jsonProvider.GetAccountById(2);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 2");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.NavigateToLogin();
                Thread.Sleep(1000);

                // ✅ Nhập Email
                var emailElement = Driver.FindElement(By.Id("EmailForm"));
                emailElement.Clear();
                emailElement.SendKeys(account.Email);

                // ✅ Nhập Password
                var passwordElement = Driver.FindElement(By.Id("PassWordForm"));
                passwordElement.Clear();
                passwordElement.SendKeys(account.Password);

                Thread.Sleep(1000);

                // ✅ Lấy attribute type để kiểm tra che mật khẩu
                string typeAttribute = passwordElement.GetAttribute("type");

                // ❌ Nếu không phải password → fail
                if (typeAttribute != "password")
                {
                    Assert.Fail($"[{testCaseId}] Mật khẩu không được che. Type hiện tại: {typeAttribute}");
                }

                // ✅ PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Ô mật khẩu được che đúng định dạng",
                    "PASS", "");

                Console.WriteLine($"{testCaseId} Passed");
            }
            catch (Exception ex)
            {
                // ✅ Chụp ảnh khi fail
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                // ✅ Ghi FAIL
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);

                throw;
            }
        }

        [Test]
        public void TC14_Login()
        {
            string testCaseId = "TC14";

            try
            {
                var account = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 0");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ❌ Nếu vẫn ở login → fail
                if (currentUrl.Contains("Login"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng nhập thất bại với tài khoản Admin");
                }

                // ❌ Nếu vào giao diện khách hàng → fail
                if (currentUrl.Contains("/Home/Privacy"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng nhập Admin nhưng vào giao diện khách hàng");
                }

                // ✅ PASS (ngầm hiểu là vào Admin)
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Đăng nhập Admin thành công và vào đúng giao diện",
                    "PASS", "");

                Console.WriteLine($"{testCaseId} Passed");
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }

        [Test]
        public void TC15_Login()
        {
            string testCaseId = "TC15";

            try
            {
                var account = _jsonProvider.GetAccountById(11);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 1");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ❌ Nếu vẫn ở login → fail
                if (currentUrl.Contains("Login"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng nhập thất bại với tài khoản Admin");
                }

                // ❌ Nếu vào giao diện khách hàng → fail
                if (currentUrl.Contains("/Home/Privacy"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng nhập Nhân viên nhưng vào giao diện khách hàng");
                }

                // ✅ PASS (ngầm hiểu là vào Admin)
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Đăng nhập Nhân viên thành công và vào đúng giao diện",
                    "PASS", "");

                Console.WriteLine($"{testCaseId} Passed");
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
                throw;
            }
        }
    }
}