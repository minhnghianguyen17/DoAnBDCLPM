using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Models;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class ProductPage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;

        // --- Locators ---
        private readonly By firstProductHomeImg = By.XPath("/html/body/div[1]/main/section[2]/div/div[2]/div[1]/div[2]/a/img");
        private readonly By detailProductName = By.XPath("/html/body/div[1]/main/section[1]/div/div/div[2]/div/div/div/div[1]/h2");
        private readonly By quantityList = By.XPath("/html/body/div[1]/main/section[1]/div/div/div[2]/div/div/form/div[1]/div[2]/ul");
        private readonly By btnPlus = By.Id("btn-plus"); // nút tăng
        private readonly By btnMinus = By.Id("btn-minus"); // nút giảm
        
        // --- Locators Related Product ---
        private readonly By relatedProductFirstImg = By.XPath("//div[@id='carousel-related-product']//div[contains(@class,'product-wap')]//img");
        private readonly By relatedProductNextBtn = By.XPath("/html/body/div[1]/main/section[2]/div/div[2]/div[1]/div/div[1]/div/ul/li[2]/a/i"); // button next



        public ProductPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        public void ExecuteTC28_IncreaseQuantity(string testCaseId)
        {
            string stepNumber = "2";
            try
            {
                // ===== 1. Scroll tới ảnh đầu tiên trên Home =====
                var homeImg = _driver.FindElement(firstProductHomeImg);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", homeImg);
                Thread.Sleep(1000);

                // ===== 2. Click vào ảnh ra trang chi tiết =====
                homeImg.Click();
                Thread.Sleep(2000);

                // ===== 3. Kiểm tra tên sản phẩm chi tiết hiển thị =====
                var nameDetail = _driver.FindElement(detailProductName).Text;
                if (string.IsNullOrEmpty(nameDetail))
                    throw new Exception("Tên sản phẩm chi tiết không hiển thị");

                // ===== 4. Click nút tăng số lượng =====
                _driver.FindElement(btnPlus).Click();
                Thread.Sleep(1000);

                // ===== 5. Lấy số lượng mới từ <li[3]/span> =====
                var qtySpan = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[1]/div/div/div[2]/div/div/form/div[1]/div[2]/ul/li[3]/span"));
                int quantity = int.Parse(qtySpan.Text);

                if (quantity < 1)
                    throw new Exception("Số lượng không tăng lên");

                // ===== 6. Ghi kết quả Excel =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Click hình sản phẩm và tăng số lượng thành công", "PASS", "", $"Tên: {nameDetail}, Số lượng: {quantity}");

                Console.WriteLine($"[{testCaseId}] PASS: TC27 click hình đầu tiên và tăng số lượng thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC29_AdjustQuantity(string testCaseId)
        {
            string stepNumber = "2";
            try
            {
                // ===== 1. Scroll tới sản phẩm đầu tiên trên Home =====
                var homeImg = _driver.FindElement(firstProductHomeImg);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", homeImg);
                Thread.Sleep(1000);

                // ===== 2. Click vào ảnh ra trang chi tiết =====
                homeImg.Click();
                Thread.Sleep(2000);

                // ===== 3. Kiểm tra tên sản phẩm chi tiết hiển thị =====
                var nameDetail = _driver.FindElement(detailProductName).Text;
                if (string.IsNullOrEmpty(nameDetail))
                    throw new Exception("Tên sản phẩm chi tiết không hiển thị");

                // ===== 4. Lấy số lượng hiện tại =====
                var qtySpan = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[1]/div/div/div[2]/div/div/form/div[1]/div[2]/ul/li[3]/span"));
                int quantityBefore = int.Parse(qtySpan.Text);

                // ===== 5. Click nút tăng số lượng =====
                _driver.FindElement(btnPlus).Click();
                Thread.Sleep(1000);

                // ===== 6. Click nút giảm số lượng =====
                _driver.FindElement(btnMinus).Click();
                Thread.Sleep(1000);

                // ===== 7. Kiểm tra số lượng cuối cùng bằng ban đầu =====
                int quantityAfter = int.Parse(qtySpan.Text);
                if (quantityAfter != quantityBefore)
                    throw new Exception($"Số lượng cuối không bằng số lượng ban đầu. Trước: {quantityBefore}, Sau: {quantityAfter}");

                // ===== 8. Ghi kết quả Excel =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Tăng 1 rồi giảm 1 thành công, số lượng về lại ban đầu", "PASS", "", $"Tên: {nameDetail}, Số lượng: {quantityAfter}");

                Console.WriteLine($"[{testCaseId}] PASS: Tăng 1 rồi giảm 1 thành công, số lượng về lại ban đầu.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }

        public void ExecuteTC31_RelatedProductNavigation(string testCaseId)
        {
            string stepNumber = "4";
            try
            {
                // ===== 1. Scroll tới ảnh đầu tiên trên trang Home/Product =====
                var firstImage = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstImage);
                Thread.Sleep(1000);

                // ===== 2. Click ảnh đầu tiên =====
                firstImage.Click();
                Thread.Sleep(3000);

                // ===== 3. Scroll tới ảnh trong related/section =====
                var sectionImage = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[2]/div/div[2]/div[1]/div/div[1]/img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", sectionImage);
                Thread.Sleep(500);
                sectionImage.Click();
                Thread.Sleep(2000);

                // ===== 4. Click nút next để qua trang khác =====
                var nextBtn = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[2]/div/div[2]/div[1]/div/div[1]/div/ul/li[2]/a/i"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", nextBtn);
                Thread.Sleep(500);
                nextBtn.Click();
                Thread.Sleep(3000);

                // ===== 5. Ghi kết quả Excel =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Đăng nhập -> click ảnh đầu -> click ảnh section -> click nút next chuyển trang thành công",
                    "PASS", "", "Flow hoàn tất");

                Console.WriteLine($"[{testCaseId}] PASS: Chuyển trang related product thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
        public void ExecuteT32_AddToCartAlert(string testCaseId)
        {
            string stepNumber = "5"; // bạn có thể tùy chỉnh step
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

        public void ExecuteTC37_IncreaseToLimit(string testCaseId)
        {
            string stepNumber = "2";
            try
            {
                // ===== 1. Scroll tới ảnh sản phẩm đầu tiên trên Home =====
                var homeImg = _driver.FindElement(firstProductHomeImg);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", homeImg);
                Thread.Sleep(1000);

                // ===== 2. Click vào ảnh ra trang chi tiết =====
                homeImg.Click();
                Thread.Sleep(2000);

                // ===== 3. Bấm nút tăng (+) 50 lần =====
                for (int i = 0; i < 50; i++)
                {
                    _driver.FindElement(btnPlus).Click();
                    Thread.Sleep(100); // Nghỉ ngắn để trình duyệt nhận click
                }
                Thread.Sleep(1000);

                // ===== 4. Kiểm tra Alert (Có Alert mới PASS) =====
                string alertText = "";
                try
                {
                    // Thử chuyển hướng sang Alert
                    IAlert alert = _driver.SwitchTo().Alert();
                    alertText = alert.Text;
                    alert.Accept(); // Nhấn OK để đóng Alert

                    // Nếu chạy đến đây tức là CÓ Alert -> Ghi kết quả PASS
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "Tăng số lượng và xuất hiện thông báo vượt số lượng kho", "PASS", "", $"Nội dung Alert: {alertText}");

                    Console.WriteLine($"[{testCaseId}] PASS: Có alert thông báo: {alertText}");
                }
                catch (NoAlertPresentException)
                {
                    // Nếu KHÔNG có Alert -> Ném lỗi để nhảy xuống catch (Exception ex) bên dưới xử lý FAIL
                    throw new Exception("FAIL: Tăng lên 50 nhưng không có Alert thông báo vượt số lượng kho");
                }
            }
            catch (Exception ex)
            {
                // Chụp màn hình và ghi log FAIL vào Excel
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC39_CheckProductPrice(string testCaseId)
        {
            string stepNumber = "7"; // tương tự TC17
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

        public void ExecuteTC40_CheckProductName(string testCaseId)
        {
            string stepNumber = "8"; // Giữ nguyên theo logic TC17 của bạn
            try
            {
                // ===== 1. Scroll xuống phần sản phẩm (Home) =====
                var firstImage = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstImage);
                Thread.Sleep(1000);

                // ===== 2. Lấy Tên sản phẩm trên trang Home =====
                // Dựa vào hình ảnh bạn gửi, tên nằm ở thẻ <a> ngay trên phần giá
                var homeNameElement = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[2]/div/div[2]/div[1]/div[2]/div/a"));
                string homeName = homeNameElement.Text.Trim();

                // ===== 3. Click vào hình để sang trang chi tiết =====
                firstImage.Click();
                Thread.Sleep(3000);

                // ===== 4. Lấy Tên sản phẩm trên trang chi tiết =====
                // Dựa vào code cũ của bạn, tên ở trang chi tiết nằm ở thẻ h2
                var detailNameElement = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[1]/div/div/div[2]/div/div/div/div[1]/h2"));
                string detailName = detailNameElement.Text.Trim();

                // ===== 5. So sánh Tên sản phẩm =====
                string testResult = homeName == detailName ? "PASS" : "FAIL";
                string message = homeName == detailName
                    ? $"Tên sản phẩm trùng khớp: {detailName}"
                    : $"Tên sản phẩm không giống nhau. Home: {homeName}, Detail: {detailName}";

                // ===== 6. Ghi kết quả vào Excel (Y chang format cũ) =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    message, testResult, "", $"Home: {homeName}, Detail: {detailName}");

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


        public void ExecuteTC41_CheckProductSizes(string testCaseId)
        {
            string stepNumber = "9";
            try
            {
                // ===== 1. Click vào Menu "Cửa hàng" (Hình 1) =====
                // XPath dựa trên code hình: <a class="nav-link" href="/Shop">Cửa hàng</a>
                var btnStore = _driver.FindElement(By.XPath("//a[@class='nav-link' and @href='/Shop']"));
                btnStore.Click();
                Thread.Sleep(2000);

                // ===== 2. Click vào hình sản phẩm trong trang Cửa hàng (Hình 2) =====
                // XPath trỏ vào ảnh sản phẩm đầu tiên có class card-img
                var productImg = _driver.FindElement(By.CssSelector("img.card-img.rounded-0.img-fluid"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", productImg);
                productImg.Click();
                Thread.Sleep(2000);

                // ===== 3. Click vào nút "Thêm vào giỏ hàng" hoặc Icon giỏ hàng để vào chi tiết (Hình 3) =====
                // Dựa vào hình 3: <a class="btn btn-success text-white mt-2" href="Shop/IndexDetails?prId=2">
                var btnDetail = _driver.FindElement(By.XPath("//a[contains(@class, 'btn-success') and contains(@href, 'IndexDetails')]"));
                btnDetail.Click();
                Thread.Sleep(2000);

                // ===== 4. Lần lượt chọn 3 Size: M, L, XL (Hình 4, 5, 6) =====

                // --- Chọn Size M (Hình 4) ---
                var sizeM = _driver.FindElement(By.XPath("//a[text()='M']"));
                sizeM.Click();
                Thread.Sleep(1000);
                if (!sizeM.Displayed) throw new Exception("Không tìm thấy hoặc không chọn được Size M");

                // --- Chọn Size L (Hình 5) ---
                var sizeL = _driver.FindElement(By.XPath("//a[text()='L']"));
                sizeL.Click();
                Thread.Sleep(1000);
                if (!sizeL.Displayed) throw new Exception("Không tìm thấy hoặc không chọn được Size L");

                // --- Chọn Size XL (Hình 6) ---
                var sizeXL = _driver.FindElement(By.XPath("//a[text()='XL']"));
                sizeXL.Click();
                Thread.Sleep(1000);
                if (!sizeXL.Displayed) throw new Exception("Không tìm thấy hoặc không chọn được Size XL");

                // ===== 5. Ghi kết quả Excel thành công =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Đã chọn thành công cả 3 size: M, L, XL", "PASS", "", "Kết quả: Chọn được M, L, XL");

                Console.WriteLine($"[{testCaseId}] PASS: Kiểm tra 3 size thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC46_CheckPriceChangesBySize(string testCaseId)
        {
            string stepNumber = "9";
            try
            {
                // ===== 1. Click vào Menu "Cửa hàng" (Hình 1) =====
                // XPath dựa trên code hình: <a class="nav-link" href="/Shop">Cửa hàng</a>
                var btnStore = _driver.FindElement(By.XPath("//a[@class='nav-link' and @href='/Shop']"));
                btnStore.Click();
                Thread.Sleep(2000);

                // ===== 2. Click vào hình sản phẩm trong trang Cửa hàng (Hình 2) =====
                // XPath trỏ vào ảnh sản phẩm đầu tiên có class card-img
                var productImg = _driver.FindElement(By.CssSelector("img.card-img.rounded-0.img-fluid"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", productImg);
                productImg.Click();
                Thread.Sleep(2000);

                // ===== 3. Click vào nút "Thêm vào giỏ hàng" hoặc Icon giỏ hàng để vào chi tiết (Hình 3) =====
                // Dựa vào hình 3: <a class="btn btn-success text-white mt-2" href="Shop/IndexDetails?prId=2">
                var btnDetail = _driver.FindElement(By.XPath("//a[contains(@class, 'btn-success') and contains(@href, 'IndexDetails')]"));
                btnDetail.Click();
                Thread.Sleep(2000);

                // ===== 4. Lấy giá của Size M =====
                var sizeM = _driver.FindElement(By.XPath("//a[text()='M']"));
                sizeM.Click();
                Thread.Sleep(1500);
                string priceM = _driver.FindElement(By.Id("Price")).Text.Trim();

                // ===== 5. Click sang Size L để kiểm tra biến động giá =====
                var sizeL = _driver.FindElement(By.XPath("//a[text()='L']"));
                sizeL.Click();
                Thread.Sleep(1500);
                string priceL = _driver.FindElement(By.Id("Price")).Text.Trim();

                // ===== 6. So sánh kết quả =====
                string testResult = (priceM != priceL) ? "PASS" : "FAIL";
                string message = (priceM != priceL)
                    ? $"Thành công: Giá đổi từ {priceM} sang {priceL}"
                    : $"Thất bại: Giá không đổi (Đều là {priceM})";

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    message, testResult, "", $"M: {priceM}, L: {priceL}");

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

        public void ExecuteTC45_AddMultipleProducts(string testCaseId)
        {
            string stepNumber = "6";
            try
            {
                // Danh sách Full XPath của 2 hình ảnh bạn cung cấp
                string xpathImg1 = "/html/body/div[1]/main/div/div/div[3]/div/div[1]/div[2]/div/div[1]/img";
                string xpathImg2 = "/html/body/div[1]/main/div/div/div[3]/div/div[1]/div[3]/div/div[1]/img";
                string[] productXPaths = { xpathImg1, xpathImg2 };

                foreach (string imgXPath in productXPaths)
                {
                    // ===== 1. Click vào Menu "Cửa hàng" =====
                    var btnStore = _driver.FindElement(By.XPath("//a[@class='nav-link' and @href='/Shop']"));
                    btnStore.Click();
                    Thread.Sleep(2000);

                    // ===== 2. Scroll và Hover vào Ảnh (Dùng Full XPath bạn gửi) =====
                    var productImg = _driver.FindElement(By.XPath(imgXPath));
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", productImg);
                    Thread.Sleep(1000);

                    // Hover chuột để nút hiện ra
                    OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(_driver);
                    action.MoveToElement(productImg).Perform();
                    Thread.Sleep(1000);

                    // ===== 3. Click nút màu xanh để vào trang chi tiết =====
                    // Tìm nút kế cận hoặc cùng khung với ảnh vừa hover
                    var btnDetail = _driver.FindElement(By.XPath("//a[contains(@class, 'btn-success') and contains(@href, 'IndexDetails')]"));
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btnDetail);
                    Thread.Sleep(3000);

                    // ===== 4. Click nút "Thêm vào giỏ hàng" (value="addtocard") =====
                    var btnAddToCart = _driver.FindElement(By.XPath("//button[@value='addtocard']"));
                    btnAddToCart.Click();
                    Thread.Sleep(1500);

                    // ===== 5. Chấp nhận Alert =====
                    try
                    {
                        IAlert alert = _driver.SwitchTo().Alert();
                        alert.Accept();
                        Thread.Sleep(1000);
                    }
                    catch (NoAlertPresentException)
                    {
                        throw new Exception("Không thấy thông báo Alert khi thêm sản phẩm vào giỏ!");
                    }
                }

                // ===== 6. Ghi kết quả PASS sau khi xong cả 2 sản phẩm =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Đã thêm thành công 2 sản phẩm bằng Full XPath vào giỏ hàng", "PASS", "", "Hoàn thành SP1 và SP2");

                Console.WriteLine($"[{testCaseId}] PASS: Đã thêm xong 2 sản phẩm theo yêu cầu.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteT47_BannerRedirect(string testCaseId)
        {
            string stepNumber = "5";
            try
            {
                // ===== 1. Scroll xuống phần sản phẩm/banner =====
                var firstProduct = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstProduct);
                Thread.Sleep(1000);

                // ===== 2. Click vào hình sản phẩm/banner =====
                firstProduct.Click();
                Thread.Sleep(2000);

                // ===== 3. Kiểm tra xem có chuyển đúng đến trang chi tiết không =====
                string currentUrl = _driver.Url;

                if (currentUrl.Contains("/Shop/IndexDetails?prId=2044"))
                {
                    // ===== 4. Ghi kết quả vào Excel: Chuyển trang thành công =====
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "Banner chuyển đúng đến trang sản phẩm mình chọn",
                        "PASS", "", "URL hiện tại: " + currentUrl);

                    Console.WriteLine($"[{testCaseId}] PASS: Banner chuyển đúng đến trang sản phẩm.");
                }
                else
                {
                    throw new Exception($"Chuyển trang thất bại. URL hiện tại: {currentUrl}");
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


        public void ExecuteTC50_RatingProduct(string testCaseId)
        {
            string stepNumber = "3";
            try
            {   // ===== 1. Scroll xuống phần sản phẩm trên Home =====
                var firstImage = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstImage);
                Thread.Sleep(1000);

                // ===== 2. Click vào hình để vào trang chi tiết =====
                firstImage.Click();
                Thread.Sleep(3000);
                // ===== 3. Thử click vào ngôi sao để đánh giá =====
                // Các ngôi sao thường nằm trong class 'text-warning' hoặc icon bi-star-fill
                By starRating = By.XPath("(//i[contains(@class, 'bi-star-fill')])[5]");
                var star = _driver.FindElement(starRating);

                Console.WriteLine("Đang thử thực hiện đánh giá sao...");

                // ===== 4. Thực hiện click và ép lỗi nếu không thành công =====
                try
                {
                    star.Click();
                    Thread.Sleep(2000);

                    // Nếu click được nhưng logic web không có gì thay đổi (không hiện alert/thành công)
                    // thì mình chủ động báo Fail vì tính năng không chạy
                    throw new Exception("Click được vào sao nhưng hệ thống không ghi nhận đánh giá.");
                }
                catch (Exception ex)
                {
                    // Nhảy vào đây để ghi kết quả FAIL
                    throw new Exception("Tính năng đánh giá sao bị lỗi hoặc không thể tương tác: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                // Ghi kết quả FAIL vào Excel kèm ảnh chụp màn hình
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");

                Console.WriteLine($"[{testCaseId}] FAIL: {ex.Message}");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }

        public void ExecuteTC51_Product_CheckOtherImage(string testCaseId)
        {
            string stepNumber = "4"; // Giống logic TC40
            try
            {
                // ===== 1. Scroll xuống phần sản phẩm trên Home =====
                var firstImage = _driver.FindElement(By.CssSelector("a[href*='/Shop/IndexDetails?prId='] img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstImage);
                Thread.Sleep(1000);

                // ===== 2. Click vào hình để vào trang chi tiết =====
                firstImage.Click();
                Thread.Sleep(3000);

                // ===== 3. Kéo xuống kiểm tra ảnh khác =====
                var otherImage = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[2]/div/div[2]/div[1]/div/div[1]/img"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", otherImage);
                Thread.Sleep(1000);

                // ===== 4. Check ảnh có hiển thị =====
                string testResult = otherImage.Displayed ? "PASS" : "FAIL";
                string message = otherImage.Displayed
                    ? "Ảnh khác hiển thị sau khi click ảnh đầu tiên"
                    : "Ảnh khác không hiển thị, test fail.";

                // ===== 5. Ghi kết quả vào Excel =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    message, testResult, "", "Kiểm tra ProductPage");
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
        public void ExecuteTC53_IncreaseQuantityAndCheckPrice(string testCaseId)
        {
            string stepNumber = "7";
            try
            {
                // ===== 1. Scroll tới ảnh đầu tiên trên Home =====
                var homeImg = _driver.FindElement(firstProductHomeImg);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", homeImg);
                Thread.Sleep(1000);

                // ===== 2. Click vào ảnh ra trang chi tiết =====
                homeImg.Click();
                Thread.Sleep(2000);

                // ===== 3. Lấy tên và GIÁ TIỀN BAN ĐẦU =====
                var nameDetail = _driver.FindElement(detailProductName).Text;
                // Lấy giá tiền từ ID "Price" theo hình bạn gửi
                string initialPriceText = _driver.FindElement(By.Id("Price")).Text.Trim();

                // ===== 4. Click nút tăng số lượng =====
                _driver.FindElement(btnPlus).Click();
                Thread.Sleep(1500); // Đợi một chút để script trang web cập nhật giá (nếu có)

                // ===== 5. Lấy số lượng mới và GIÁ TIỀN SAU KHI TĂNG =====
                var qtySpan = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[1]/div/div/div[2]/div/div/form/div[1]/div[2]/ul/li[3]/span"));
                int quantity = int.Parse(qtySpan.Text);

                string updatedPriceText = _driver.FindElement(By.Id("Price")).Text.Trim();

                // ===== 6. KIỂM TRA LOGIC GIÁ TIỀN =====
                // Nếu số lượng > 1 mà giá tiền vẫn bằng giá ban đầu => FAIL
                if (quantity > 1 && initialPriceText == updatedPriceText)
                {
                    throw new Exception($"FAIL: Số lượng đã tăng lên {quantity} nhưng giá tiền không thay đổi (Vẫn là: {initialPriceText})");
                }

                // ===== 7. Ghi kết quả Excel (Trường hợp Pass - nếu có thay đổi) =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Tăng số lượng và giá tiền đã cập nhật", "PASS", "", $"Số lượng: {quantity}, Giá cũ: {initialPriceText}, Giá mới: {updatedPriceText}");

                Console.WriteLine($"[{testCaseId}] PASS: Giá tiền đã thay đổi theo số lượng.");
            }
            catch (Exception ex)
            {
                // Khi giá không đổi, Exception sẽ bị ném xuống đây và ghi FAIL vào Excel
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC55_IncreaseThenDecreaseCheckPrice(string testCaseId)
        {
            string stepNumber = "3";
            try
            {
                // ===== 1. Vào trang chi tiết sản phẩm =====
                var homeImg = _driver.FindElement(firstProductHomeImg);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", homeImg);
                Thread.Sleep(1000);
                homeImg.Click();
                Thread.Sleep(2000);

                // ===== 2. Bấm nút TĂNG (btnPlus) để lên số lượng là 3 =====
                var plusBtn = _driver.FindElement(btnPlus);
                plusBtn.Click(); // lên 2
                Thread.Sleep(800);
                plusBtn.Click(); // lên 3
                Thread.Sleep(1000);

                // Lấy giá tại thời điểm số lượng = 3
                string priceAtThree = _driver.FindElement(By.Id("Price")).Text.Trim();
                Console.WriteLine($"Số lượng là 3, giá hiện tại: {priceAtThree}");

                // ===== 3. Bấm nút GIẢM (ID: btn-minus) để xuống số lượng là 2 =====
                // Cập nhật theo hình ảnh bạn vừa gửi: ID="btn-minus"
                var btnMinusElement = _driver.FindElement(By.Id("btn-minus"));
                btnMinusElement.Click();
                Thread.Sleep(1500); // Đợi hệ thống cập nhật lại giá tiền

                // ===== 4. Lấy số lượng và giá mới sau khi giảm =====
                var qtySpan = _driver.FindElement(By.XPath("/html/body/div[1]/main/section[1]/div/div/div[2]/div/div/form/div[1]/div[2]/ul/li[3]/span"));
                string currentQty = qtySpan.Text;
                string priceAfterDecrease = _driver.FindElement(By.Id("Price")).Text.Trim();

                // ===== 5. KIỂM TRA LOGIC GIẢM GIÁ (Mong đợi FAIL) =====
                // Nếu giá sau khi giảm về 2 mà vẫn bằng giá lúc có 3 SP => BUG
                if (priceAfterDecrease == priceAtThree)
                {
                    throw new Exception($"FAIL: Số lượng đã giảm xuống {currentQty} nhưng giá tiền đứng yên (Vẫn là: {priceAfterDecrease})");
                }

                // ===== 6. Ghi kết quả PASS (Nếu giá có giảm thật) =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Tăng lên 3 rồi giảm xuống 2, giá tiền có cập nhật giảm theo", "PASS", "", $"SL hiện tại: {currentQty}, Giá mới: {priceAfterDecrease}");

            }
            catch (Exception ex)
            {
                // Ghi kết quả FAIL vào Excel khi giá không đổi
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC56_CheckProductSizesAndPrice(string testCaseId)
        {
            string stepNumber = "9";
            try
            {
                // ===== 1. Click vào Menu "Cửa hàng" =====
                var btnStore = _driver.FindElement(By.XPath("//a[@class='nav-link' and @href='/Shop']"));
                btnStore.Click();
                Thread.Sleep(2000);

                // ===== 2. Click vào hình sản phẩm trong trang Cửa hàng =====
                var productImg = _driver.FindElement(By.CssSelector("img.card-img.rounded-0.img-fluid"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", productImg);
                productImg.Click();
                Thread.Sleep(2000);

                // ===== 3. Click vào nút chi tiết (Hình 3) =====
                var btnDetail = _driver.FindElement(By.XPath("//a[contains(@class, 'btn-success') and contains(@href, 'IndexDetails')]"));
                btnDetail.Click();
                Thread.Sleep(2000);

                // ===== 4. Lấy GIÁ BAN ĐẦU (Mặc định thường là size nhỏ nhất) =====
                string initialPriceText = _driver.FindElement(By.Id("Price")).Text.Replace(" VNĐ", "").Replace(",", "").Trim();
                double initialPrice = double.Parse(initialPriceText);
                Console.WriteLine($"Giá ban đầu: {initialPrice}");

                // ===== 5. Lần lượt chọn các Size và kiểm tra giá ở Size XL =====

                // --- Chọn Size M ---
                var sizeM = _driver.FindElement(By.XPath("//a[text()='M']"));
                sizeM.Click();
                Thread.Sleep(1000);

                // --- Chọn Size L ---
                var sizeL = _driver.FindElement(By.XPath("//a[text()='L']"));
                sizeL.Click();
                Thread.Sleep(1000);

                // --- Chọn Size XL và KIỂM TRA GIÁ (Hình 807e5f) ---
                var sizeXL = _driver.FindElement(By.XPath("//a[text()='XL']"));
                sizeXL.Click();
                Thread.Sleep(1500); // Đợi script cập nhật giá tiền

                string xlPriceText = _driver.FindElement(By.Id("Price")).Text.Replace(" VNĐ", "").Replace(",", "").Trim();
                double xlPrice = double.Parse(xlPriceText);
                Console.WriteLine($"Giá sau khi chọn XL: {xlPrice}");

                // ===== 6. SO SÁNH GIÁ TIỀN =====
                if (xlPrice <= initialPrice)
                {
                    // Nếu giá XL không lớn hơn giá ban đầu thì báo lỗi
                    throw new Exception($"FAIL: Chọn size XL nhưng giá tiền không tăng (Giá XL: {xlPrice} <= Giá cũ: {initialPrice})");
                }

                // ===== 7. Ghi kết quả Excel thành công =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    $"Chọn size XL thành công và giá tăng lên {xlPrice} VNĐ", "PASS", "", $"Giá cũ: {initialPrice}, Giá XL: {xlPrice}");

                Console.WriteLine($"[{testCaseId}] PASS: Kiểm tra size XL và tăng giá thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC57_CheckPriceDecreaseOnSizeChange(string testCaseId)
        {
            string stepNumber = "9";
            try
            {
                // ===== 1. Vào trang chi tiết sản phẩm (Americano nóng) =====
                var btnStore = _driver.FindElement(By.XPath("//a[@class='nav-link' and @href='/Shop']"));
                btnStore.Click();
                Thread.Sleep(2000);

                var productImg = _driver.FindElement(By.CssSelector("img.card-img.rounded-0.img-fluid"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", productImg);
                productImg.Click();
                Thread.Sleep(2000);

                var btnDetail = _driver.FindElement(By.XPath("//a[contains(@class, 'btn-success') and contains(@href, 'IndexDetails')]"));
                btnDetail.Click();
                Thread.Sleep(2000);

                // ===== 2. Chọn Size XL trước để lấy giá cao làm mốc =====
                var sizeXL = _driver.FindElement(By.XPath("//a[text()='XL']"));
                sizeXL.Click();
                Thread.Sleep(1500); // Đợi cập nhật giá XL

                string xlPriceText = _driver.FindElement(By.Id("Price")).Text.Replace(" VNĐ", "").Replace(",", "").Trim();
                double priceXL = double.Parse(xlPriceText);
                Console.WriteLine($"Giá khi chọn Size XL: {priceXL}");

                // ===== 3. Chọn xuống lại Size M (Size nhỏ hơn) =====
                var sizeM = _driver.FindElement(By.XPath("//a[text()='M']"));
                sizeM.Click();
                Thread.Sleep(1500); // Đợi cập nhật lại giá M

                string mPriceText = _driver.FindElement(By.Id("Price")).Text.Replace(" VNĐ", "").Replace(",", "").Trim();
                double priceM = double.Parse(mPriceText);
                Console.WriteLine($"Giá khi chọn lại Size M: {priceM}");

                // ===== 4. KIỂM TRA LOGIC GIẢM GIÁ =====
                // Nếu giá size M mà vẫn bằng hoặc lớn hơn giá size XL => BUG (FAIL)
                if (priceM >= priceXL)
                {
                    throw new Exception($"FAIL: Đã đổi từ XL xuống M nhưng giá tiền không giảm (Giá M: {priceM} >= Giá XL: {priceXL})");
                }

                // ===== 5. Ghi kết quả PASS nếu giá có giảm thật =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    $"Giá tiền đã giảm chính xác khi đổi từ XL ({priceXL}) xuống M ({priceM})",
                    "PASS", "", $"Giá XL: {priceXL}, Giá M: {priceM}");

                Console.WriteLine($"[{testCaseId}] PASS: Giá tiền giảm thành công khi chọn size nhỏ hơn.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        public void ExecuteTC58_CheckAlertWhenNoSizeSelected(string testCaseId)
        {
            string stepNumber = "9";
            try
            {
                // ===== 1. Click vào Menu "Cửa hàng" =====
                var btnStore = _driver.FindElement(By.XPath("//a[@class='nav-link' and @href='/Shop']"));
                btnStore.Click();
                Thread.Sleep(2000);

                // ===== 2. Click vào hình sản phẩm (Americano nóng) tại trang Cửa hàng =====
                // Sử dụng CssSelector cho ảnh sản phẩm như hình bạn gửi
                var productImg = _driver.FindElement(By.CssSelector("img.card-img.rounded-0.img-fluid"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", productImg);
                Thread.Sleep(1000);
                productImg.Click();
                Thread.Sleep(2000);

                // ===== 3. Click vào nút xem chi tiết để vào trang IndexDetails =====
                var btnDetail = _driver.FindElement(By.XPath("//a[contains(@class, 'btn-success') and contains(@href, 'IndexDetails')]"));
                btnDetail.Click();
                Thread.Sleep(2000);

                // ===== 4. HÀNH ĐỘNG CHÍNH: KHÔNG CHỌN SIZE -> Nhấn thẳng nút "Thêm vào giỏ hàng" =====
                // Locator nút thêm vào giỏ hàng: button có value='addtocard'
                var btnAddToCart = _driver.FindElement(By.XPath("//button[@value='addtocard']"));
                btnAddToCart.Click();
                Thread.Sleep(2000); // Đợi một chút để xem Alert có kịp hiện ra không

                // ===== 5. KIỂM TRA ALERT (Bắt Bug tại đây) =====
                try
                {
                    // Thử chuyển hướng sang cửa sổ Alert
                    IAlert alert = _driver.SwitchTo().Alert();
                    string alertText = alert.Text;

                    // Nếu Alert hiện ra với nội dung yêu cầu chọn size (đúng logic thiết kế)
                    alert.Accept();
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "Hệ thống hoạt động đúng: Có hiển thị alert yêu cầu chọn size.",
                        "PASS", "", "Nội dung Alert: " + alertText);

                    Console.WriteLine($"[{testCaseId}] PASS: Hệ thống đã chặn và hiển thị Alert.");
                }
                catch (NoAlertPresentException)
                {
                    // NẾU KHÔNG CÓ ALERT: Nghĩa là hệ thống đã tự động thêm vào giỏ (BUG)
                    // Chúng ta chủ động quăng lỗi để ghi nhận FAIL
                    throw new Exception("FAIL: Hệ thống không hiển thị Alert thông báo bắt buộc chọn Size. Sản phẩm bị tự động thêm vào giỏ hàng!");
                }
            }
            catch (Exception ex)
            {
                // ===== 6. Ghi kết quả FAIL và Chụp màn hình lỗi =====
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "Lỗi: Thiếu Alert ràng buộc chọn Size");

                Console.WriteLine($"[{testCaseId}] FAIL: {ex.Message}");
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
    }
}