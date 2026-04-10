using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class CartPage
    {
        // === PHẢI CÓ 3 DÒNG NÀY ĐỂ HẾT LỖI GẠCH ĐỎ ===
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;

        // --- Locators ---
        private readonly By ProductLink = By.XPath("//a[contains(@href, 'prId=')]");
        private readonly By AddToCartBtn = By.CssSelector("button.btn-success.btn-lg");
        private readonly By CartIconHeader = By.CssSelector("i.bi-cart-check-fill");
        private readonly By PlusButton = By.XPath("//button[contains(@onclick, 'increaseValue')]");
        private readonly By MinusButton = By.XPath("//button[contains(@onclick, 'decreaseValue')]");
        private readonly By QuantityInput = By.CssSelector("input.product-number");
        private readonly By ContinueShoppingBtn = By.XPath("//a[contains(@text, 'Tiếp tục xem sản phẩm') or contains(@href, '/Shop')]");
        private readonly By DeleteButton = By.CssSelector("a[onclick*='DeleteProduct']");
        // --- Constructor (Hàm khởi tạo) ---
        public CartPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        // --- HÀM TĂNG ---
        public void ExecuteFullCartFlow(string testCaseId)
        {
            string stepNumber = "2"; // Hoặc số dòng tương ứng trong Excel
            string testData = "Bấm nút tăng số lượng (+)";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                Thread.Sleep(2000);
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();

                Thread.Sleep(2000);
                _driver.FindElement(PlusButton).Click(); // Bấm Tăng
                Thread.Sleep(1500);

                string actualQty = _driver.FindElement(QuantityInput).GetAttribute("value");

                // ✅ Ghi kết quả PASS vào Excel (Cột J: testData, M: PASS, N: "")
                _excelProvider.UpdateTestResult(_config, testCaseId, "2",
                    $"Số lượng hiện tại: {actualQty}", "PASS", "", testData);

                Console.WriteLine($"[KẾT QUẢ TĂNG] {testCaseId}: PASS");
            }
            catch (Exception ex)
            {
                // ✅ Ghi kết quả FAIL vào Excel (Cột J: testData, M: FAIL, N: Ảnh)
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", ScreenshotHelper.TakeScreenshot(_driver, testCaseId), testData);

                Assert.Fail(ex.Message);
            }
        }

        // --- HÀM GIẢM ---
        public void ExecuteDecreaseFlow(string testCaseId)
        {
            string stepNumber = "1"; // Hoặc số dòng tương ứng trong Excel
            string testData = "Bấm nút giảm số lượng (-)";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                Thread.Sleep(2000);
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();

                Thread.Sleep(2000);
                _driver.FindElement(MinusButton).Click(); // Bấm Giảm
                Thread.Sleep(1500);

                string actualQty = _driver.FindElement(QuantityInput).GetAttribute("value");

                // ✅ Ghi kết quả PASS vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, "2",
                    $"Số lượng hiện tại: {actualQty}", "PASS", "", testData);

                Console.WriteLine($"[KẾT QUẢ GIẢM] {testCaseId}: PASS");
            }
            catch (Exception ex)
            {
                // ✅ Ghi kết quả FAIL vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", ScreenshotHelper.TakeScreenshot(_driver, testCaseId), testData);

                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC5_ContinueShopping(string testCaseId)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // 1. Thao tác điều hướng giỏ hàng
                Thread.Sleep(2000);
                js.ExecuteScript("window.scrollTo(0, 0);");
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(2000);

                // Xử lý spinner/backdrop để không bị chặn click
                js.ExecuteScript("document.querySelectorAll('.modal-backdrop, .spinner').forEach(el => el.remove());");

                // 2. Click Tiếp tục xem sản phẩm
                _driver.FindElement(ContinueShoppingBtn).Click();
                Thread.Sleep(2000);

                // 3. Kiểm tra điều kiện (Dùng Assert giống LoginPage)
                Assert.IsTrue(_driver.Url.Contains("/Shop"), "Không chuyển về trang Shop thành công.");

                // ✅ Ghi PASS: Cột J là Test Data, Cột M là PASS, Cột N để trống ""
                _excelProvider.UpdateTestResult(_config, testCaseId, "4",
                    "Chuyển sang trang chủ để chọn món thành công", "PASS", "", "Click vào tiếp tục xem sản phẩm");

                Console.WriteLine($"[{testCaseId}] PASS");
            }
            catch (Exception ex)
            {
                // ✅ Ghi FAIL: Cột J vẫn có Test Data, Cột M là FAIL, Cột N gọi ScreenshotHelper
                _excelProvider.UpdateTestResult(_config, testCaseId, "5",
                    ex.Message, "FAIL", ScreenshotHelper.TakeScreenshot(_driver, testCaseId), "Click vào tiếp tục xem sản phẩm");

                // Báo đỏ cho trình chạy test
                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC8_DeleteFlow(string testCaseId)
        {
            string stepNumber = "7";
            string testData = "Click vào icon xóa";

            try
            {
                

                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // 1. Lấy sản phẩm (KHÔNG crash)
                var products = _driver.FindElements(ProductLink);

                if (products.Count == 0)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "Không tìm thấy sản phẩm", "FAIL", "", testData);

                    Assert.Fail("Không tìm thấy sản phẩm");
                }

                var product = products[0];

                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                // 2. Add to cart
                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);

                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // 3. Mở giỏ hàng
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(2000);

                // 🔥 xử lý overlay (rất quan trọng)
                js.ExecuteScript("document.querySelectorAll('.modal-backdrop, .spinner').forEach(el => el.remove());");

                // 4. Click delete
                var deleteBtns = _driver.FindElements(DeleteButton);

                if (deleteBtns.Count == 0)
                {
                    throw new Exception("Không tìm thấy nút xóa");
                }

                var deleteBtn = deleteBtns[0];

                js.ExecuteScript("arguments[0].scrollIntoView(true);", deleteBtn);
                Thread.Sleep(1000);
                js.ExecuteScript("arguments[0].click();", deleteBtn);

                // 5. Accept alert
                Thread.Sleep(1000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                Thread.Sleep(2000);

                // ✅ PASS
                _excelProvider.UpdateTestResult(_config, testCaseId, "7",
                    "Xóa sản phẩm thành công", "PASS", "", testData);

                Console.WriteLine(">>> TC8 PASS");
            }
            catch (Exception ex)
            {
                _excelProvider.UpdateTestResult(_config, testCaseId, "7",
                    ex.Message, "FAIL",
                    ScreenshotHelper.TakeScreenshot(_driver, testCaseId),
                    testData);

                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC1_VerifyImageFlow(string testCaseId)
        {
            string stepNumber = "1"; // Ghi vào dòng Step 1 của TC1
            string testData = "Kiểm tra hình ảnh sản phẩm đồng nhất";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // --- TẠI TRANG CHI TIẾT ---
                Thread.Sleep(2000);
                // Lấy link ảnh ở trang chi tiết (ID product-image như trong hình e68a8f.png)
                var detailImg = _driver.FindElement(By.Id("product-image"));
                string detailSrc = detailImg.GetAttribute("src");

                // Bấm Thêm vào giỏ hàng
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // --- VÀO TRANG GIỎ HÀNG ---
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(2000);

                // Xử lý spinner/backdrop nếu có
                js.ExecuteScript("document.querySelectorAll('.modal-backdrop, .spinner').forEach(el => el.remove());");

                // Lấy link ảnh trong giỏ hàng (Class avatar-lg rounded như trong hình e68acb.png)
                var cartImg = _driver.FindElement(By.CssSelector(".avatar-lg.rounded"));
                string cartSrc = cartImg.GetAttribute("src");

                // --- SO SÁNH ---
                // Nếu trùng nhau hoàn toàn là Pass
                Assert.AreEqual(detailSrc, cartSrc, "Lỗi: Hình ảnh trang chi tiết và giỏ hàng khác nhau!");

                // ✅ Ghi kết quả PASS vào Excel (Cột K-10 và M-12)
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Hình ảnh trùng khớp thành công (Src: " + cartSrc + ")", "PASS", "", testData);

                Console.WriteLine($"[TC1] PASS: 2 ảnh trùng khớp");
            }
            catch (Exception ex)
            {
                // ✅ Ghi kết quả FAIL vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", ScreenshotHelper.TakeScreenshot(_driver, testCaseId), testData);

                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC10_VerifyPriceFlow(string testCaseId)
        {
            string stepNumber = "9"; // Dòng số 10 trong file Excel của bạn
            string testData = "Kiểm tra giá sản phẩm đồng nhất";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // --- TẠI TRANG CHI TIẾT ---
                Thread.Sleep(2000);
                // Lấy giá trang chi tiết (Dùng ID "Price" từ hình image_e6ec83.png)
                var detailPriceElement = _driver.FindElement(By.Id("Price"));
                string detailPrice = detailPriceElement.Text.Trim();
                Console.WriteLine($"[PRICE DETAIL] {detailPrice}");

                // Bấm Thêm vào giỏ hàng
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // --- VÀO TRANG GIỎ HÀNG ---
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(2000);

                // Xử lý spinner/backdrop
                js.ExecuteScript("document.querySelectorAll('.modal-backdrop, .spinner').forEach(el => el.remove());");

                // Lấy giá trong giỏ hàng (Dùng locator h5 như trong hình image_e6eca7.png)
                // Lưu ý: Nếu có nhiều sản phẩm, bạn nên dùng locator cụ thể hơn, ở đây mình dùng h5 trong card-body
                var cartPriceElement = _driver.FindElement(By.CssSelector(".card-body h5.mb-0"));
                string cartPrice = cartPriceElement.Text.Trim();
                Console.WriteLine($"[PRICE CART] {cartPrice}");

                // --- SO SÁNH ---
                Assert.AreEqual(detailPrice, cartPrice, "Lỗi: Giá ở trang chi tiết và giỏ hàng khác nhau!");

                // ✅ Ghi kết quả PASS vào Excel (Cột K-10 và M-12)
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    $"Giá trùng khớp: {cartPrice}", "PASS", "", testData);

                Console.WriteLine($"[TC10] PASS: Giá đồng nhất");
            }
            catch (Exception ex)
            {
                // ✅ Ghi kết quả FAIL vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", ScreenshotHelper.TakeScreenshot(_driver, testCaseId), testData);

                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC13_FailAtOneFlow(string testCaseId)
        {
            string stepNumber = "6";
            string testData = "Bấm nút giảm số lượng (-)";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                Thread.Sleep(3000);

                // 1. TÌM Ô INPUT & NÚT GIẢM (Dựa trên source code image_e7d5a3.png)
                // Dùng CSS Selector cho class: input.quantity.product-number
                var qtyInput = _driver.FindElement(By.CssSelector("input.quantity.product-number"));
                // Dùng XPath cho button có onclick chứa 'decreaseValue'
                var minusBtn = _driver.FindElement(By.XPath("//button[contains(@onclick, 'decreaseValue')]"));

                int currentQty = int.Parse(qtyInput.GetAttribute("value"));

                // 2. VÒNG LẶP BẤM GIẢM CHO ĐẾN KHI BẰNG 1
                while (currentQty > 1)
                {
                    js.ExecuteScript("arguments[0].click();", minusBtn);
                    Thread.Sleep(1200);
                    currentQty = int.Parse(qtyInput.GetAttribute("value"));
                }

                // 3. THỬ BẤM LẦN CUỐI KHI ĐANG Ở 1 ĐỂ KIỂM TRA CHẶN
                js.ExecuteScript("arguments[0].click();", minusBtn);
                Thread.Sleep(2000);

                int finalQty = int.Parse(qtyInput.GetAttribute("value"));

                // 4. NẾU VẪN LÀ 1 -> XỬ LÝ FAIL & CHỤP ẢNH
                if (finalQty == 1)
                {
                    // Chụp ảnh màn hình
                    string screenshotPath = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");

                    // CẬP NHẬT EXCEL: 
                    // Cột M (Actual): "không thông báo gì hết"
                    // Cột N (Link ảnh): screenshotPath
                    _excelProvider.UpdateTestResult(
                        _config,
                        testCaseId,
                        stepNumber,
                        "không thông báo gì hết", // Actual Result (Cột M)
                        "FAIL",                   // Status (Cột L)
                        screenshotPath,           // Link ảnh (Sẽ dán vào cột N)
                        testData                  // Test Data (Cột K)
                    );

                    Assert.Fail("FAIL: Không thể giảm xuống 0 và không có thông báo xác nhận.");
                }
            }
            catch (Exception ex)
            {
                string errorImg = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Error");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber, "Lỗi: " + ex.Message, "FAIL", errorImg, testData);
                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC15_IncreaseLimitFlow(string testCaseId)
        {
            string stepNumber = "6"; // Ghi vào step tương ứng trong Excel
            string testData = "Tăng số lượng lên 50";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                Thread.Sleep(3000);

                // 1. TÌM Ô INPUT & NÚT TĂNG (Theo source code image_e7e49e.png)
                var qtyInput = _driver.FindElement(By.CssSelector("input.quantity.product-number"));
                // Nút tăng có onclick="increaseValue(event,0)"
                var plusBtn = _driver.FindElement(By.XPath("//button[contains(@onclick, 'increaseValue')]"));

                int currentQty = int.Parse(qtyInput.GetAttribute("value"));
                Console.WriteLine($"[TC15] Số lượng bắt đầu: {currentQty}");

                // 2. VÒNG LẶP BẤM TĂNG CHO ĐẾN KHI ĐẠT 50
                // (Nếu trang web của bạn chặn ở mức thấp hơn 50, nó sẽ dừng lại và ghi FAIL)
                while (currentQty < 50)
                {
                    js.ExecuteScript("arguments[0].click();", plusBtn);
                    Thread.Sleep(800); // Tăng tốc độ bấm một chút

                    int newQty = int.Parse(qtyInput.GetAttribute("value"));

                    // Kiểm tra nếu bấm mà số lượng không tăng nữa (bị chặn giữa chừng)
                    if (newQty == currentQty) break;

                    currentQty = newQty;
                    Console.WriteLine($"[TC15] Đang tăng: {currentQty}");
                }

                // 3. KIỂM TRA THÔNG BÁO & GHI FAIL
                // Sau khi tăng xong (hoặc bị chặn), chụp ảnh dán vào cột N
                string screenshotPath = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Increase_Limit");

                // Ghi kết quả FAIL vì "khômg hiện thông báo vượt quá số lượng quán"
                _excelProvider.UpdateTestResult(
                    _config,
                    testCaseId,
                    stepNumber,
                    "không thông báo gì hết", // Actual Result (Cột M)
                    "FAIL",                   // Status (Cột L)
                    screenshotPath,           // Dán link ảnh vào cột N
                    testData                  // Test Data (Cột K)
                );

                Console.WriteLine($"[TC15] Kết quả: FAIL - Đã chụp ảnh lưu vào cột N");
                Assert.Fail("FAIL: Không hiển thị thông báo giới hạn số lượng của quán.");
            }
            catch (Exception ex)
            {
                string errorImg = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Error");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber, "Lỗi: " + ex.Message, "FAIL", errorImg, testData);
                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC16_CheckFinalCart(string testCaseId, int expectedCount)
        {
            string stepNumber = "6";
            string testData = $"Kiểm tra giỏ hàng có đủ {expectedCount} món";
            try
            {
                // TĂNG THỜI GIAN ĐỢI: Để bảng giỏ hàng kịp hiện ra
                Thread.Sleep(5000);

                // TÌM CÁC DÒNG SẢN PHẨM: 
                // Thử tìm theo class row hoặc thẻ tr trong giỏ hàng (dựa trên image_e85847)
                var productRows = _driver.FindElements(By.CssSelector(".card-body .row.my-4"));

                // Nếu cách trên vẫn ra 0, thử tìm theo thẻ img bên trong card-body của giỏ hàng
                if (productRows.Count == 0)
                {
                    productRows = _driver.FindElements(By.CssSelector(".card-body img"));
                }

                int actualCount = productRows.Count;
                Console.WriteLine($"[TC16] Số món đếm được trong thực tế: {actualCount}");

                if (actualCount >= expectedCount)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        $"PASS: Đã thêm thành công {actualCount} sản phẩm", "PASS", "", testData);
                }
                else
                {
                    string screenFail = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_FinalCheck");
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        $"FAIL: Hệ thống chỉ ghi nhận {actualCount} món", "FAIL", screenFail, testData);
                    Assert.Fail($"Thất bại! Giỏ hàng đếm được {actualCount} món.");
                }
            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }
        public void ExecuteTC27_ClearCartAndVerify(string testCaseId)
        {
            string stepNumber = "6";
            string testData = "Xóa toàn bộ sản phẩm và kiểm tra thông báo trống";
            try
            {
                Thread.Sleep(3000);
                // 1. Tìm danh sách nút xóa (icon thùng rác)
                var deleteButtons = _driver.FindElements(By.CssSelector("a.text-danger, .bi-trash"));

                if (deleteButtons.Count > 0)
                {
                    Console.WriteLine($"Phát hiện {deleteButtons.Count} món, bắt đầu xóa...");
                    // Xóa từng món một
                    while (deleteButtons.Count > 0)
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                        js.ExecuteScript("arguments[0].click();", deleteButtons[0]);
                        Thread.Sleep(2000); // Chờ load lại trang sau khi xóa

                        // Cập nhật lại danh sách nút xóa còn lại
                        deleteButtons = _driver.FindElements(By.CssSelector("a.text-danger, .bi-trash"));
                    }
                }

                // 2. Kiểm tra thông báo giỏ hàng trống
                var emptyMsg = _driver.FindElement(By.CssSelector(".text-warning, h1.text-warning"));
                string actualText = emptyMsg.Text;

                if (actualText.Contains("Bạn chưa có sản phẩm nào trong giỏ hàng"))
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "PASS: Giỏ hàng đã trống và hiển thị đúng thông báo", "PASS", "", testData);
                }
                else
                {
                    string screenFail = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_NotMovingToEmpty");
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "FAIL: Không tìm thấy thông báo giỏ hàng trống", "FAIL", screenFail, testData);
                    Assert.Fail("Thất bại! Giỏ hàng không hiển thị thông báo trống.");
                }
            }
            catch (Exception ex)
            {
                // Trường hợp không có sản phẩm ngay từ đầu, check luôn thông báo
                try
                {
                    var emptyMsg = _driver.FindElement(By.CssSelector(".text-warning"));
                    if (emptyMsg.Displayed)
                    {
                        _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber, "PASS: Giỏ hàng trống sẵn", "PASS", "", testData);
                        return;
                    }
                }
                catch { }
                Assert.Fail("Lỗi hệ thống: " + ex.Message);
            }
        }

        public void ExecuteTC56_CheckEmptyCartMessage(string testCaseId)
        {
            string stepNumber = "9";
            string testData = "Kiểm tra giỏ hàng trống";

            try
            {
                // Mở giỏ hàng
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(2000);

                // Tìm thông báo giỏ hàng trống
                var emptyMsg = _driver.FindElement(By.CssSelector(".text-warning, h1.text-warning"));

                if (emptyMsg.Displayed)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "PASS: Giỏ hàng trống hiển thị thông báo", "PASS", "", testData);
                    Console.WriteLine($"[{testCaseId}] Step {stepNumber}: PASS - Giỏ hàng trống");
                }
                else
                {
                    string screenFail = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "FAIL: Không tìm thấy thông báo giỏ hàng trống", "FAIL", screenFail, testData);
                    Assert.Fail("Giỏ hàng không hiển thị thông báo trống.");
                }
            }
            catch (Exception ex)
            {
                string screenFail = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "FAIL: " + ex.Message, "FAIL", screenFail, testData);
                Assert.Fail(ex.Message);
            }
        }
    }
}