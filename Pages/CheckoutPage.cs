using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;

        // --- Locators dùng chung với Cart ---
        private readonly By ProductLink = By.XPath("//a[contains(@href, 'prId=')]");
        private readonly By AddToCartBtn = By.CssSelector("button.btn-success.btn-lg");
        private readonly By CartIconHeader = By.CssSelector("a.nav-icon .bi-cart-check-fill");
        private readonly By totalProductPrice = By.Id("totalMoneyText_0");
        private readonly By shippingCharge = By.Id("shipping_charge");
        private readonly By totalCheckoutPrice = By.Id("total_money");
        private readonly By DeleteButton = By.CssSelector("a[onclick*='DeleteProduct']");


        // --- Locators Form Thanh Toán (ID chuẩn từ ảnh Inspect bạn gửi) ---
        private readonly By txtFullName = By.Id("customer_name");
        private readonly By txtPhone = By.Id("customer_phone");
        private readonly By txtAddress = By.Id("customer_address");
        private readonly By btnOrder = By.CssSelector("button.btn-success[onclick='CreateOrder()']");
        private readonly By btnVNPay = By.CssSelector("button[onclick='PayWithVNPay()']");
        public CheckoutPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        public void ExecuteTC29_EmptyPhoneFlow(string testCaseId)
        {
            string stepNumber = "2";
            // Dữ liệu nhập cứng trực tiếp vào code theo yêu cầu của bạn
            string input_Name = "Nguyễn Văn A";
            string input_Add = "828 Sư Vạn Hạnh";
            string input_Phone = ""; // Để trống SĐT để test lỗi

            string testData = $"Nhập cứng -> Tên: {input_Name}, ĐC: {input_Add}, SĐT: '{input_Phone}'";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // 1. KÉO XUỐNG VÀ CHỌN SẢN PHẨM (Y chang CartPage)
                Thread.Sleep(2000);
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                // 2. THÊM VÀO GIỎ
                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // 3. VÀO GIỎ HÀNG (Dùng Icon trên Header)
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(3000);

                // 4. NHẬP DỮ LIỆU TỰ ĐỘNG
                var inputName = _driver.FindElement(txtFullName);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", inputName);
                inputName.Clear();
                inputName.SendKeys(input_Name);

                var inputAddress = _driver.FindElement(txtAddress);
                inputAddress.Clear();
                inputAddress.SendKeys(input_Add);

                var inputPhoneField = _driver.FindElement(txtPhone);
                inputPhoneField.Clear();
                inputPhoneField.SendKeys(input_Phone); // Máy tự nhập rỗng

                Thread.Sleep(2000);

                // 5. BẤM ĐẶT HÀNG
                var orderBtn = _driver.FindElement(btnOrder);
                js.ExecuteScript("arguments[0].click();", orderBtn);

                // 6. XỬ LÝ ALERT KẾT QUẢ
                Thread.Sleep(2000);
                IAlert alert = _driver.SwitchTo().Alert();
                string alertText = alert.Text;
                alert.Accept();

                // Ghi kết quả vào Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Thông báo hệ thống: " + alertText, "PASS", "", testData);

                Console.WriteLine($"[{testCaseId}] PASS: Máy đã tự nhập liệu xong.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber, ex.Message, "FAIL", screen, testData);
                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC30_InvalidPhone_OnlyStep2(string testCaseId)
        {
            string stepNumber = "2";

            string input_Name = "Nguyễn Văn A";
            string input_Add = "828 Sư Vạn Hạnh";
            string input_Phone = "1234"; // SĐT sai
            string testData = $"SĐT: {input_Phone}";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // ===== Vào sản phẩm + thêm giỏ =====
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", product);
                js.ExecuteScript("arguments[0].click();", product);
                Thread.Sleep(500);

                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(500);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // ===== Click giỏ hàng =====
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(500);

                // ===== Điền form (Step 1 & 3) =====
                _driver.FindElement(txtFullName).SendKeys(input_Name);
                _driver.FindElement(txtAddress).SendKeys(input_Add);

                var inputPhoneField = _driver.FindElement(txtPhone);
                inputPhoneField.Clear();
                inputPhoneField.SendKeys(input_Phone);
                Thread.Sleep(500);

                // ===== Click đặt hàng =====
                var orderBtn = _driver.FindElement(btnOrder);
                js.ExecuteScript("arguments[0].click();", orderBtn);
                Thread.Sleep(500);

                // ===== Bấm OK confirm alert =====
                try
                {
                    IAlert confirmAlert = _driver.SwitchTo().Alert();
                    confirmAlert.Accept();
                    Thread.Sleep(500);
                }
                catch { }

                // ===== Chờ alert "Đặt hàng thành công!" =====
                IAlert successAlert = null;
                int retry = 0;
                while (retry < 5)
                {
                    try
                    {
                        successAlert = _driver.SwitchTo().Alert();
                        break;
                    }
                    catch { Thread.Sleep(500); retry++; }
                }

                string alertText = successAlert?.Text ?? "Không có alert";

                // ===== Screenshot =====
                string screenshotPath = @"D:\ScreenShotFail\" + testCaseId + "_Fail.png";
                ScreenshotHelper.TakeScreenshot(_driver, screenshotPath);

                // ===== Ghi Excel Step 2 =====
                string actualResult = $"Đặt hàng vẫn được -> {alertText}";
                string status = "FAIL"; // Step 2 sai SĐT -> FAIL

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    actualResult, status, screenshotPath, testData);

                Console.WriteLine($"[{testCaseId}] Step 2: {status} - {actualResult}");

                // ===== Accept alert cuối =====
                successAlert?.Accept();

                // ===== Ném exception để NUnit báo FAIL/X =====
                Assert.Fail("Step 2: SĐT sai nhưng hệ thống vẫn đặt hàng thành công");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);

                Assert.Fail(ex.Message);
            }
        }
        public void ExecuteTC31_EmptyNameFlow(string testCaseId)
        {
            string stepNumber = "1";

            // Dữ liệu nhập cứng
            string input_Name = ""; // Bỏ trống tên để test lỗi
            string input_Add = "828 Sư Vạn Hạnh";
            string input_Phone = "0987654321"; // SĐT nhập đại

            string testData = $"Nhập cứng -> Tên: '{input_Name}', ĐC: {input_Add}, SĐT: {input_Phone}";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // 1. KÉO XUỐNG VÀ CHỌN SẢN PHẨM
                Thread.Sleep(2000);
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                // 2. THÊM VÀO GIỎ
                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // 3. VÀO GIỎ HÀNG
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(3000);

                // 4. NHẬP DỮ LIỆU
                var inputName = _driver.FindElement(txtFullName);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", inputName);
                inputName.Clear();
                inputName.SendKeys(input_Name); // để trống

                var inputAddress = _driver.FindElement(txtAddress);
                inputAddress.Clear();
                inputAddress.SendKeys(input_Add);

                var inputPhoneField = _driver.FindElement(txtPhone);
                inputPhoneField.Clear();
                inputPhoneField.SendKeys(input_Phone);

                Thread.Sleep(2000);

                // 5. BẤM ĐẶT HÀNG
                var orderBtn = _driver.FindElement(btnOrder);
                js.ExecuteScript("arguments[0].click();", orderBtn);

                // 6. XỬ LÝ ALERT
                Thread.Sleep(2000);
                IAlert alert = _driver.SwitchTo().Alert();
                string alertText = alert.Text;
                alert.Accept();

                // Ghi Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Thông báo hệ thống: " + alertText, "PASS", "", testData);

                Console.WriteLine($"[{testCaseId}] PASS: Test bỏ trống tên thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);

                Assert.Fail(ex.Message);
            }

        }





        public void ExecuteTC40_EmptyAddressFlow(string testCaseId)
        {
            string stepNumber = "3";

            // Dữ liệu nhập cứng
            string input_Name = "Nguyễn Văn A";
            string input_Add = ""; // Bỏ trống địa chỉ
            string input_Phone = "0987654321";

            string testData = $"Nhập cứng -> Tên: {input_Name}, ĐC: '{input_Add}', SĐT: {input_Phone}";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // 1. KÉO XUỐNG VÀ CHỌN SẢN PHẨM
                Thread.Sleep(2000);
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                // 2. THÊM VÀO GIỎ
                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // 3. VÀO GIỎ HÀNG
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(3000);

                // 4. NHẬP DỮ LIỆU
                var inputName = _driver.FindElement(txtFullName);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", inputName);
                inputName.Clear();
                inputName.SendKeys(input_Name);

                var inputAddress = _driver.FindElement(txtAddress);
                inputAddress.Clear();
                inputAddress.SendKeys(input_Add); // để trống

                var inputPhoneField = _driver.FindElement(txtPhone);
                inputPhoneField.Clear();
                inputPhoneField.SendKeys(input_Phone);

                Thread.Sleep(2000);

                // 5. BẤM ĐẶT HÀNG
                var orderBtn = _driver.FindElement(btnOrder);
                js.ExecuteScript("arguments[0].click();", orderBtn);

                // 6. XỬ LÝ ALERT
                Thread.Sleep(2000);
                IAlert alert = _driver.SwitchTo().Alert();
                string alertText = alert.Text;
                alert.Accept();

                // Ghi Excel
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Thông báo hệ thống: " + alertText, "PASS", "", testData);

                Console.WriteLine($"[{testCaseId}] PASS: Test bỏ trống địa chỉ thành công.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);

                Assert.Fail(ex.Message);
            }
        }





        public void ExecuteTC45_FullValidFlow(string testCaseId)
        {
            string input_Name = "Nguyễn Văn A";
            string input_Phone = "0987654321";
            string input_Add = "828 Sư Vạn Hạnh";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // Setup: chọn SP + thêm giỏ
                Thread.Sleep(2000);
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // ===== STEP 4: CLICK GIỎ HÀNG =====
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(3000);

                _excelProvider.UpdateTestResult(_config, testCaseId, "4",
                    "Đã click vào giỏ hàng", "PASS", "", "Click giỏ hàng");

                // ===== STEP 1: TÊN =====
                var inputName = _driver.FindElement(txtFullName);
                inputName.Clear();
                inputName.SendKeys(input_Name);

                _excelProvider.UpdateTestResult(_config, testCaseId, "1",
                    "Nhập tên thành công", "PASS", "", $"Tên: {input_Name}");

                // ===== STEP 2: SĐT =====
                var inputPhone = _driver.FindElement(txtPhone);
                inputPhone.Clear();
                inputPhone.SendKeys(input_Phone);

                _excelProvider.UpdateTestResult(_config, testCaseId, "2",
                    "Nhập SĐT thành công", "PASS", "", $"SĐT: {input_Phone}");

                // ===== STEP 3: ĐỊA CHỈ =====
                var inputAddress = _driver.FindElement(txtAddress);
                inputAddress.Clear();
                inputAddress.SendKeys(input_Add);

                _excelProvider.UpdateTestResult(_config, testCaseId, "3",
                    "Nhập địa chỉ thành công", "PASS", "", $"ĐC: {input_Add}");

                Thread.Sleep(2000);

                // Click đặt hàng
                var orderBtn = _driver.FindElement(btnOrder);
                js.ExecuteScript("arguments[0].click();", orderBtn);

                Thread.Sleep(2000);
                var alert = _driver.SwitchTo().Alert();
                alert.Accept();

                Console.WriteLine($"[{testCaseId}] PASS toàn bộ step.");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");

                _excelProvider.UpdateTestResult(_config, testCaseId, "ERROR",
                    ex.Message, "FAIL", screen, "");

                Assert.Fail(ex.Message);
            }
        }




        public void ExecuteTC48_VNPayFlow(string testCaseId)
        {
            string input_Name = "Nguyễn Văn A";
            string input_Phone = "0987654321";
            string input_Add = "828 Sư Vạn Hạnh";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // Setup: chọn SP + thêm giỏ
                Thread.Sleep(2000);
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // ===== STEP 4: CLICK GIỎ HÀNG =====
                js.ExecuteScript("window.scrollTo(0, 0);");
                Thread.Sleep(1000);
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(3000);

               
                // ===== STEP 1: NHẬP TÊN =====
                var inputName = _driver.FindElement(txtFullName);
                inputName.Clear();
                inputName.SendKeys(input_Name);

               

                // ===== STEP 2: NHẬP SĐT =====
                var inputPhone = _driver.FindElement(txtPhone);
                inputPhone.Clear();
                inputPhone.SendKeys(input_Phone);

                

                // ===== STEP 3: NHẬP ĐỊA CHỈ =====
                var inputAddress = _driver.FindElement(txtAddress);
                inputAddress.Clear();
                inputAddress.SendKeys(input_Add);

               

                Thread.Sleep(2000);

                // ===== STEP 6: CLICK THANH TOÁN VNPay =====
                var payBtn = _driver.FindElement(btnVNPay);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", payBtn);
                Thread.Sleep(300);
                js.ExecuteScript("arguments[0].click();", payBtn);
                Thread.Sleep(500);

                // ===== Xử lý alert =====
                IAlert payAlert = _driver.SwitchTo().Alert();
                string alertText = payAlert.Text;
                payAlert.Accept();

                // ===== Ghi Excel Step 6: FAIL =====
                _excelProvider.UpdateTestResult(_config, testCaseId, "6",
                    "Không thanh toán được -> " + alertText, "FAIL",
                    ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail"),
                    "Click Thanh toán VNPay");

                // ===== Ném exception để NUnit báo FAIL =====
                Assert.Fail("Step 6: Thanh toán VNPay không thành công");

            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");

                _excelProvider.UpdateTestResult(_config, testCaseId, "ERROR",
                    ex.Message, "FAIL", screen, "");

                Assert.Fail(ex.Message);
            }
        }

        public void ExecuteTC47_CheckTotalMoney(string testCaseId)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // ===== Setup: chọn SP + thêm giỏ =====
                Thread.Sleep(2000);
                var product = _driver.FindElement(ProductLink);
                js.ExecuteScript("arguments[0].scrollIntoView(true);", product);
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].click();", product);

                Thread.Sleep(3000);
                _driver.FindElement(AddToCartBtn).Click();
                Thread.Sleep(2000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                // ===== Step 1: Click giỏ hàng =====
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(3000);
                _excelProvider.UpdateTestResult(_config, testCaseId, "1",
                    "Click giỏ hàng thành công", "PASS", "", "Click giỏ hàng");

                // ===== Step 2: Lấy và xử lý giá tiền =====
                string productPriceText = _driver.FindElement(totalProductPrice).Text;
                string shippingChargeText = _driver.FindElement(shippingCharge).Text;
                string checkoutPriceText = _driver.FindElement(totalCheckoutPrice).Text;

                decimal productPrice = Decimal.Parse(System.Text.RegularExpressions.Regex.Replace(productPriceText, @"[^\d]", ""));
                decimal shippingPrice = Decimal.Parse(System.Text.RegularExpressions.Regex.Replace(shippingChargeText, @"[^\d]", ""));
                decimal checkoutPrice = Decimal.Parse(System.Text.RegularExpressions.Regex.Replace(checkoutPriceText, @"[^\d]", ""));

                string testData = $"Giá SP: {productPriceText}, Phí vận chuyển: {shippingChargeText}, Tổng tiền checkout: {checkoutPriceText}";

                if (productPrice + shippingPrice == checkoutPrice)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, "2",
                        "Tổng tiền + phí vận chuyển khớp với tổng tiền checkout", "PASS", "", testData);
                    Console.WriteLine($"[{testCaseId}] PASS: Tổng tiền đúng");
                }
                else
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, "2",
                        "Tổng tiền + phí vận chuyển KHÔNG khớp với tổng tiền checkout", "FAIL",
                        ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail"), testData);
                    Assert.Fail("Tổng tiền + phí vận chuyển không khớp với tổng tiền checkout");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");

                _excelProvider.UpdateTestResult(_config, testCaseId, "ERROR",
                    ex.Message, "FAIL", screen, "");

                Assert.Fail(ex.Message);
            }
        }

        public void ExecuteTC51_DeleteAndCheckTotal(string testCaseId)
        {
            string stepDelete = "7";
            string stepCheckTotal = "5";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // ===== Mở giỏ hàng =====
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(2000);

                // Xử lý overlay
                js.ExecuteScript("document.querySelectorAll('.modal-backdrop, .spinner').forEach(el => el.remove());");

                // ===== Lấy giá tiền trước khi xóa =====
                string totalBeforeText = _driver.FindElement(By.Id("total_money")).Text;
                decimal totalBefore = Decimal.Parse(System.Text.RegularExpressions.Regex.Replace(totalBeforeText, @"[^\d]", ""));

                // ===== Step 7: Click xóa sản phẩm =====
                var deleteBtns = _driver.FindElements(DeleteButton);
                if (deleteBtns.Count == 0) throw new Exception("Không tìm thấy nút xóa");

                js.ExecuteScript("arguments[0].scrollIntoView(true);", deleteBtns[0]);
                js.ExecuteScript("arguments[0].click();", deleteBtns[0]);

                Thread.Sleep(1000);
                try { _driver.SwitchTo().Alert().Accept(); } catch { }

                Thread.Sleep(2000);
                _excelProvider.UpdateTestResult(_config, testCaseId, stepDelete,
                    "Xóa 1 sản phẩm thành công", "PASS", "", "Click xóa");
                Console.WriteLine(">>> STEP 7 PASS: Xóa 1 sản phẩm");

                // ===== Step 5: Kiểm tra tổng tiền =====
                string totalAfterText = _driver.FindElement(By.Id("total_money")).Text;
                decimal totalAfter = Decimal.Parse(System.Text.RegularExpressions.Regex.Replace(totalAfterText, @"[^\d]", ""));

                if (totalAfter < totalBefore)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepCheckTotal,
                        $"Tổng tiền giảm đúng: trước {totalBeforeText}, sau {totalAfterText}", "PASS", "", "");
                    Console.WriteLine(">>> STEP 5 PASS: Tổng tiền giảm đúng");
                }
                else
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepCheckTotal,
                        $"Tổng tiền KHÔNG giảm: trước {totalBeforeText}, sau {totalAfterText}", "FAIL",
                        ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail"), "");
                    Assert.Fail("Tổng tiền không giảm sau khi xóa sản phẩm");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepDelete,
                    ex.Message, "FAIL", screen, "Xóa/Check total");
                Assert.Fail(ex.Message);
            }
        }


        public void ExecuteTC53_EmptyCartCheckTotal(string testCaseId)
        {
            string stepDelete = "7";
            string stepCheckTotal = "5";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // ===== Mở giỏ hàng =====
                _driver.FindElement(CartIconHeader).Click();
                Thread.Sleep(2000);

                // Xử lý overlay
                js.ExecuteScript("document.querySelectorAll('.modal-backdrop, .spinner').forEach(el => el.remove());");

                // ===== Step 7: Xóa tất cả sản phẩm =====
                var deleteBtns = _driver.FindElements(DeleteButton);
                if (deleteBtns.Count == 0)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepDelete,
                        "Giỏ hàng đã trống", "PASS", "", "Không còn sản phẩm");
                    Console.WriteLine(">>> STEP 7 PASS: Giỏ hàng đã trống");
                }
                else
                {
                    foreach (var btn in deleteBtns)
                    {
                        js.ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                        js.ExecuteScript("arguments[0].click();", btn);
                        Thread.Sleep(500);
                        try { _driver.SwitchTo().Alert().Accept(); } catch { }
                        Thread.Sleep(500);
                    }

                    _excelProvider.UpdateTestResult(_config, testCaseId, stepDelete,
                        "Xóa tất cả sản phẩm thành công", "PASS", "", "Click xóa tất cả");
                    Console.WriteLine(">>> STEP 7 PASS: Xóa tất cả sản phẩm");
                }

                Thread.Sleep(2000);

                // ===== Step 5: Kiểm tra tổng tiền =====
                string totalAfterText = _driver.FindElement(By.Id("total_money")).Text;
                decimal totalAfter = Decimal.Parse(System.Text.RegularExpressions.Regex.Replace(totalAfterText, @"[^\d]", ""));

                if (totalAfter == 0)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepCheckTotal,
                        $"Tổng tiền = 0 như mong đợi: {totalAfterText}", "PASS", "", "");
                    Console.WriteLine(">>> STEP 5 PASS: Tổng tiền = 0");
                }
                else
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepCheckTotal,
                        $"Tổng tiền KHÔNG = 0: {totalAfterText}", "FAIL",
                        ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail"), "");
                    Assert.Fail("Tổng tiền vẫn còn sau khi xóa tất cả sản phẩm");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepDelete,
                    ex.Message, "FAIL", screen, "Xóa tất cả/Check total");
                Assert.Fail(ex.Message);
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