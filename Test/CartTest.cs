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
    public class CartTest : BaseTest
    {
        private CartPage _cartPage;
        private LoginPage _loginPage;
        private JsonDataProvider _jsonDataProvider;
        private ExcelConfig _cartConfig; // 🔥 Config riêng cho CartTest

        [SetUp]
        public void Setup()
        {
            _cartPage = new CartPage(Driver, Config, ExcelProvider);
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _jsonDataProvider = new JsonDataProvider(Config);

            
        }

        // --- TEST CASE 2: TĂNG SỐ LƯỢNG ---
        [Test]
        public void TC2_Cart_IncreaseQuantity_Workflow()
        {
            string testCaseId = "TC2";
            string testData = "Bấm nút tăng số lượng (+)";

            try
            {
                // Bước 0: Điều hướng và Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // Bước chính: Thực hiện luồng Tăng
                _cartPage.ExecuteFullCartFlow(testCaseId);
            }
            catch (System.Exception ex)
            {
                // 🔥 Dùng _cartConfig thay vì Config
                ExcelProvider.UpdateTestResult(_cartConfig, testCaseId, "1",
                    "Lỗi hệ thống: " + ex.Message, "FAIL", "", testData);

                Assert.Fail($"[HỆ THỐNG] {testCaseId} lỗi: {ex.Message}");
            }
        }

        // --- TEST CASE 3: GIẢM SỐ LƯỢNG ---
        [Test]
        public void TC3_Cart_DecreaseQuantity_Workflow()
        {
            string testCaseId = "TC3";
            string testData = "Bấm nút giảm số lượng (-)";

            try
            {
                // Bước 0: Điều hướng và Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // Bước chính: Thực hiện luồng Giảm
                _cartPage.ExecuteDecreaseFlow(testCaseId);
            }
            catch (System.Exception ex)
            {
                // 🔥 Dùng _cartConfig thay vì Config
                ExcelProvider.UpdateTestResult(_cartConfig, testCaseId, "1",
                    "Lỗi hệ thống: " + ex.Message, "FAIL", "", testData);

                Assert.Fail($"[HỆ THỐNG] {testCaseId} lỗi: {ex.Message}");
            }
        }

        [Test]
        public void TC5_Cart_ContinueShopping_Workflow()
        {
            string testCaseId = "TC5";

            try
            {
                // Bước 0: Điều hướng và Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // Bước chính: Thực hiện luồng TC5 
                // (Bên trong hàm này đã tự ghi kết quả PASS/FAIL và Test Data vào Excel rồi)
                _cartPage.ExecuteTC5_ContinueShopping(testCaseId);
            }
            catch (System.Exception ex)
            {
                // 🔥 Dùng _cartConfig thay vì Config
                ExcelProvider.UpdateTestResult(_cartConfig, testCaseId, "4",
                    "Lỗi hệ thống: " + ex.Message, "FAIL", "", "Click vào tiếp tục xem sản phẩm");

                Assert.Fail($"[HỆ THỐNG] {testCaseId} tạch sớm: {ex.Message}");
            }
        }

        [Test]
        public void TC8_DeleteProduct_Test()
        {
            string testCaseId = "TC8";

            try
            {
                // Bước 0: Điều hướng và Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // Bước chính: Thực hiện luồng TC8
                // (Hàm này tự ghi PASS/FAIL vào Excel rồi)
                _cartPage.ExecuteTC8_DeleteFlow(testCaseId);
            }
            catch (System.Exception ex)
            {
                // 🔥 Dùng _cartConfig thay vì Config
                ExcelProvider.UpdateTestResult(_cartConfig, testCaseId, "7",
                    "Lỗi hệ thống: " + ex.Message, "FAIL", "", "Click vào icon xóa");

                Assert.Fail($"[HỆ THỐNG] {testCaseId} tạch sớm: {ex.Message}");
            }
        }

        [Test]
        public void TC1_ProductImage_Consistency_Test()
        {
            string testCaseId = "TC1";

            try
            {
                // 1. Đăng nhập (Y hệt mẫu TC5)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Kéo xuống trang chủ để chọn sản phẩm
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                // Tìm link sản phẩm hoặc ảnh sản phẩm ở trang chủ
                var productImg = Driver.FindElement(By.CssSelector(".product-item img, .card-img-top"));
                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", productImg);
                Thread.Sleep(1000);
                productImg.Click(); // Click để vào trang chi tiết
                Thread.Sleep(2000);

                // 3. Gọi hàm thực hiện so sánh ảnh ở trang Page
                _cartPage.ExecuteTC1_VerifyImageFlow(testCaseId);
            }
            catch (System.Exception ex)
            {
                // 🔥 Dùng _cartConfig thay vì Config
                ExcelProvider.UpdateTestResult(_cartConfig, testCaseId, "1",
                    "Lỗi hệ thống: " + ex.Message, "FAIL", "", "Kiểm tra hình ảnh");

                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC10_ProductPrice_Consistency_Test()
        {
            string testCaseId = "TC10"; // Theo hình Excel bạn gửi, các step này nằm trong khối TC8

            try
            {
                // 1. Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chọn sản phẩm từ trang chủ để vào trang chi tiết
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                var productImg = Driver.FindElement(By.CssSelector(".product-item img, .card-img-top"));
                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", productImg);
                Thread.Sleep(1000);
                productImg.Click();
                Thread.Sleep(2000);

                // 3. Thực hiện kiểm tra giá
                _cartPage.ExecuteTC10_VerifyPriceFlow(testCaseId);
            }
            catch (System.Exception ex)
            {
                // 🔥 Dùng _cartConfig thay vì Config
                ExcelProvider.UpdateTestResult(_cartConfig, testCaseId, "9",
                    "Lỗi hệ thống: " + ex.Message, "FAIL", "", "Kiểm tra giá sản phẩm");

                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC13_Quantity_Limit_Test()
        {
            string testCaseId = "TC13";

            try
            {
                // 1. Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                _loginPage.LoginWithAccount(_jsonDataProvider.GetAccountById(2));
                Thread.Sleep(2000);

                // 2. Đi thẳng vào trang Giỏ hàng để tránh lỗi locator icon
                Driver.Navigate().GoToUrl("https://localhost:7116/Cart");
                Thread.Sleep(3000);

                // 3. Thực hiện logic giảm và ghi kết quả
                _cartPage.ExecuteTC13_FailAtOneFlow(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail("Lỗi hệ thống: " + ex.Message);
            }
        }

        [Test]
        public void TC15_Increase_Quantity_Limit_Test()
        {
            string testCaseId = "TC15";

            try
            {
                // 1. Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                _loginPage.LoginWithAccount(_jsonDataProvider.GetAccountById(2));
                Thread.Sleep(2000);

                // 2. Đi thẳng vào trang Giỏ hàng
                Driver.Navigate().GoToUrl("https://localhost:7116/Cart");
                Thread.Sleep(3000);

                // 3. Thực hiện logic tăng lên 50 và ghi FAIL + chụp ảnh
                _cartPage.ExecuteTC15_IncreaseLimitFlow(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail("Lỗi hệ thống: " + ex.Message);
            }
        }

        [Test]
        public void TC16_Add_Multiple_Products_Test()
        {
            string testCaseId = "TC16";
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            var actions = new OpenQA.Selenium.Interactions.Actions(Driver);

            try
            {
                // B1: Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                _loginPage.LoginWithAccount(_jsonDataProvider.GetAccountById(2));
                Thread.Sleep(3000);

                // --- BẮT ĐẦU CHU TRÌNH 3 MÓN ---
                for (int i = 0; i < 3; i++)
                {
                    // Nếu từ món thứ 2, bấm 'Tiếp tục' để quay lại Shop
                    if (i > 0)
                    {
                        var continueBtn = Driver.FindElement(By.CssSelector("a[href='/Shop'].btn-link"));
                        js.ExecuteScript("arguments[0].click();", continueBtn);
                        Thread.Sleep(3000);
                    }

                    // BƯỚC QUAN TRỌNG: Kéo xuống để thấy sản phẩm (đặc biệt là lần đầu ở Trang Chủ)
                    js.ExecuteScript("window.scrollBy(0, 800);");
                    Thread.Sleep(2000);

                    // Tìm danh sách card sản phẩm
                    var productCards = Driver.FindElements(By.CssSelector(".product-wap, .card.h-100"));

                    // Cuộn chính xác đến món thứ i
                    js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", productCards[i]);
                    Thread.Sleep(1500);

                    // Rê chuột hiện icon xanh
                    actions.MoveToElement(productCards[i]).Perform();
                    Thread.Sleep(1000);

                    // Click vào icon xanh (hoặc ảnh nếu chưa có icon)
                    try
                    {
                        var blueIcon = productCards[i].FindElement(By.CssSelector("a.btn-success.text-white"));
                        js.ExecuteScript("arguments[0].click();", blueIcon);
                    }
                    catch
                    {
                        var imgLink = productCards[i].FindElement(By.CssSelector("img"));
                        js.ExecuteScript("arguments[0].click();", imgLink);
                    }

                    // Bấm Thêm vào giỏ
                    Thread.Sleep(2000);
                    Driver.FindElement(By.CssSelector("button.btn-success")).Click();
                    Thread.Sleep(1500);
                    try { Driver.SwitchTo().Alert().Accept(); } catch { }

                    // Click icon giỏ hàng header để vào Cart
                    var cartIcon = Driver.FindElement(By.CssSelector("a.nav-icon .bi-cart-check-fill"));
                    js.ExecuteScript("arguments[0].click();", cartIcon);
                    Thread.Sleep(3000);
                }

                // BƯỚC CHỐT: Kiểm tra đủ 3 món
                _cartPage.ExecuteTC16_CheckFinalCart(testCaseId, 3);
            }
            catch (Exception ex) { Assert.Fail("Lỗi kịch bản: " + ex.Message); }
        }

        [Test]
        public void TC27_Clear_Cart_Test()
        {
            string testCaseId = "TC27";
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;

            try
            {
                // BƯỚC 1: Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                _loginPage.LoginWithAccount(_jsonDataProvider.GetAccountById(2));
                Thread.Sleep(2000);

                // BƯỚC 2: Click vào icon giỏ hàng trên Header
                var cartIcon = Driver.FindElement(By.CssSelector("a.nav-icon .bi-cart-check-fill"));
                js.ExecuteScript("arguments[0].click();", cartIcon);
                Thread.Sleep(2000);

                // BƯỚC 3: Xóa sản phẩm (nếu có) và xác nhận thông báo trống
                _cartPage.ExecuteTC27_ClearCartAndVerify(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail("Lỗi TC27: " + ex.Message);
            }
        }
    }
}

