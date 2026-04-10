using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;

        // ===== Locators =====
        private readonly By banner = By.CssSelector("div.row.p-5 img");
        private readonly By homeLink = By.CssSelector("li.nav-item a.nav-link[href='/']");
        private readonly By shopLink = By.CssSelector("li.nav-item a.nav-link[href='/Shop']");
        private readonly By cartIcon = By.CssSelector("a.nav-icon .bi-cart-check-fill");
        private readonly By profileIcon = By.CssSelector("i.bi-person-fill.dropdown-toggle");
        private readonly By firstSlideIndicator = By.CssSelector("ol.carousel-indicators li.active");
        private readonly By firstProductBanner = By.CssSelector("a[href^='/Shop/IndexDetails'] img");
        private readonly By footerTraLink = By.XPath("//a[text()='Trà']");
        private readonly By coffeeBtn = By.CssSelector("i.bi-caret-down-fill.mt-1");
        private readonly By vietNamCoffeeLink = By.CssSelector("a[onclick='changeType(4)']");
        private readonly By AddToCartBtn = By.CssSelector("button.btn-success.btn-lg[value='addtocard']");

        public HomePage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        // ===== TC1: Kiểm tra hiển thị trang Home =====
        public void ExecuteTC1_HomePageDisplay(string testCaseId)
        {
            string stepNumber = "1";
            try
            {
                string testData = "Kiểm tra các thành phần: Banner, Trang chủ, Cửa hàng, Icon giỏ hàng, Icon hồ sơ, Slide, Banner sản phẩm, Footer Trà";

                Assert.IsTrue(_driver.FindElement(banner).Displayed, "Banner không hiển thị");
                Assert.IsTrue(_driver.FindElement(homeLink).Displayed, "Link Trang chủ không hiển thị");
                Assert.IsTrue(_driver.FindElement(shopLink).Displayed, "Link Cửa hàng không hiển thị");
                Assert.IsTrue(_driver.FindElement(cartIcon).Displayed, "Icon giỏ hàng không hiển thị");
                Assert.IsTrue(_driver.FindElement(profileIcon).Displayed, "Icon hồ sơ không hiển thị");
                Assert.IsTrue(_driver.FindElement(firstSlideIndicator).Displayed, "Slide trang quảng cáo không hiển thị");
                Assert.IsTrue(_driver.FindElement(firstProductBanner).Displayed, "Banner sản phẩm đầu tiên không hiển thị");
                Assert.IsTrue(_driver.FindElement(footerTraLink).Displayed, "Footer Trà không hiển thị");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Tất cả thành phần hiển thị đúng", "PASS", "", testData);

                Console.WriteLine($"[{testCaseId}] PASS: HomePage hiển thị đầy đủ");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail(ex.Message);
            }
        }

        // ===== TC2: Chuyển Home -> Shop -> Home =====
        public void ExecuteTC2_NavigateShopAndBack(string testCaseId)
        {
            string stepNumber = "1";
            try
            {
                string testData = "Chuyển Home -> Shop -> Home";

                // Click Shop link
                _driver.FindElement(shopLink).Click();
                Thread.Sleep(2000);

                // Kiểm tra URL Shop
                Assert.IsTrue(_driver.Url.Contains("/Shop"), "Không chuyển sang Shop page");

                // Quay lại Home
                _driver.FindElement(homeLink).Click();
                Thread.Sleep(2000);

                // Kiểm tra URL Home
                Assert.IsTrue(_driver.Url.EndsWith("/"), "Không quay lại Home page");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Chuyển Home -> Shop -> Home thành công", "PASS", "", testData);

                Console.WriteLine($"[{testCaseId}] PASS: TC2 chuyển Home -> Shop -> Home thành công");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData: "");
                Assert.Fail(ex.Message);
            }
        }

        public void ExecuteTC4_NavigateToShop(string testCaseId)
        {
            string stepNumber = "2";
            try
            {
                string testData = "Từ Home bấm link Shop và kiểm tra chuyển trang";

                // Click Shop link
                _driver.FindElement(shopLink).Click();
                Thread.Sleep(2000);

                // Kiểm tra URL Shop
                Assert.IsTrue(_driver.Url.Contains("/Shop"), "Không chuyển sang Shop page");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Chuyển sang Shop page thành công", "PASS", "", testData);

                Console.WriteLine($"[{testCaseId}] PASS: TC4 chuyển Home -> Shop thành công");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData: "");
                Assert.Fail(ex.Message);
            }
        }



        // ===== TC5: Chọn Cà phê Việt Nam trên Shop =====
        public void ExecuteTC5_SelectVietnamCoffee(string testCaseId)
        {
            string stepNumber = "1";
            try
            {
                // 1. Click link Shop
                _driver.FindElement(shopLink).Click();
                Thread.Sleep(2000);

                // 2. Click button Coffee
                _driver.FindElement(coffeeBtn).Click();
                Thread.Sleep(1000);

                // 3. Click link Cà phê Việt Nam
                _driver.FindElement(vietNamCoffeeLink).Click();
                Thread.Sleep(2000);

                // 4. Kiểm tra URL khác để xác nhận chuyển trang thành công
                if (!_driver.Url.Contains("/Shop"))
                {
                    throw new Exception("Chưa chuyển sang trang Shop hoặc loại Coffee không đúng!");
                }

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Chọn Cà phê Việt Nam thành công, chuyển trang đúng", "PASS", "", "");
                Console.WriteLine($"[{testCaseId}] PASS: Chọn Cà phê Việt Nam thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        // ===== TC6: Click slide quảng cáo chuyển slide khác =====
        public void ExecuteTC6_ClickCarouselSlide(string testCaseId)
        {
            string stepNumber = "1";
            try
            {
                // 1. Lấy slide đầu tiên
                var firstSlideIndicator = _driver.FindElement(By.CssSelector("ol.carousel-indicators li.active"));

                // 2. Click slide tiếp theo (nếu có)
                var nextSlide = _driver.FindElement(By.CssSelector("ol.carousel-indicators li:not(.active)"));
                nextSlide.Click();
                Thread.Sleep(2000);

                // 3. Kiểm tra active slide đã đổi
                var currentActive = _driver.FindElement(By.CssSelector("ol.carousel-indicators li.active"));
                if (currentActive.GetAttribute("data-bs-slide-to") == firstSlideIndicator.GetAttribute("data-bs-slide-to"))
                {
                    throw new Exception("Slide không chuyển đổi sau khi click!");
                }

                // 4. Ghi kết quả vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Slide quảng cáo chuyển đổi thành công", "PASS", "", "");
                Console.WriteLine($"[{testCaseId}] PASS: Slide quảng cáo chuyển đổi thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        // ===== TC8: Click hình đầu tiên chuyển trang =====
        public void ExecuteTC14_ClickFirstImage(string testCaseId)
        {
            string stepNumber = "4";
            try
            {
                // 1. Scroll xuống để thấy hình đầu tiên
                var firstImage = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstImage);
                Thread.Sleep(1000);

                // 2. Click vào hình
                firstImage.Click();
                Thread.Sleep(3000);

                // 3. Kiểm tra URL có thay đổi (chuyển trang)
                if (!_driver.Url.Contains("/Shop/IndexDetails"))
                {
                    throw new Exception("Không chuyển qua trang chi tiết sản phẩm sau khi click hình đầu tiên!");
                }

                // 4. Ghi kết quả vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Click hình đầu tiên chuyển trang thành công", "PASS", "", "");
                Console.WriteLine($"[{testCaseId}] PASS: Click hình đầu tiên chuyển trang thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC15_ClickNextPage(string testCaseId)
        {
            string stepNumber = "10";
            try
            {
                // 1. Click vào trang Cửa hàng
                var shopLink = _driver.FindElement(By.CssSelector("a.nav-link[href='/Shop']"));
                shopLink.Click();
                Thread.Sleep(3000);

                // 2. Scroll xuống phần pagination
                var page2Link = _driver.FindElement(By.CssSelector("li.page-item a[onclick*='nextPage()']"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", page2Link);
                Thread.Sleep(1000);

                // 3. Click page 2
                page2Link.Click();
                Thread.Sleep(3000);

                // 4. Kiểm tra page 3 đã hiển thị (trang 3 sẽ là disabled li)
                var page3Li = _driver.FindElement(By.CssSelector("li.page-item.disabled"));
                if (!page3Li.Displayed)
                {
                    throw new Exception("Trang 3 không hiển thị sau khi click page 2!");
                }

                // 5. Ghi kết quả vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Click pagination chuyển sang trang 3 thành công", "PASS", "", "");
                Console.WriteLine($"[{testCaseId}] PASS: Click pagination chuyển trang thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }

        public void ExecuteTC17_ClickFirstImage(string testCaseId)
        {
            string stepNumber = "9";
            try
            {
                // ===== 1. Scroll xuống phần sản phẩm/banner =====
                var firstImage = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstImage);
                Thread.Sleep(1000);

                // ===== 2. Lấy src ảnh trên trang Home =====
                string homeImgSrc = firstImage.GetAttribute("src");

                // ===== 3. Click vào hình =====
                firstImage.Click();
                Thread.Sleep(3000);

                // ===== 4. Kiểm tra ảnh chi tiết hiển thị đúng =====
                var detailImage = _driver.FindElement(By.Id("product-image")); // dùng ID trực tiếp
                string detailImgSrc = detailImage.GetAttribute("src");

                if (!detailImgSrc.Equals(homeImgSrc))
                    throw new Exception("Ảnh chi tiết không đúng");

                // ===== 5. Ghi kết quả Excel =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Click vào hình đầu tiên hiển thị chi tiết thành công", "PASS", "", $"Ảnh: {homeImgSrc}");

                Console.WriteLine($"[{testCaseId}] PASS: Click hình đầu tiên chuyển trang chi tiết thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }

        public void ExecuteTC23_CheckProductPrice(string testCaseId)
        {
            string stepNumber = "9"; // tương tự TC17
            try
            {
                // ===== 1. Scroll xuống phần sản phẩm/banner =====
                var firstImage = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstImage);
                Thread.Sleep(1000);

                // ===== 2. Lấy giá trên Home =====
                // ===== Lấy giá trên trang Home =====
                var homePriceElement = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[2]/div/div[2]/div[1]/div[2]/div/ul/li[2]"));
                string homePrice = homePriceElement.Text.Trim();

                // ===== 3. Click vào hình để sang trang chi tiết =====
                firstImage.Click();
                Thread.Sleep(3000);

                // ===== 4. Lấy giá trên trang chi tiết =====
                string detailPrice = _driver.FindElement(By.CssSelector("p.h3.py-2#Price")).Text.Trim();

                // ===== 5. So sánh giá =====
                string testResult = homePrice == detailPrice ? "PASS" : "FAIL";
                string message = homePrice == detailPrice
                    ? $"Giá sản phẩm đúng: {detailPrice}"
                    : $"Giá sản phẩm sai. Home: {homePrice}, Detail: {detailPrice}";

                // ===== 6. Ghi kết quả vào Excel =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    message, testResult, "", $"Home: {homePrice}, Detail: {detailPrice}");

                Console.WriteLine($"[{testCaseId}] {testResult}: {message}");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC8_AddToCartAlert(string testCaseId)
        {
            string stepNumber = "4"; // bạn có thể tùy chỉnh step
            try
            {
                // ===== 1. Scroll xuống phần sản phẩm/banner =====
                var firstProduct = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstProduct);
                Thread.Sleep(1000);

                // ===== 2. Click vào hình sản phẩm =====
                firstProduct.Click();
                Thread.Sleep(2000);

                // ===== 3. Click thêm vào giỏ hàng =====
                var addToCartBtn = _driver.FindElement(By.CssSelector("button[value='addtocard']"));
                addToCartBtn.Click();
                Thread.Sleep(1000);

                // ===== 4. Kiểm tra alert =====
                IAlert alert = _driver.SwitchTo().Alert();
                string alertText = alert.Text;

                if (alertText.Contains("Thêm vào giỏ hàng thành công"))
                {
                    alert.Accept(); // đóng alert

                    // ===== 5. Ghi kết quả vào Excel =====
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "Thêm sản phẩm vào giỏ hàng thành công, alert hiển thị đúng",
                        "PASS", "", "Alert: " + alertText);

                    Console.WriteLine($"[{testCaseId}] PASS: Alert hiển thị đúng khi thêm sản phẩm vào giỏ hàng.");
                }
                else
                {
                    throw new Exception($"Alert không đúng: {alertText}");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
    }
}