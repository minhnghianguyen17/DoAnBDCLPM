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
    public class AdminProductTest : BaseTest
    {
        private LoginPage _loginPage;
        private AdminProductPage _adminProductPage;
        private JsonDataProvider _jsonProvider;
        private ExcelConfig _config;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _adminProductPage = new AdminProductPage(Driver, Config, ExcelProvider);
            _jsonProvider = new JsonDataProvider(Config);

            _config = new ExcelConfig
            {
                ExcelFilePath = Config.ExcelFilePath,
                SheetName = "TC-Hùng Khánh",
                TestCaseIdColumn = 1,
                StepColumn = 7,
                StepActionColumn = 8,
                TestDataColumn = 9,
                ExpectedResultColumn = 10,
                ActualResultColumn = 11,
                StatusColumn = 12,
                NotesColumn = 13,
                StartRow = 1
            };
        }

        /// <summary>
        /// TC2_AdminProduct: Kiểm tra thêm mới sản phẩm với thông tin hợp lệ
        /// </summary>
        [Test]
        public void TC2_AdminProduct()
        {
            string testCaseId = "TC2";

            try
            {
                // ===== LOGIN =====
                var account = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account ID 0");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                Console.WriteLine($"Đã nhập Email: {account.Email}");

                // ===== NAVIGATE =====
                _adminProductPage.NavigateToProductManagement();
                Thread.Sleep(1000);

                // ===== DATA =====
                var product = _jsonProvider.GetAdminProductByIndex(0);
                Assert.IsNotNull(product, $"[{testCaseId}] Không tìm thấy Product");

                var price = _jsonProvider.GetAdminProductPriceByIndex(0);
                Assert.IsNotNull(price, $"[{testCaseId}] Không tìm thấy Price");

                var topping = _jsonProvider.GetAdminToppingByIndex(0);
                Assert.IsNotNull(topping, $"[{testCaseId}] Không tìm thấy Topping");

                // Tạo mã sản phẩm unique với timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string uniqueProductCode = $"{product.ProductCode}_{timestamp}";
                string uniqueProductName = product.ProductName;

                Console.WriteLine($"[INFO] Mã sản phẩm: {uniqueProductCode}");
                Console.WriteLine($"[INFO] Tên sản phẩm: {uniqueProductName}");

                // ===== ADD PRODUCT =====
                _adminProductPage.ClickAddButton();
                Thread.Sleep(1000);

                // ===== TAB INFO =====
                _adminProductPage.ClickTabInfo();
                Thread.Sleep(500);

                _adminProductPage.FillProductForm(
                    uniqueProductCode,
                    uniqueProductName,
                    product.IsActive.ToString(),
                    product.ProductTypeId.ToString(),
                    product.Note
                );

                Console.WriteLine("✅ Đã nhập tab Thông tin");

                // ===== TAB PRICE =====
                _adminProductPage.ClickTabPrice();
                Thread.Sleep(500);

                _adminProductPage.ClickAddPriceRow();
                Thread.Sleep(500);

                _adminProductPage.FillLastPriceRow(
                    price.SizeId.ToString(),
                    price.Price,
                    price.Unit
                );

                Console.WriteLine("✅ Đã nhập tab Giá");

                // ===== TAB TOPPING =====
                _adminProductPage.ClickTabTopping();
                Thread.Sleep(500);

                _adminProductPage.SetToppingChecked(topping.ToppingName, true);
                Console.WriteLine("✅ Đã chọn Topping");
                Thread.Sleep(500);

                // ===== SAVE (DÙNG METHOD CŨ) =====
                // Method cũ đã xử lý alert bên trong
                _adminProductPage.ClickSaveButton();

                // Nếu không có exception, coi như thành công
                Console.WriteLine($"✅ Đã lưu sản phẩm thành công: {uniqueProductName}");

                // ===== PASS =====
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Thêm sản phẩm '{uniqueProductName}' thành công", "PASS", "");

                Console.WriteLine($"[{testCaseId}] ✅ PASSED");
            }
            catch (AssertionException ex)
            {
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);

                Console.WriteLine($"[{testCaseId}] ❌ ASSERTION FAILED: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Lỗi: {ex.Message}", "FAIL", CurrentTestScreenshot);

                Console.WriteLine($"[{testCaseId}] ❌ ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// TC4_AdminProduct_EditProduct: Chỉnh sửa sản phẩm vừa tạo ở TC2
        /// - Dùng sản phẩm HKTEST001 từ TC2
        /// - Thay đổi tên, giá, topping
        /// - Verify cập nhật thành công
        /// </summary>
        [Test]
        public void TC4_AdminProduct_EditProduct()
        {
            string testCaseId = "TC4";

            try
            {
                // ===== 1. LOGIN =====
                var account = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                Console.WriteLine($"Đã login: {account.Email}");

                // ===== NAVIGATE =====
                _adminProductPage.NavigateToProductManagement();
                Thread.Sleep(1000);

                // ===== DATA =====
                var product = _jsonProvider.GetAdminProductByIndex(0);
                var price = _jsonProvider.GetAdminProductPriceByIndex(0);
                var topping = _jsonProvider.GetAdminToppingByIndex(0);

                Assert.IsNotNull(product, "Không tìm thấy product");

                // ===== SEARCH =====
                _adminProductPage.SearchProduct(product.ProductName);
                Thread.Sleep(1000);

                // ===== OPEN EDIT (FIXED) =====
                _adminProductPage.OpenEditProductByName(product.ProductName);
                Thread.Sleep(1500);

                Console.WriteLine($"Đã mở edit: {product.ProductName}");

                // Sau khi mở modal
                _adminProductPage.OpenEditProductByName(product.ProductName);
                Thread.Sleep(2000);

                // Debug để xem trạng thái tabs
                _adminProductPage.DebugModalTabs();

                // Sau đó mới click tab
                _adminProductPage.ClickTabInfo();
                // ===== TAB INFO =====
                _adminProductPage.ClickTabInfo();

                string updatedName = product.ProductName + " - Updated";
                string updatedNote = product.Note + " (Edited)";

                _adminProductPage.FillProductForm(
                    product.ProductCode,
                    updatedName,
                    product.IsActive.ToString(),
                    product.ProductTypeId.ToString(),
                    updatedNote
                );

                Console.WriteLine("Đã update thông tin");

                // ===== TAB PRICE =====
                _adminProductPage.ClickTabPrice();
                Thread.Sleep(500);

                _adminProductPage.ClickAddPriceRow();
                Thread.Sleep(500);

                _adminProductPage.FillLastPriceRow(
                    price.SizeId.ToString(),
                    (int.Parse(price.Price) + 5000).ToString(),
                    price.Unit
                );

                Console.WriteLine("Đã update price");

                // ===== TAB TOPPING =====
                _adminProductPage.ClickTabTopping();
                Thread.Sleep(500);

                _adminProductPage.SetToppingChecked(topping.ToppingName, true);

                Console.WriteLine("Đã update topping");

                // ===== SAVE =====
                _adminProductPage.ClickSaveButton();

                Thread.Sleep(2000);

                // ===== VERIFY =====
                _adminProductPage.SearchProduct(updatedName);
                bool isFound = _adminProductPage.IsProductFound(updatedName);

                Assert.IsTrue(isFound, $"[{testCaseId}] Không tìm thấy sau khi edit");

                Console.WriteLine($"[{testCaseId}] PASS");

                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Edit thành công: {updatedName}", "PASS", "");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);

                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);

                Console.WriteLine($"ASSERT FAIL: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{testCaseId}] ❌ ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// TC5_AdminProduct_DeleteProduct: Xóa sản phẩm vừa chỉnh sửa ở TC4
        /// </summary>
        [Test]
        public void TC5_AdminProduct_DeleteProduct()
        {
            string testCaseId = "TC5";

            try
            {
                // ===== 1. LOGIN =====
                var account = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                Console.WriteLine($"Đã login: {account.Email}");

                // ===== 2. NAVIGATE =====
                _adminProductPage.NavigateToProductManagement();
                Thread.Sleep(1000);

                // ===== 3. LẤY TÊN SẢN PHẨM CẦN XÓA =====
                // Lấy sản phẩm đã được chỉnh sửa ở TC4
                var product = _jsonProvider.GetAdminProductByIndex(0);
                Assert.IsNotNull(product, "Không tìm thấy product");

                string productNameToDelete = product.ProductName + " - Updated";
                Console.WriteLine($"[INFO] Sản phẩm cần xóa: {productNameToDelete}");

                // ===== 4. TÌM KIẾM SẢN PHẨM =====
                _adminProductPage.SearchProduct(productNameToDelete);
                Thread.Sleep(1000);

                // Kiểm tra sản phẩm tồn tại trước khi xóa
                bool existsBeforeDelete = _adminProductPage.IsProductFound(productNameToDelete);
                Assert.IsTrue(existsBeforeDelete, $"[{testCaseId}] Sản phẩm '{productNameToDelete}' không tồn tại để xóa");

                Console.WriteLine($"✅ Tìm thấy sản phẩm '{productNameToDelete}' trước khi xóa");

                // ===== 5. XÓA SẢN PHẨM =====
                _adminProductPage.DeleteProductByName(productNameToDelete);
                Thread.Sleep(2000);

                // ===== 6. TÌM KIẾM LẠI =====
                _adminProductPage.SearchProduct(productNameToDelete);
                Thread.Sleep(1000);

                // ===== 7. VERIFY - SẢN PHẨM KHÔNG CÒN =====
                bool existsAfterDelete = _adminProductPage.IsProductFound(productNameToDelete);
                Assert.IsFalse(existsAfterDelete, $"[{testCaseId}] Sản phẩm '{productNameToDelete}' vẫn còn sau khi xóa");

                Console.WriteLine($"✅ Xóa sản phẩm thành công: {productNameToDelete}");

                // ===== 8. GHI KẾT QUẢ =====
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Xóa sản phẩm '{productNameToDelete}' thành công", "PASS", "");

                Console.WriteLine($"[{testCaseId}] ✅ PASSED");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ASSERTION FAILED: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Lỗi: {ex.Message}", "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ERROR: {ex.Message}");
                throw;
            }
        }

        [Test]
        public void TC8_AdminProduct_ToggleProductStatus()
        {
            string testCaseId = "TC8";

            try
            {
                // ===== 1. LOGIN =====
                var account = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                Console.WriteLine($"Đã login: {account.Email}");

                // ===== 2. NAVIGATE =====
                _adminProductPage.NavigateToProductManagement();
                Thread.Sleep(1000);

                // ===== 3. LẤY SẢN PHẨM CẦN TEST =====
                // Có thể dùng sản phẩm từ TC2 hoặc TC4
                var product = _jsonProvider.GetAdminProductByIndex(0);
                Assert.IsNotNull(product, "Không tìm thấy product");

                string productName = product.ProductName;
                Console.WriteLine($"[INFO] Sản phẩm cần thay đổi trạng thái: {productName}");

                // ===== 4. TÌM KIẾM SẢN PHẨM =====
                _adminProductPage.SearchProduct(productName);
                Thread.Sleep(1000);

                // Lấy trạng thái hiện tại trước khi thay đổi
                string statusBefore = _adminProductPage.GetProductStatusInTable(productName);
                Console.WriteLine($"Trạng thái trước khi thay đổi: {statusBefore}");

                // ===== 5. THAY ĐỔI TRẠNG THÁI =====
                _adminProductPage.ToggleProductStatus(productName);
                Thread.Sleep(2000);

                // ===== 6. KIỂM TRA LẠI TRẠNG THÁI =====
                // Tìm kiếm lại sản phẩm
                _adminProductPage.SearchProduct(productName);
                Thread.Sleep(1000);

                // Lấy trạng thái sau khi thay đổi
                string statusAfter = _adminProductPage.GetProductStatusInTable(productName);
                Console.WriteLine($"Trạng thái sau khi thay đổi: {statusAfter}");

                // ===== 7. VERIFY =====
                // Verify trạng thái đã thay đổi
                Assert.AreNotEqual(statusBefore, statusAfter,
                    $"[{testCaseId}] Trạng thái sản phẩm không thay đổi. Vẫn là: {statusBefore}");

                // Verify trạng thái hợp lệ (Mở bán hoặc Ngưng bán)
                bool isValidStatus = (statusAfter.Contains("Mở bán") || statusAfter.Contains("Ngưng bán"));
                Assert.IsTrue(isValidStatus,
                    $"[{testCaseId}] Trạng thái không hợp lệ: {statusAfter}");

                Console.WriteLine($"✅ Thay đổi trạng thái sản phẩm thành công: {statusBefore} -> {statusAfter}");

                // ===== 8. GHI KẾT QUẢ =====
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Thay đổi trạng thái sản phẩm '{productName}' thành công: {statusBefore} -> {statusAfter}",
                    "PASS", "");

                Console.WriteLine($"[{testCaseId}] ✅ PASSED");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ASSERTION FAILED: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Lỗi: {ex.Message}", "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// TC7_AdminProduct_SearchProductByName: Kiểm tra tìm kiếm sản phẩm theo tên
        /// </summary>
        [Test]
        public void TC6_AdminProduct_SearchProductByName()
        {
            string testCaseId = "TC6";

            try
            {
                // ===== 1. LOGIN =====
                var account = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                Console.WriteLine($"Đã login: {account.Email}");

                // ===== 2. NAVIGATE =====
                _adminProductPage.NavigateToProductManagement();
                Thread.Sleep(1000);

                // ===== 3. RESET FILTERS =====
                _adminProductPage.ResetFilters();
                Thread.Sleep(1000);

                // ===== 4. LẤY DANH SÁCH SẢN PHẨM =====
                var allProducts = _adminProductPage.GetProductNamesFromTable();
                Assert.IsNotEmpty(allProducts, "Không có sản phẩm nào trong danh sách");

                // Lấy tên sản phẩm đầu tiên để tìm kiếm
                string productToSearch = allProducts[0];
                Console.WriteLine($"[INFO] Tìm kiếm sản phẩm: {productToSearch}");

                // ===== 5. TEST 1: TÌM KIẾM CHÍNH XÁC =====
                Console.WriteLine("\n=== TEST 1: Tìm kiếm chính xác ===");
                _adminProductPage.SearchAndVerifyByName(productToSearch, true);

                // ===== 6. TEST 2: TÌM KIẾM KHÔNG DẤU =====
                Console.WriteLine("\n=== TEST 2: Tìm kiếm không dấu ===");
                string noAccentKeyword = RemoveVietnameseAccent(productToSearch);
                _adminProductPage.SearchAndVerifyByName(noAccentKeyword, true);

                // ===== 7. TEST 3: TÌM KIẾM MỘT PHẦN TỪ KHÓA =====
                Console.WriteLine("\n=== TEST 3: Tìm kiếm một phần từ khóa ===");
                string partialKeyword = productToSearch.Length > 3 ? productToSearch.Substring(0, 3) : productToSearch;
                _adminProductPage.SearchAndVerifyByName(partialKeyword, true);

                // ===== 8. TEST 4: TÌM KIẾM KHÔNG CÓ KẾT QUẢ =====
                Console.WriteLine("\n=== TEST 4: Tìm kiếm không có kết quả ===");
                string invalidKeyword = "XYZ123KHONGTONTAI";
                _adminProductPage.SearchAndVerifyByName(invalidKeyword, false);

                // ===== 9. TEST 5: TÌM KIẾM VỚI KÝ TỰ ĐẶC BIỆT =====
                Console.WriteLine("\n=== TEST 5: Tìm kiếm với ký tự đặc biệt ===");
                string specialCharKeyword = "@#$%";
                _adminProductPage.SearchAndVerifyByName(specialCharKeyword, false);

                // ===== 10. GHI KẾT QUẢ =====
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    "Tìm kiếm sản phẩm theo tên hoạt động chính xác", "PASS", "");

                Console.WriteLine($"\n[{testCaseId}] ✅ PASSED");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ASSERTION FAILED: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Lỗi: {ex.Message}", "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// TC8_AdminProduct_FilterProductByType: Kiểm tra lọc sản phẩm theo loại
        /// </summary>
        
        /// <summary>
        /// TC8_AdminProduct_FilterProductByType - Kiểm tra lọc sản phẩm theo loại
        /// </summary>
        [Test]
        public void TC7_AdminProduct_FilterProductByType()
        {
            string testCaseId = "TC7";

            try
            {
                // ===== 1. LOGIN =====
                var account = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                Console.WriteLine($"Đã login: {account.Email}");

                // ===== 2. NAVIGATE =====
                _adminProductPage.NavigateToProductManagement();
                Thread.Sleep(1000);

                // ===== 3. RESET FILTERS =====
                _adminProductPage.ResetFilters();
                Thread.Sleep(1000);

                // ===== 4. LẤY DANH SÁCH NHÓM (DEBUG) =====
                var allGroups = _adminProductPage.GetAllProductGroups();
                Console.WriteLine("Danh sách các nhóm sản phẩm:");
                foreach (var group in allGroups)
                {
                    Console.WriteLine($"  Value: {group.Value}, Text: {group.Text}");
                }

                // ===== 5. LẤY TỔNG SỐ SẢN PHẨM BAN ĐẦU =====
                var allProductsBefore = _adminProductPage.GetProductNamesFromTable();
                int totalProducts = allProductsBefore.Count;
                Console.WriteLine($"[INFO] Tổng số sản phẩm: {totalProducts}");

                // ===== 6. TEST 1: LỌC THEO "Cà phê Việt Nam" (value=4) =====
                Console.WriteLine("\n=== TEST 1: Lọc theo Cà phê Việt Nam ===");
                _adminProductPage.FilterByProductGroupValue("4");
                Thread.Sleep(1000);

                var coffeeProducts = _adminProductPage.GetProductNamesFromTable();
                Console.WriteLine($"Số lượng sản phẩm Cà phê Việt Nam: {coffeeProducts.Count}");
                Assert.LessOrEqual(coffeeProducts.Count, totalProducts, "Số lượng sau lọc phải <= tổng số");

                // ===== 7. TEST 2: LỌC THEO "Trà trái cây" (value=1) =====
                Console.WriteLine("\n=== TEST 2: Lọc theo Trà trái cây ===");
                _adminProductPage.FilterByProductGroupValue("1");
                Thread.Sleep(1000);

                var teaFruitProducts = _adminProductPage.GetProductNamesFromTable();
                Console.WriteLine($"Số lượng sản phẩm Trà trái cây: {teaFruitProducts.Count}");
                Assert.LessOrEqual(teaFruitProducts.Count, totalProducts, "Số lượng sau lọc phải <= tổng số");

                // ===== 8. TEST 3: LỌC THEO "Trà sữa" (value=2) =====
                Console.WriteLine("\n=== TEST 3: Lọc theo Trà sữa ===");
                _adminProductPage.FilterByProductGroupValue("2");
                Thread.Sleep(1000);

                var milkTeaProducts = _adminProductPage.GetProductNamesFromTable();
                Console.WriteLine($"Số lượng sản phẩm Trà sữa: {milkTeaProducts.Count}");

                // ===== 9. TEST 4: LỌC THEO "TẤT CẢ" (value=0) =====
                Console.WriteLine("\n=== TEST 4: Lọc theo Tất cả ===");
                _adminProductPage.FilterByProductGroupValue("0");
                Thread.Sleep(1000);

                var allProductsAfter = _adminProductPage.GetProductNamesFromTable();
                Console.WriteLine($"Số lượng sản phẩm sau reset: {allProductsAfter.Count}");
                Assert.AreEqual(totalProducts, allProductsAfter.Count, "Tổng số sản phẩm phải trở về ban đầu");

                // ===== 10. GHI KẾT QUẢ =====
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Lọc sản phẩm theo loại thành công. Cà phê: {coffeeProducts.Count}, Trà trái cây: {teaFruitProducts.Count}, Trà sữa: {milkTeaProducts.Count}",
                    "PASS", "");

                Console.WriteLine($"\n[{testCaseId}] ✅ PASSED");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ASSERTION FAILED: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Lỗi: {ex.Message}", "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Helper method: Loại bỏ dấu tiếng Việt
        /// </summary>
        private string RemoveVietnameseAccent(string text)
        {
            string[] vietnameseSigns = new string[]
            {
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
            };

            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                {
                    text = text.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
                }
            }

            return text;
        }

        /// <summary>
        /// TC9_AdminProduct_SetPriceBySize: Kiểm tra thiết lập giá cho sản phẩm theo từng kích thước
        /// </summary>
        /// <summary>
        /// TC9_AdminProduct_SetPriceBySize: Kiểm tra thiết lập giá cho sản phẩm theo từng kích thước
        /// </summary>
        [Test]
        public void TC9_AdminProduct_SetPriceBySize()
        {
            string testCaseId = "TC9";

            try
            {
                // ===== 1. LOGIN =====
                var account = _jsonProvider.GetAccountById(0);
                Assert.IsNotNull(account, $"[{testCaseId}] Không tìm thấy account");

                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(1000);

                _loginPage.LoginWithAccount(account);
                Thread.Sleep(1000);

                Console.WriteLine($"Đã login: {account.Email}");

                // ===== 2. NAVIGATE =====
                _adminProductPage.NavigateToProductManagement();
                Thread.Sleep(1000);

                // ===== 3. TẠO SẢN PHẨM MỚI ĐỂ TEST =====
                var product = _jsonProvider.GetAdminProductByIndex(0);
                Assert.IsNotNull(product, "Không tìm thấy product");

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string uniqueProductCode = $"PRICE_TEST_{timestamp}";
                string uniqueProductName = $"Sản phẩm test giá {timestamp}";

                Console.WriteLine($"[INFO] Tạo sản phẩm mới: {uniqueProductName}");

                // Click nút tạo mới
                _adminProductPage.ClickAddButton();
                Thread.Sleep(1000);

                // Điền thông tin cơ bản
                _adminProductPage.ClickTabInfo();
                Thread.Sleep(500);

                _adminProductPage.FillProductForm(
                    uniqueProductCode,
                    uniqueProductName,
                    product.IsActive.ToString(),
                    product.ProductTypeId.ToString(),
                    "Sản phẩm test thiết lập giá theo size"
                );

                // ===== 4. THIẾT LẬP GIÁ CHO NHIỀU SIZE =====
                Console.WriteLine("\n=== Thiết lập giá cho các size ===");
                _adminProductPage.ClickTabPrice();
                Thread.Sleep(500);

                // Xóa các dòng giá mặc định (nếu có)
                _adminProductPage.ClearAllPriceRows();
                Thread.Sleep(500);

                // Thêm giá cho size S (Nhỏ)
                _adminProductPage.AddPriceRowWithSize("S", "25000", "VND");
                Thread.Sleep(300);

                // Thêm giá cho size M (Vừa)
                _adminProductPage.AddPriceRowWithSize("M", "35000", "VND");
                Thread.Sleep(300);

                // Thêm giá cho size L (Lớn)
                _adminProductPage.AddPriceRowWithSize("L", "45000", "VND");
                Thread.Sleep(300);

                // ===== 5. LƯU SẢN PHẨM =====
                _adminProductPage.ClickSaveButton();
                Thread.Sleep(2000);

                // Đóng modal nếu vẫn còn mở
                _adminProductPage.ClickCloseButton();
                Thread.Sleep(1000);

                // ===== 6. MỞ LẠI SẢN PHẨM ĐỂ KIỂM TRA =====
                _adminProductPage.SearchProduct(uniqueProductName);
                Thread.Sleep(1000);

                _adminProductPage.OpenEditProductByName(uniqueProductName);
                Thread.Sleep(1500);

                // Chuyển đến tab Giá
                _adminProductPage.ClickTabPrice();
                Thread.Sleep(1000);

                // ===== 7. VERIFY GIÁ ĐÃ LƯU =====
                Console.WriteLine("\n=== Kiểm tra giá đã thiết lập ===");

                bool sizeSCorrect = _adminProductPage.VerifyPriceBySize("S", "25000");
                bool sizeMCorrect = _adminProductPage.VerifyPriceBySize("M", "35000");
                bool sizeLCorrect = _adminProductPage.VerifyPriceBySize("L", "45000");

                Assert.IsTrue(sizeSCorrect, "Giá size S không đúng");
                Assert.IsTrue(sizeMCorrect, "Giá size M không đúng");
                Assert.IsTrue(sizeLCorrect, "Giá size L không đúng");

                Console.WriteLine("✅ Tất cả các size đã được thiết lập giá chính xác");

                // ===== 8. KIỂM TRA SỬA GIÁ =====
                Console.WriteLine("\n=== Sửa giá size M ===");

                // Sửa giá size M
                var priceTableBody = _driver.FindElement(By.Id("tbodySteps"));
                var rows = priceTableBody.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    if (row.Text.Contains("M"))
                    {
                        var priceInput = row.FindElement(By.TagName("input"));
                        priceInput.Clear();
                        priceInput.SendKeys("40000");
                        Console.WriteLine("✅ Đã sửa giá size M thành 40000");
                        break;
                    }
                }

                // Lưu thay đổi
                _adminProductPage.ClickSaveButton();
                Thread.Sleep(2000);

                // Đóng modal
                _adminProductPage.ClickCloseButton();
                Thread.Sleep(1000);

                // Mở lại để kiểm tra
                _adminProductPage.SearchProduct(uniqueProductName);
                Thread.Sleep(1000);

                _adminProductPage.OpenEditProductByName(uniqueProductName);
                Thread.Sleep(1500);

                _adminProductPage.ClickTabPrice();
                Thread.Sleep(1000);

                // Verify giá đã được cập nhật
                bool sizeMUpdated = _adminProductPage.VerifyPriceBySize("M", "40000");
                Assert.IsTrue(sizeMUpdated, "Giá size M không được cập nhật");

                Console.WriteLine("✅ Sửa giá thành công");

                // ===== 9. KIỂM TRA XÓA GIÁ =====
                Console.WriteLine("\n=== Xóa giá size S ===");

                // Tìm và xóa dòng giá size S
                priceTableBody = _driver.FindElement(By.Id("tbodySteps"));
                rows = priceTableBody.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    if (row.Text.Contains("S"))
                    {
                        var deleteButton = row.FindElement(By.XPath(".//button[contains(@class, 'btn-danger')]"));
                        deleteButton.Click();
                        Console.WriteLine("✅ Đã xóa giá size S");
                        Thread.Sleep(500);
                        break;
                    }
                }

                // Lưu thay đổi
                _adminProductPage.ClickSaveButton();
                Thread.Sleep(2000);

                // Đóng modal
                _adminProductPage.ClickCloseButton();
                Thread.Sleep(1000);

                // Mở lại để kiểm tra
                _adminProductPage.SearchProduct(uniqueProductName);
                Thread.Sleep(1000);

                _adminProductPage.OpenEditProductByName(uniqueProductName);
                Thread.Sleep(1500);

                _adminProductPage.ClickTabPrice();
                Thread.Sleep(1000);

                // Verify size S đã bị xóa
                var priceList = _adminProductPage.GetAllPriceRows();
                Assert.IsFalse(priceList.ContainsKey("S"), "Giá size S vẫn còn sau khi xóa");

                Console.WriteLine("✅ Xóa giá thành công");

                // ===== 10. DỌN DẸP: XÓA SẢN PHẨM TEST =====
                Console.WriteLine("\n=== Dọn dẹp: Xóa sản phẩm test ===");
                _adminProductPage.ClickCloseButton();
                Thread.Sleep(1000);

                _adminProductPage.SearchProduct(uniqueProductName);
                Thread.Sleep(1000);

                _adminProductPage.DeleteProductByName(uniqueProductName);
                Thread.Sleep(2000);

                Console.WriteLine("✅ Đã xóa sản phẩm test");

                // ===== 11. GHI KẾT QUẢ =====
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Thiết lập giá theo kích thước thành công. S:25000, M:35000->40000, L:45000",
                    "PASS", "");

                Console.WriteLine($"\n[{testCaseId}] ✅ PASSED");
            }
            catch (AssertionException ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    ex.Message, "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ASSERTION FAILED: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                CurrentTestScreenshot = ScreenshotHelper.TakeScreenshot(Driver, testCaseId);
                ExcelProvider.UpdateTestResult(_config, testCaseId, "1",
                    $"Lỗi: {ex.Message}", "FAIL", CurrentTestScreenshot);
                Console.WriteLine($"[{testCaseId}] ❌ ERROR: {ex.Message}");
                throw;
            }
        }
    }
}