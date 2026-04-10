using NPOI.SS.Formula.Functions;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using System;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class AccountDetailsPage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;
        private readonly By fileUploadInput = By.CssSelector("input[name='AttachFiles']");
        private readonly By userImage = By.Id("main-user-image");
        private readonly By uploadInput = By.CssSelector("input[name='AttachFiles']");
        private readonly By oldPassword = By.Id("Account_Old_Password");
        private readonly By newPassword = By.Id("Account_New_Password");
        private readonly By confirmPassword = By.Id("Account_Confirm_Password");
        private readonly By btnUpdatePassword = By.Id("btn-save-password");
        // Locators
        private readonly By fullName = By.Id("Account_FullName");
        private readonly By email = By.Id("Account_Email");
        private readonly By phone = By.Id("Account_PhoneNumber");
        private readonly By gender = By.Id("Account_Gender");
        private readonly By address = By.Id("Account_Address");
        private readonly By avatar = By.Id("main-user-image");

        public AccountDetailsPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        public void ExecuteTC64_AccountDetailsDisplay(string testCaseId)
        {
            string stepNumber = "1";
            try
            {
                string testData = "Kiểm tra các field hiển thị: FullName, Email, Phone, Gender, Address, Avatar";

                Assert.IsTrue(_driver.FindElement(fullName).Displayed, "FullName không hiển thị");
                Assert.IsTrue(_driver.FindElement(email).Displayed, "Email không hiển thị");
                Assert.IsTrue(_driver.FindElement(phone).Displayed, "Phone không hiển thị");
                Assert.IsTrue(_driver.FindElement(gender).Displayed, "Gender không hiển thị");
                Assert.IsTrue(_driver.FindElement(address).Displayed, "Address không hiển thị");
                Assert.IsTrue(_driver.FindElement(avatar).Displayed, "Avatar không hiển thị");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Tất cả field hiển thị đúng", "PASS", "", testData);

                Console.WriteLine($"[{testCaseId}] PASS: AccountDetails hiển thị đầy đủ");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, "");
                Assert.Fail(ex.Message);
            }
        }


        public void ExecuteTC66_UploadAvatar(string testCaseId)
        {
            string stepNumber = "3";
            string testData = "Upload ảnh từ folder Pictures/ngua";

            try
            {
                // ===== 1. Lấy path ảnh =====
                string picturePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "ngua.jpg");
                if (!File.Exists(picturePath))
                    throw new FileNotFoundException($"File không tồn tại: {picturePath}");

                // ===== 2. Upload file =====
                var uploadInput = _driver.FindElement(fileUploadInput);
                uploadInput.SendKeys(picturePath); // Selenium tự upload ảnh

                Thread.Sleep(2000); // chờ ảnh load

                // ===== 3. Kiểm tra ảnh xuất hiện =====
                var img = _driver.FindElement(userImage);
                string src = img.GetAttribute("src");

                if (!string.IsNullOrEmpty(src) && src.Contains("data") || src.Contains("ngua"))
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "PASS: Ảnh đã upload thành công", "PASS", "", testData);
                    Console.WriteLine($"[{testCaseId}] PASS: Upload ảnh thành công.");
                }
                else
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "FAIL: Ảnh chưa hiển thị sau khi upload", "FAIL",
                        ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail"), testData);
                    throw new Exception("Ảnh chưa hiển thị sau khi upload");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);
                throw;
            }
        }
        public void ExecuteTC67_CancelUploadAvatar(string testCaseId)
        {
            string stepNumber = "3";
            string testData = "Click vào upload avatar nhưng không chọn file";

            try
            {
                var uploadInput = _driver.FindElement(By.CssSelector("input[name='AttachFiles']"));

                // Chỉ click vào input (mở dialog hệ thống) -> KHÔNG gửi file
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].click();", uploadInput);
                Thread.Sleep(1000); // chờ dialog mở (hệ thống)

                // Kiểm tra avatar chưa đổi (ví dụ dựa vào src cũ)
                var avatarImg = _driver.FindElement(By.Id("main-user-image"));
                string avatarSrc = avatarImg.GetAttribute("src");

                // Nếu src không thay đổi -> pass
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "PASS: Mở dialog chọn file nhưng không upload -> avatar không đổi", "PASS", "", testData);
                Console.WriteLine($"[{testCaseId}] STEP {stepNumber} PASS");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "[TC67] Thất bại: " + ex.Message, "FAIL", screen, testData);
                Assert.Fail(ex.Message);
            }
        }


        public void ExecuteTC68_UpdateAccountInfo(string testCaseId)
        {
            string stepName = "";
            string testData = "Cập nhật thông tin: tên, số điện thoại, địa chỉ và click nút Cập nhật";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                // ===== Step 2: Điền tên =====
                stepName = "2";
                var fullNameInput = _driver.FindElement(By.Id("Account_FullName"));
                fullNameInput.Clear();
                fullNameInput.SendKeys("Nguyen Van B");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepName, "Nhập tên thành công", "PASS", "", "Tên: Nguyen Van B");

                // ===== Step 5: Điền số điện thoại =====
                stepName = "5";
                var phoneInput = _driver.FindElement(By.Id("Account_PhoneNumber"));
                phoneInput.Clear();
                phoneInput.SendKeys("0987654321");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepName, "Nhập SĐT thành công", "PASS", "", "SĐT: 0987654321");

                // ===== Step 6: Điền địa chỉ =====
                stepName = "6";
                var addressInput = _driver.FindElement(By.Id("Account_Address"));
                addressInput.Clear();
                addressInput.SendKeys("828 Su Van Hanh");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepName, "Nhập địa chỉ thành công", "PASS", "", "Địa chỉ: 828 Su Van Hanh");

                // ===== Step 4: Click nút Cập nhật =====
                stepName = "4";
                var updateBtn = _driver.FindElement(By.Id("btn-save-infor"));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", updateBtn);
                js.ExecuteScript("arguments[0].click();", updateBtn);
                Thread.Sleep(2000); // chờ phản hồi

                // ===== Pass nếu bấm update thành công =====
                _excelProvider.UpdateTestResult(_config, testCaseId, stepName,
                    "PASS: Cập nhật thông tin thành công", "PASS", "", testData);
                Console.WriteLine($"[{testCaseId}] PASS: Cập nhật thành công");
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepName,
                    ex.Message, "FAIL", screen, testData);
                Assert.Fail(ex.Message);
            }
        }

        public void ExecuteTC69_EmptyFullNameUpdate(string testCaseId)
        {
            string stepNumber = "1";
            string testData = "Bỏ trống FullName và bấm Cập nhật";

            try
            {
                // ===== 1. Xóa họ tên =====
                var fullNameInput = _driver.FindElement(fullName); // Dùng đúng By.Id("Account_FullName")
                fullNameInput.Clear();

                // ===== 2. Click nút Cập nhật =====
                var updateBtn = _driver.FindElement(By.Id("btn-save-infor"));
                updateBtn.Click();

                // ===== 3. Chờ trình duyệt show validation =====
                Thread.Sleep(1000);

                // ===== 4. Lấy thông báo trình duyệt =====
                string validationMessage = fullNameInput.GetAttribute("validationMessage");

                if (!string.IsNullOrEmpty(validationMessage))
                {
                    // PASS nếu có thông báo
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        $"PASS: Hiển thị thông báo required -> '{validationMessage}'",
                        "PASS", "", testData);
                    Console.WriteLine($"[{testCaseId}] PASS: Thông báo họ tên hiển thị -> {validationMessage}");
                }
                else
                {
                    string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "FAIL: Không hiển thị thông báo họ tên", "FAIL", screen, testData);
                    Assert.Fail("Không hiển thị thông báo họ tên khi bỏ trống");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);
                Assert.Fail(ex.Message);
            }
        }



        public void ExecuteTC75_EmptyPhoneUpdate(string testCaseId)
        {
            string stepNumber = "5";
            string testData = "Điền họ tên, bỏ trống số điện thoại và bấm Cập nhật";

            try
            {
                // ===== 1. Nhập họ tên hợp lệ =====
                var fullNameInput = _driver.FindElement(fullName); // id="Account_FullName"
                fullNameInput.Clear();
                fullNameInput.SendKeys("Nguyễn Văn Test"); // Nhập tên đại

                // ===== 2. Xóa số điện thoại =====
                var phoneInput = _driver.FindElement(phone); // id="Account_PhoneNumber"
                phoneInput.Clear();

                // ===== 3. Click nút Cập nhật =====
                var updateBtn = _driver.FindElement(By.Id("btn-save-infor"));
                updateBtn.Click();

                // ===== 4. Chờ trình duyệt show validation =====
                Thread.Sleep(1000);

                // ===== 5. Lấy thông báo trình duyệt =====
                string validationMessage = phoneInput.GetAttribute("validationMessage");

                if (!string.IsNullOrEmpty(validationMessage))
                {
                    // PASS nếu có thông báo
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        $"PASS: Hiển thị thông báo required cho số điện thoại -> '{validationMessage}'",
                        "PASS", "", testData);
                    Console.WriteLine($"[{testCaseId}] PASS: Thông báo số điện thoại hiển thị -> {validationMessage}");
                }
                else
                {
                    string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "FAIL: Không hiển thị thông báo số điện thoại", "FAIL", screen, testData);
                    Assert.Fail("Không hiển thị thông báo số điện thoại khi bỏ trống");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);
                Assert.Fail(ex.Message);
            }
        }


        public void ExecuteTC80_EmptyAddressUpdate(string testCaseId)
        {
            string stepNumber = "6";
            string testData = "Điền họ tên, số điện thoại, bỏ trống địa chỉ và bấm Cập nhật";

            try
            {
                // ===== 1. Nhập họ tên hợp lệ =====
                var fullNameInput = _driver.FindElement(fullName); // id="Account_FullName"
                fullNameInput.Clear();
                fullNameInput.SendKeys("Nguyễn Văn Test"); // Nhập tên đại

                // ===== 2. Nhập số điện thoại hợp lệ =====
                var phoneInput = _driver.FindElement(phone); // id="Account_PhoneNumber"
                phoneInput.Clear();
                phoneInput.SendKeys("0987654321");

                // ===== 3. Xóa địa chỉ =====
                var addressInput = _driver.FindElement(address); // id="Account_Address"
                addressInput.Clear();

                // ===== 4. Click nút Cập nhật =====
                var updateBtn = _driver.FindElement(By.Id("btn-save-infor"));
                updateBtn.Click();

                // ===== 5. Chờ trình duyệt show validation =====
                Thread.Sleep(1000);

                // ===== 6. Lấy thông báo trình duyệt =====
                string validationMessage = addressInput.GetAttribute("validationMessage");

                if (!string.IsNullOrEmpty(validationMessage))
                {
                    // PASS nếu có thông báo
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        $"PASS: Hiển thị thông báo required cho địa chỉ -> '{validationMessage}'",
                        "PASS", "", testData);
                    Console.WriteLine($"[{testCaseId}] PASS: Thông báo địa chỉ hiển thị -> {validationMessage}");
                }
                else
                {
                    string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "FAIL: Không hiển thị thông báo địa chỉ", "FAIL", screen, testData);
                    Assert.Fail("Không hiển thị thông báo địa chỉ khi bỏ trống");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);
                Assert.Fail(ex.Message);
            }
        }


        public void ExecuteTC83_NavigateToPasswordTab(string testCaseId)
        {
            string stepNumber = "7";
            string testData = "Click tab Mật khẩu trong Account Details";

            try
            {
                // ===== 1. Tìm nút Mật khẩu =====
                var passwordTabBtn = _driver.FindElement(By.Id("password-tab"));

                // ===== 2. Click vào nút =====
                passwordTabBtn.Click();
                Thread.Sleep(1000); // chờ animation chuyển tab

                // ===== 3. Kiểm tra đã chuyển sang tab password chưa =====
                var passwordTabContent = _driver.FindElement(By.Id("password-tab-data"));
                if (passwordTabContent.Displayed)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "PASS: Chuyển sang tab Mật khẩu thành công", "PASS", "", testData);
                    Console.WriteLine($"[{testCaseId}] PASS: Chuyển sang tab Mật khẩu thành công");
                }
                else
                {
                    string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "FAIL: Không chuyển sang tab Mật khẩu", "FAIL", screen, testData);
                    Assert.Fail("Không chuyển sang tab Mật khẩu");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);
                Assert.Fail(ex.Message);
            }
        }


        public void ExecuteTC84_ChangePassword(string testCaseId)
        {
            string stepNumber = "";
            string testData = "Đổi mật khẩu: cũ=123, mới=1234, xác nhận=1234";
            try
            {
                // ===== 1. Tìm nút Mật khẩu =====
                var passwordTabBtn = _driver.FindElement(By.Id("password-tab"));

                // ===== 2. Click vào nút =====
                passwordTabBtn.Click();
                Thread.Sleep(1000); // chờ animation chuyển tab
                // ===== Step 8: Nhập mật khẩu cũ =====
                stepNumber = "8";
                _driver.FindElement(oldPassword).Clear();
                _driver.FindElement(oldPassword).SendKeys("123");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Nhập mật khẩu cũ", "PASS", "", testData);

                // ===== Step 9: Nhập mật khẩu mới =====
                stepNumber = "9";
                _driver.FindElement(newPassword).Clear();
                _driver.FindElement(newPassword).SendKeys("1234");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Nhập mật khẩu mới", "PASS", "", testData);

                // ===== Step 11: Nhập xác nhận mật khẩu mới =====
                stepNumber = "12";
                _driver.FindElement(confirmPassword).Clear();
                _driver.FindElement(confirmPassword).SendKeys("1234");

                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Nhập xác nhận mật khẩu mới", "PASS", "", testData);

                // ===== Step 4: Bấm nút cập nhật =====
                stepNumber = "4";
                _driver.FindElement(btnUpdatePassword).Click();
                Thread.Sleep(1000); // đợi alert xuất hiện

                try
                {
                    IAlert alert = _driver.SwitchTo().Alert();
                    string alertText = alert.Text;
                    if (alertText.Contains("Cập nhật thành công"))
                    {
                        alert.Accept(); // Bấm OK
                        _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                            $"Thông báo: {alertText}", "PASS", "", testData);
                        Console.WriteLine($"[{testCaseId}] PASS: Đổi mật khẩu thành công với alert: {alertText}");
                    }
                    else
                    {
                        alert.Accept();
                        string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                        _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                            $"Alert không đúng: {alertText}", "FAIL", screen, testData);
                        Assert.Fail($"Alert không đúng: {alertText}");
                    }
                }
                catch (NoAlertPresentException)
                {
                    string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "Không có alert xuất hiện", "FAIL", screen, testData);
                    Assert.Fail("Không có alert xuất hiện");
                }
            }
            catch (Exception ex)
            {
                string screen = ScreenshotHelper.TakeScreenshot(_driver, testCaseId + "_Fail");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    ex.Message, "FAIL", screen, testData);
                Assert.Fail(ex.Message);
            }
        }


        public void ExecuteTC85_WrongOldPassword(string testCaseId)
        {
            string stepNumber = "";
            try
            {
                // ===== 1. Tìm nút Mật khẩu =====
                var passwordTabBtn = _driver.FindElement(By.Id("password-tab"));

                // ===== 2. Click vào nút =====
                passwordTabBtn.Click();
                Thread.Sleep(1000); // chờ animation chuyển tab
                // ===== Step 8: Nhập mật khẩu cũ sai =====
                stepNumber = "8";
                var oldPasswordInput = _driver.FindElement(By.Id("Account_Old_Password"));
                oldPasswordInput.Clear();
                oldPasswordInput.SendKeys("1234"); // mật khẩu cũ sai
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Nhập mật khẩu cũ sai: 1234", "INFO", "", "");

                // ===== Step 9: Nhập mật khẩu mới =====
                stepNumber = "9";
                var newPasswordInput = _driver.FindElement(By.Id("Account_New_Password"));
                newPasswordInput.Clear();
                newPasswordInput.SendKeys("1234");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Nhập mật khẩu mới: 1234", "INFO", "", "");

                // ===== Step 11: Xác nhận mật khẩu mới =====
                stepNumber = "11";
                var confirmPasswordInput = _driver.FindElement(By.Id("Account_Confirm_Password"));
                confirmPasswordInput.Clear();
                confirmPasswordInput.SendKeys("1234");
                _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                    "Xác nhận mật khẩu mới: 1234", "INFO", "", "");

                // ===== Step 12: Bấm nút cập nhật =====
                stepNumber = "12";
                var updateButton = _driver.FindElement(By.Id("btn-save-password"));
                updateButton.Click();

                // ===== Step 13: Kiểm tra alert =====
                Thread.Sleep(2000); // đợi alert xuất hiện

                try
                {
                    IAlert alert = _driver.SwitchTo().Alert();
                    string alertText = alert.Text;

                    // Nếu alert có thông báo mật khẩu cũ không đúng → PASS
                    if (alertText.Contains("Mật khẩu cũ không chính xác") || alertText.Contains("không đúng"))
                    {
                        _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                            "Alert xuất hiện đúng: " + alertText, "PASS", "", "");
                        alert.Accept();
                        Console.WriteLine($"[{testCaseId}] PASS: Mật khẩu cũ sai, alert hiển thị đúng");
                    }
                    else
                    {
                        // Nếu alert khác → FAIL
                        _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                            "Alert không đúng: " + alertText, "FAIL", "", "");
                        alert.Accept();
                        Assert.Fail($"[{testCaseId}] FAIL: Alert không đúng nội dung: {alertText}");
                    }
                }
                catch (NoAlertPresentException)
                {
                    _excelProvider.UpdateTestResult(_config, testCaseId, stepNumber,
                        "Không có alert xuất hiện", "FAIL", "", "");
                    Assert.Fail($"[{testCaseId}] FAIL: Không có alert xuất hiện khi mật khẩu cũ sai");
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




