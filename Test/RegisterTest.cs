using Microsoft.Win32;
using NPOI.SS.Formula.Functions;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Models;
using SeleniumNUnitExcelAutomation.Pages;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class RegisterTest : BaseTest
    {
        private RegisterPage _registerPage;
        private JsonDataProvider _jsonProvider;

        [SetUp]
        public void TestSetup()
        {
            _registerPage = new RegisterPage(Driver, Config, ExcelProvider);
            _jsonProvider = new JsonDataProvider(Config);
        }

        [Test]
        public void TC16_Register()
        {
            string testCaseId = "TC16";

            try
            {
                // Mở trang chủ
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                // Chuyển đến trang đăng ký
                _registerPage.NavigateToRegister();
                Thread.Sleep(2000);

                // ==================== KIỂM TRA CÁC PHẦN TỬ QUAN TRỌNG ====================

                _registerPage.VerifyElementExists(By.Id("formFullName"), testCaseId, "1", "Có ô Họ và Tên");
                _registerPage.VerifyElementExists(By.Id("formEmail"), testCaseId, "2", "Có ô Email");
                _registerPage.VerifyElementExists(By.Id("formPassword"), testCaseId, "3", "Có ô Mật khẩu");
                _registerPage.VerifyElementExists(By.Id("formPasswordConfirm"), testCaseId, "4", "Có ô Nhập lại Mật khẩu");
                _registerPage.VerifyElementExists(By.Id("formCheck"), testCaseId, "5", "Có checkbox điều khoản");

                _registerPage.VerifyElementExists(
                    By.XPath("//button[contains(text(),'Đăng ký')]"),
                    testCaseId, "6", "Có button Đăng ký");

                // SỬA Ở ĐÂY - Locator đúng cho "Đăng nhập"
                _registerPage.VerifyElementExists(
                    By.LinkText("Đăng nhập"),
                    testCaseId, "7", "Có link Đăng nhập");

                // Ghi kết quả PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    "Tất cả các phần tử quan trọng trên form đăng ký đều hiển thị đúng",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1", ex.Message, "FAIL", CurrentTestScreenshot);
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
        public void TC17_Register()
        {
            string testCaseId = "TC17";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(1);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 1");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ✅ THAY Assert.That → if + Assert.Fail
                if (currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Vẫn ở trang đăng ký - Đăng ký thất bại");
                }

                // ✅ Check error message (nếu có là fail)
                try
                {
                    var errorElement = Driver.FindElement(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                    if (errorElement.Displayed)
                    {
                        string errorMessage = errorElement.Text;
                        Assert.Fail($"[{testCaseId}] Có error message: {errorMessage}");
                    }
                }
                catch (NoSuchElementException)
                {
                    // Không có error → OK
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"Đăng ký thành công với Email: {registerData.Email}",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                // ✅ CLEAN MESSAGE (giống LoginTest)
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
        public void TC18_Register()
        {
            string testCaseId = "TC18";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(1);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 1");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ❌ Nếu đăng ký thành công → fail
                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công với email đã tồn tại");
                }

                // ✅ Tìm error an toàn
                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                if (!errorMessage.Contains("Email đã tồn tại"))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo không đúng. Nhận được: {errorMessage}");
                }

                // ✅ PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị đúng thông báo email đã tồn tại",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
        public void TC19_Register()
        {
            string testCaseId = "TC19";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(4);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 4");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ❌ Nếu đăng ký thành công → fail
                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công khi để trống Họ và Tên");
                }

                // ✅ Tìm thông báo lỗi (an toàn)
                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                // ❌ Nếu có error nhưng nội dung rỗng → fail
                if (string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo lỗi không có nội dung");
                }

                // ✅ PASS (chỉ cần có thông báo lỗi là đủ theo yêu cầu)
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị thông báo lỗi khi để trống Họ và Tên",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
        public void TC20_Register()
        {
            string testCaseId = "TC20";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(5);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 5");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công khi để trống Email");
                }

                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo lỗi không có nội dung");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị lỗi khi để trống Email",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
        public void TC21_Register()
        {
            string testCaseId = "TC21";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(6);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 6");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công khi để trống Password");
                }

                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo lỗi không có nội dung");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị lỗi khi để trống Password",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
        public void TC22_Register()
        {
            string testCaseId = "TC22";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(7);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 7");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công khi để trống Nhập lại mật khẩu");
                }

                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo lỗi không có nội dung");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị lỗi khi để trống nhập lại mật khẩu",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
        public void TC23_Register()
        {
            string testCaseId = "TC23";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(8);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 8");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                // ❗ Không tick checkbox (đảm bảo data không tick)
                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công khi chưa đồng ý điều khoản");
                }

                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo lỗi không có nội dung");
                }

                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị lỗi khi chưa đồng ý điều khoản",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
        public void TC25_Register()
        {
            string testCaseId = "TC25";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(9);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 9");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ❌ Nếu đăng ký thành công → fail
                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công với email sai định dạng");
                }

                // ✅ Chỉ check error UI giống các TC trước
                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo lỗi không có nội dung");
                }

                // ✅ PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị lỗi khi email sai định dạng",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
        public void TC27_Register()
        {
            string testCaseId = "TC27";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(6);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 6");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ❌ Nếu đăng ký thành công → fail
                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công với password là khoảng trắng");
                }

                // ✅ Check thông báo lỗi giống các TC trước
                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo lỗi không có nội dung");
                }

                // ✅ PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị lỗi khi password là khoảng trắng",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
        public void TC29_Register()
        {
            string testCaseId = "TC29";

            try
            {
                var registerData = _jsonProvider.GetRegistrationById(10);
                Assert.IsNotNull(registerData, $"[{testCaseId}] Không tìm thấy dữ liệu đăng ký ReID 10");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _registerPage.NavigateToRegister();
                Thread.Sleep(1000);

                _registerPage.FillRegistrationForm(registerData);
                Thread.Sleep(1000);

                _registerPage.ClickRegisterButton();
                Thread.Sleep(1000);

                string currentUrl = Driver.Url;

                // ❌ Nếu đăng ký thành công → fail
                if (!currentUrl.Contains("/Account/Register"))
                {
                    Assert.Fail($"[{testCaseId}] Đăng ký thành công với email có khoảng trắng");
                }

                // ✅ Check thông báo lỗi giống các TC trước
                var errorElements = Driver.FindElements(By.CssSelector("p.text-danger, .alert-danger, .error-message"));

                if (errorElements.Count == 0)
                {
                    Assert.Fail($"[{testCaseId}] Không hiển thị thông báo lỗi");
                }

                string errorMessage = errorElements[0].Text.Trim();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail($"[{testCaseId}] Thông báo lỗi không có nội dung");
                }

                // ✅ PASS
                ExcelProvider.UpdateTestResult(Config, testCaseId, "1",
                    $"[{testCaseId}] Hiển thị lỗi khi email có khoảng trắng",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] Passed");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

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
    }
}