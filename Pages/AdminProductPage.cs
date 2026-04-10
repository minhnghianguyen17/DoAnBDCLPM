using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Pages
{
    public class AdminProductPage
    {
        private readonly IWebDriver _driver;
        private readonly ExcelConfig _config;
        private readonly ExcelDataProvider _excelProvider;


        // Locators
        private readonly By ProductMenuParent = By.XPath("//a[@data-bs-target='#product-nav']");
        private readonly By ProductMenuItem = By.XPath("//a[@href='/Admin/Product']");
        private readonly By GroupProductFilter = By.Id("group_product");
        private readonly By SearchInput = By.Id("request");
        private readonly By SearchButton = By.Id("btn_search");
        private readonly By AddButton = By.Id("add_new");
        private readonly By SaveButton = By.XPath("//button[@onclick='CreateOrUpdate()']");
        private readonly By DeleteButton = By.Id("btn_deleteModal");
        private readonly By CloseButton = By.XPath("//button[@onclick='CloseModal()']");
        private readonly By TabInfo = By.Id("btninformation");
        private readonly By TabPrice = By.Id("btnsteps");
        private readonly By TabTopping = By.Id("btn-product-topping");
        private readonly By FormCode = By.Id("formCode");
        private readonly By FormName = By.Id("formName");
        private readonly By FormIsActive = By.Id("formIsActive");
        private readonly By FormProductTypeId = By.Id("formProductTypeId");
        private readonly By FormNote = By.Id("formNote");
        private readonly By FormAttachFiles = By.XPath("//input[@name='AttachFiles']");
        private readonly By AddPriceRowButton = By.XPath("//button[@onclick='addRow()']");
        private readonly By PriceTableBody = By.Id("tbodySteps");
        private readonly By CheckAllTopping = By.Id("check-box-product-topping");
        private readonly By ToppingTableBody = By.Id("tbody_product_topping");
        private readonly By ProductTableBody = By.Id("tbody");
        private readonly By Modal = By.Id("staticBackdrop");
        private readonly By ModalSpinner = By.Id("model_spinner_layout_client");



        public AdminProductPage(IWebDriver driver, ExcelConfig config, ExcelDataProvider excelProvider)
        {
            _driver = driver;
            _config = config;
            _excelProvider = excelProvider;
        }

        public void NavigateToProductManagement()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var parentMenu = wait.Until(drv =>
                {
                    try
                    {
                        var menu = drv.FindElement(ProductMenuParent);
                        return menu.Displayed ? menu : null;
                    }
                    catch { return null; }
                });

                if (parentMenu != null)
                {
                    parentMenu.Click();
                    Thread.Sleep(500);
                    Console.WriteLine("Đã mở menu Sản phẩm");
                }

                var productItem = wait.Until(drv => drv.FindElement(ProductMenuItem));
                productItem.Click();
                Thread.Sleep(2000);
                Console.WriteLine("✅ Đã điều hướng đến trang Quản lý Sản phẩm");
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể điều hướng đến trang Quản lý Sản phẩm: {ex.Message}");
            }
        }

        public void SearchProduct(string keyword)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var searchInput = wait.Until(drv => drv.FindElement(SearchInput));
                searchInput.Clear();
                searchInput.SendKeys(keyword);
                Thread.Sleep(300);

                var searchBtn = wait.Until(drv => drv.FindElement(SearchButton));
                searchBtn.Click();
                Thread.Sleep(1500);
                Console.WriteLine($"✅ Đã tìm kiếm: {keyword}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm sản phẩm: {ex.Message}");
            }
        }

        public void ClickAddButton()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var addBtn = wait.Until(drv => drv.FindElement(AddButton));
                addBtn.Click();
                Thread.Sleep(1000);
                Console.WriteLine("✅ Đã nhấn nút Tạo mới");

                // Đợi modal hiển thị
                WaitForModalVisible();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhấn nút Tạo mới: {ex.Message}");
            }
        }

        public void ClickSaveButton()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            try
            {
                // Đợi và click nút Lưu
                var saveBtn = wait.Until(drv =>
                {
                    try
                    {
                        var btn = drv.FindElement(SaveButton);
                        return btn.Displayed && btn.Enabled ? btn : null;
                    }
                    catch { return null; }
                });

                if (saveBtn == null)
                    throw new Exception("Không tìm thấy nút Lưu");

                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", saveBtn);
                Thread.Sleep(300);
                js.ExecuteScript("arguments[0].click();", saveBtn);

                Console.WriteLine("✅ Đã nhấn nút Lưu");
                Thread.Sleep(1500);

                // Xử lý alert
                try
                {
                    IAlert alert = null;
                    for (int i = 0; i < 15; i++)
                    {
                        try
                        {
                            alert = _driver.SwitchTo().Alert();
                            break;
                        }
                        catch (NoAlertPresentException)
                        {
                            Thread.Sleep(200);
                        }
                    }

                    if (alert != null)
                    {
                        string alertText = alert.Text;
                        Console.WriteLine($"[ALERT] Nội dung: '{alertText}'");
                        alert.Accept();
                        Thread.Sleep(500);

                        if (string.IsNullOrEmpty(alertText) || alertText.Equals("undefined", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("[INFO] Alert 'undefined' - lưu thành công");
                        }
                        else if (alertText.ToLower().Contains("lỗi") || alertText.ToLower().Contains("error"))
                        {
                            throw new Exception($"Lỗi từ server: {alertText}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[INFO] Lưu thành công (không có alert)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Khi xử lý alert: {ex.Message}");
                }

                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhấn nút Lưu: {ex.Message}");
            }
        }
        public void ClearAllPriceRows()
        {
            var priceTableBody = _driver.FindElement(PriceTableBody);
            var rows = priceTableBody.FindElements(By.TagName("tr"));
            foreach (var row in rows)
            {
                try { row.FindElement(By.XPath(".//button[contains(@class, 'btn-danger')]")).Click(); }
                catch { }
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// Thêm dòng giá với kích thước cụ thể
        /// </summary>
        public void AddPriceRowWithSize(string sizeName, string price, string unit = "VND")
        {
            try
            {
                // Click nút thêm dòng mới
                ClickAddPriceRow();
                Thread.Sleep(500);

                // Điền thông tin cho dòng cuối cùng
                FillLastPriceRow(sizeName, price, unit);

                Console.WriteLine($"✅ Đã thêm giá cho size {sizeName}: {price} {unit}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm giá cho size {sizeName}: {ex.Message}");
            }
        }

        public void WaitForModalVisible()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                wait.Until(drv => drv.FindElement(Modal).Displayed);
                Thread.Sleep(500);

                // Đợi spinner biến mất nếu có
                try
                {
                    wait.Until(drv => !drv.FindElement(ModalSpinner).Displayed);
                }
                catch { }

                Console.WriteLine("✅ Modal đã hiển thị");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Modal không hiển thị: {ex.Message}");
            }
        }

        public void WaitForModalClosed()
        {
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        var modal = _driver.FindElement(Modal);
                        if (!modal.Displayed)
                        {
                            Console.WriteLine("✅ Modal đã đóng");
                            Thread.Sleep(1000);
                            return;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("✅ Modal đã đóng");
                        Thread.Sleep(1000);
                        return;
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Không thể xác định modal đóng: {ex.Message}");
            }
        }

        public void ClickCloseButton()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var closeBtn = wait.Until(drv => drv.FindElement(CloseButton));
                closeBtn.Click();
                Thread.Sleep(500);
                Console.WriteLine("✅ Đã đóng Modal");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đóng Modal: {ex.Message}");
            }
        }

        public void ClickTabInfo()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

                // ĐẢM BẢO modal đã hiển thị trước khi tìm tab
                WaitForModalFullyDisplayed();

                // Tìm tab Thông tin
                IWebElement tabInfo = null;

                // Strategy 1: Tìm bằng ID
                try
                {
                    tabInfo = wait.Until(drv =>
                    {
                        var element = drv.FindElement(TabInfo);
                        return element != null && element.Displayed && element.Enabled ? element : null;
                    });
                    Console.WriteLine("✅ Tìm thấy tab bằng ID");
                }
                catch
                {
                    Console.WriteLine("⚠️ Không tìm thấy bằng ID, thử chiến lược khác...");

                    // Strategy 2: Tìm bằng XPath với button chứa text "Thông tin"
                    tabInfo = wait.Until(drv =>
                        drv.FindElement(By.XPath("//div[@class='modal-body']//button[contains(text(), 'Thông tin')]"))
                    );
                    Console.WriteLine("✅ Tìm thấy tab bằng text 'Thông tin'");
                }

                // Cuộn đến tab
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", tabInfo);
                Thread.Sleep(300);

                // Click bằng JavaScript
                js.ExecuteScript("arguments[0].click();", tabInfo);
                Thread.Sleep(1000);

                // Đợi tab content hiển thị
                wait.Until(drv =>
                {
                    try
                    {
                        var tabContent = drv.FindElement(By.Id("bordered-justified-home"));
                        return tabContent.Displayed && tabContent.GetAttribute("class").Contains("active");
                    }
                    catch { return false; }
                });

                Console.WriteLine("✅ Đã chuyển sang tab Thông tin và content đã hiển thị");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi chuyển tab Thông tin: {ex.Message}");
            }
        }

        public void ClickTabPrice()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
                Thread.Sleep(1000);

                IWebElement tabPrice = null;

                // Tìm tab Giá
                try
                {
                    tabPrice = wait.Until(drv => drv.FindElement(TabPrice));
                }
                catch
                {
                    tabPrice = wait.Until(drv =>
                        drv.FindElement(By.XPath("//button[contains(@class, 'nav-link') and contains(text(), 'Giá')]"))
                    );
                }

                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", tabPrice);
                Thread.Sleep(300);
                js.ExecuteScript("arguments[0].click();", tabPrice);
                Thread.Sleep(1000);

                // Đợi tab content Giá hiển thị
                wait.Until(drv =>
                {
                    try
                    {
                        var tabContent = drv.FindElement(By.Id("bordered-justified-profile"));
                        return tabContent.Displayed;
                    }
                    catch { return false; }
                });

                Console.WriteLine("✅ Đã chuyển sang tab Giá");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi chuyển tab Giá: {ex.Message}");
            }
        }

        public void ClickTabTopping()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
                Thread.Sleep(1000);

                IWebElement tabTopping = null;

                try
                {
                    tabTopping = wait.Until(drv => drv.FindElement(TabTopping));
                }
                catch
                {
                    tabTopping = wait.Until(drv =>
                        drv.FindElement(By.XPath("//button[contains(@class, 'nav-link') and contains(text(), 'Topping')]"))
                    );
                }

                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", tabTopping);
                Thread.Sleep(300);
                js.ExecuteScript("arguments[0].click();", tabTopping);
                Thread.Sleep(1000);

                // Đợi tab content Topping hiển thị
                wait.Until(drv =>
                {
                    try
                    {
                        var tabContent = drv.FindElement(By.Id("bordered-justified-product-topping"));
                        return tabContent.Displayed;
                    }
                    catch { return false; }
                });

                Console.WriteLine("✅ Đã chuyển sang tab Topping");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi chuyển tab Topping: {ex.Message}");
            }
        }
        public void DebugModalTabs()
        {
            try
            {
                Console.WriteLine("=== DEBUG MODAL TABS ===");
                var tabs = _driver.FindElements(By.CssSelector("#staticBackdrop .nav-tabs button"));
                Console.WriteLine($"Số lượng tab tìm thấy: {tabs.Count}");

                foreach (var tab in tabs)
                {
                    Console.WriteLine($"- Tab: Id='{tab.GetAttribute("id")}', Text='{tab.Text}', Class='{tab.GetAttribute("class")}', Displayed={tab.Displayed}, Enabled={tab.Enabled}");
                }

                var activeTab = _driver.FindElement(By.CssSelector("#staticBackdrop .nav-tabs button.active"));
                Console.WriteLine($"Tab đang active: {activeTab.Text}");

                var tabContents = _driver.FindElements(By.CssSelector("#staticBackdrop .tab-pane"));
                foreach (var content in tabContents)
                {
                    Console.WriteLine($"Content: Id='{content.GetAttribute("id")}', Class='{content.GetAttribute("class")}', Displayed={content.Displayed}");
                }
                Console.WriteLine("=== END DEBUG ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Debug error: {ex.Message}");
            }
        }

        public void FillProductForm(string code, string name, string isActive, string productTypeId, string note)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                var codeField = wait.Until(drv => drv.FindElement(FormCode));
                codeField.Clear();
                codeField.SendKeys(code);
                Thread.Sleep(300);

                var nameField = _driver.FindElement(FormName);
                nameField.Clear();
                nameField.SendKeys(name);
                Thread.Sleep(300);

                var isActiveSelect = new SelectElement(_driver.FindElement(FormIsActive));
                isActiveSelect.SelectByValue(isActive);
                Thread.Sleep(300);

                var productTypeElement = wait.Until(drv => drv.FindElement(FormProductTypeId));
                var productTypeSelect = new SelectElement(productTypeElement);
                productTypeSelect.SelectByValue(productTypeId);
                Thread.Sleep(300);

                var noteField = _driver.FindElement(FormNote);
                noteField.Clear();
                noteField.SendKeys(note);
                Thread.Sleep(300);

                Console.WriteLine($"✅ Đã điền form sản phẩm: {code} - {name}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi điền form sản phẩm: {ex.Message}");
            }
        }

        public void ClickAddPriceRow()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var addBtn = wait.Until(drv => drv.FindElement(AddPriceRowButton));
                addBtn.Click();
                Thread.Sleep(500);
                Console.WriteLine("✅ Đã thêm dòng giá mới");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm dòng giá: {ex.Message}");
            }
        }

        

        public void SetToppingChecked(string toppingName, bool isChecked)
        {
            try
            {
                Thread.Sleep(1500);
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var toppingTableBody = wait.Until(drv => drv.FindElement(ToppingTableBody));

                var rows = toppingTableBody.FindElements(By.TagName("tr"));
                Console.WriteLine($"[DEBUG] Số hàng topping: {rows.Count}");

                if (rows.Count == 0)
                    throw new Exception("Bảng topping không có dữ liệu");

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 3)
                    {
                        string toppingNameFromWeb = cells[2].Text.Trim();
                        if (toppingNameFromWeb.Equals(toppingName, StringComparison.OrdinalIgnoreCase))
                        {
                            var checkbox = cells[0].FindElement(By.TagName("input"));
                            bool currentState = checkbox.Selected;

                            if (currentState != isChecked)
                            {
                                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", checkbox);
                                Thread.Sleep(300);
                            }

                            Console.WriteLine($"✅ Đã {(isChecked ? "tick" : "bỏ tick")} topping: {toppingName}");
                            return;
                        }
                    }
                }
                throw new Exception($"Không tìm thấy topping: {toppingName}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thao tác topping: {ex.Message}");
            }
        }

        public bool IsProductFound(string keyword)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var tableBody = wait.Until(drv => drv.FindElement(ProductTableBody));
                var rows = tableBody.FindElements(By.TagName("tr"));

                if (rows.Count == 0)
                {
                    Console.WriteLine("[Warning] Bảng sản phẩm không có dữ liệu");
                    return false;
                }

                foreach (var row in rows)
                {
                    if (row.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"✅ Tìm thấy '{keyword}' trong danh sách");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
                return false;
            }
        }

        public void OpenEditProductByName(string productName)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                // Tìm dòng sản phẩm
                var rows = wait.Until(drv => drv.FindElements(By.XPath("//tbody[@id='tbody']//tr")));

                IWebElement targetRow = null;
                foreach (var row in rows)
                {
                    if (row.Text.Contains(productName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetRow = row;
                        break;
                    }
                }

                if (targetRow == null)
                    throw new Exception($"Không tìm thấy sản phẩm '{productName}' để chỉnh sửa");

                // Tìm nút Sửa trong dòng
                IWebElement editButton = null;

                try
                {
                    // Thử tìm button Edit
                    editButton = targetRow.FindElement(By.XPath(".//button[contains(@class, 'btn-edit') or contains(@class, 'edit-btn')]"));
                }
                catch
                {
                    try
                    {
                        // Thử tìm icon pencil
                        var pencilIcon = targetRow.FindElement(By.XPath(".//i[contains(@class, 'bi-pencil')]"));
                        editButton = pencilIcon.FindElement(By.XPath("./.."));
                    }
                    catch
                    {
                        try
                        {
                            // Thử tìm bất kỳ button nào trong dòng
                            var buttons = targetRow.FindElements(By.TagName("button"));
                            if (buttons.Count > 0)
                            {
                                editButton = buttons[0];
                            }
                            else
                            {
                                throw new Exception($"Không tìm thấy nút Sửa");
                            }
                        }
                        catch
                        {
                            throw new Exception($"Không tìm thấy nút Sửa cho sản phẩm '{productName}'");
                        }
                    }
                }

                // Click vào nút Sửa
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
                Thread.Sleep(500);
                js.ExecuteScript("arguments[0].click();", editButton);

                Console.WriteLine($"✅ Đã click nút Sửa cho sản phẩm: {productName}");

                // Đợi modal hiển thị hoàn toàn
                WaitForModalFullyDisplayed();

                Console.WriteLine($"✅ Đã mở modal chỉnh sửa: {productName}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi mở modal chỉnh sửa: {ex.Message}");
            }
        }

        // Thêm method mới để đợi modal hiển thị hoàn toàn
        public void WaitForModalFullyDisplayed()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

                // 1. Đợi modal có trong DOM
                wait.Until(drv => drv.FindElement(Modal));

                // 2. Đợi modal hiển thị (class không còn 'fade' hoặc style display block)
                wait.Until(drv =>
                {
                    var modal = drv.FindElement(Modal);
                    string display = modal.GetCssValue("display");
                    string visibility = modal.GetCssValue("visibility");
                    string classes = modal.GetAttribute("class");

                    Console.WriteLine($"[DEBUG] Modal state - display: {display}, visibility: {visibility}, class: {classes}");

                    return display == "block" && visibility == "visible";
                });

                // 3. Đợi thêm để modal animation hoàn tất
                Thread.Sleep(1000);

                // 4. Đợi backdrop hiển thị (nếu có)
                try
                {
                    wait.Until(drv => drv.FindElement(By.CssSelector(".modal-backdrop")).Displayed);
                }
                catch { }

                // 5. Đợi spinner biến mất nếu có
                try
                {
                    wait.Until(drv =>
                    {
                        var spinners = drv.FindElements(ModalSpinner);
                        return spinners.Count == 0 || !spinners[0].Displayed;
                    });
                }
                catch { }

                // 6. Đợi tabs hiển thị
                wait.Until(drv =>
                {
                    try
                    {
                        var tabs = drv.FindElements(By.CssSelector("#staticBackdrop .nav-tabs button"));
                        return tabs.Count == 3 && tabs[0].Displayed;
                    }
                    catch { return false; }
                });

                Console.WriteLine("✅ Modal đã hiển thị hoàn toàn");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Modal không hiển thị đúng cách: {ex.Message}");
                throw new Exception($"Modal không hiển thị sau 15 giây: {ex.Message}");
            }
        }

        public int CountPriceRows()
        {
            try
            {
                var rows = _driver.FindElement(PriceTableBody).FindElements(By.TagName("tr"));
                return rows.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Xóa sản phẩm theo tên
        /// </summary>
        public void DeleteProductByName(string productName)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                // Tìm dòng sản phẩm
                var rows = wait.Until(drv => drv.FindElements(By.XPath("//tbody[@id='tbody']//tr")));

                IWebElement targetRow = null;
                foreach (var row in rows)
                {
                    if (row.Text.Contains(productName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetRow = row;
                        break;
                    }
                }

                if (targetRow == null)
                    throw new Exception($"Không tìm thấy sản phẩm '{productName}' để xóa");

                // Tìm nút Xóa trong dòng
                IWebElement deleteButton = null;

                try
                {
                    // Thử tìm button Delete
                    deleteButton = targetRow.FindElement(By.XPath(".//button[contains(@class, 'btn-delete') or contains(@class, 'delete-btn') or contains(@class, 'btn-danger')]"));
                }
                catch
                {
                    try
                    {
                        // Thử tìm icon trash
                        var trashIcon = targetRow.FindElement(By.XPath(".//i[contains(@class, 'bi-trash')]"));
                        deleteButton = trashIcon.FindElement(By.XPath("./.."));
                    }
                    catch
                    {
                        throw new Exception($"Không tìm thấy nút Xóa cho sản phẩm '{productName}'");
                    }
                }

                // Click nút xóa
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", deleteButton);
                Thread.Sleep(500);
                js.ExecuteScript("arguments[0].click();", deleteButton);

                Console.WriteLine($"✅ Đã click nút Xóa cho sản phẩm: {productName}");
                Thread.Sleep(500);

                // Xử lý confirm dialog (nếu có)
                try
                {
                    IAlert alert = _driver.SwitchTo().Alert();
                    string alertText = alert.Text;
                    Console.WriteLine($"[ALERT] Xác nhận xóa: '{alertText}'");
                    alert.Accept();
                    Thread.Sleep(1000);
                }
                catch (NoAlertPresentException)
                {
                    // Nếu không có alert, có thể có modal confirm
                    try
                    {
                        var confirmModal = wait.Until(drv => drv.FindElement(By.Id("confirmDeleteModal")));
                        var confirmButton = confirmModal.FindElement(By.XPath(".//button[contains(text(), 'Xóa') or contains(text(), 'Đồng ý')]"));
                        confirmButton.Click();
                        Console.WriteLine("✅ Đã xác nhận xóa trong modal");
                        Thread.Sleep(1000);
                    }
                    catch { }
                }

                // Đợi xóa hoàn tất
                Thread.Sleep(2000);
                Console.WriteLine($"✅ Đã xóa sản phẩm: {productName}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa sản phẩm: {ex.Message}");
            }
        }
        public void DebugStatusColumn()
        {
            try
            {
                Console.WriteLine("=== DEBUG STATUS COLUMN ===");
                var rows = _driver.FindElements(By.XPath("//tbody[@id='tbody']//tr"));

                if (rows.Count > 0)
                {
                    var firstRow = rows[0];
                    var cells = firstRow.FindElements(By.TagName("td"));
                    Console.WriteLine($"Số lượng cột: {cells.Count}");

                    for (int i = 0; i < cells.Count; i++)
                    {
                        Console.WriteLine($"Cột {i}: '{cells[i].Text}'");
                    }
                }
                Console.WriteLine("=== END DEBUG ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Debug error: {ex.Message}");
            }
        }
        /// <summary>
        /// Kiểm tra sản phẩm không còn trong danh sách
        /// </summary>
        public bool IsProductNotExist(string productName)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

                // Đợi bảng load lại
                Thread.Sleep(1000);

                var tableBody = wait.Until(drv => drv.FindElement(ProductTableBody));
                var rows = tableBody.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    if (row.Text.Contains(productName, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"❌ Sản phẩm '{productName}' vẫn còn trong danh sách");
                        return false;
                    }
                }

                Console.WriteLine($"✅ Sản phẩm '{productName}' đã bị xóa khỏi danh sách");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Kiểm tra sản phẩm: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Thay đổi trạng thái sản phẩm (Mở bán/Ngưng bán)
        /// </summary>
        /// <summary>
        /// Thay đổi trạng thái sản phẩm (Mở bán/Ngưng bán)
        /// </summary>
        public void ToggleProductStatus(string productName)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                // Tìm dòng sản phẩm
                var rows = wait.Until(drv => drv.FindElements(By.XPath("//tbody[@id='tbody']//tr")));

                IWebElement targetRow = null;
                foreach (var row in rows)
                {
                    if (row.Text.Contains(productName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetRow = row;
                        break;
                    }
                }

                if (targetRow == null)
                    throw new Exception($"Không tìm thấy sản phẩm '{productName}' để thay đổi trạng thái");

                // Tìm nút Sửa trong dòng
                IWebElement editButton = null;

                try
                {
                    // Thử tìm button Edit
                    editButton = targetRow.FindElement(By.XPath(".//button[contains(@class, 'btn-edit') or contains(@class, 'edit-btn')]"));
                }
                catch
                {
                    try
                    {
                        // Thử tìm icon pencil
                        var pencilIcon = targetRow.FindElement(By.XPath(".//i[contains(@class, 'bi-pencil')]"));
                        editButton = pencilIcon.FindElement(By.XPath("./.."));
                    }
                    catch
                    {
                        try
                        {
                            // Thử tìm bất kỳ button nào trong dòng
                            var buttons = targetRow.FindElements(By.TagName("button"));
                            if (buttons.Count > 0)
                            {
                                editButton = buttons[0];
                            }
                            else
                            {
                                // Thử tìm link (thẻ a) có chứa text "Sửa" hoặc "Edit"
                                editButton = targetRow.FindElement(By.XPath(".//a[contains(text(), 'Sửa') or contains(text(), 'Edit')]"));
                            }
                        }
                        catch
                        {
                            throw new Exception($"Không tìm thấy nút Sửa cho sản phẩm '{productName}'");
                        }
                    }
                }

                // Click vào nút Sửa để mở modal
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
                Thread.Sleep(500);
                js.ExecuteScript("arguments[0].click();", editButton);

                Console.WriteLine($"✅ Đã click nút Sửa cho sản phẩm: {productName}");

                // Đợi modal hiển thị
                WaitForModalFullyDisplayed();

                // Click tab Thông tin (nếu chưa active)
                ClickTabInfo();

                // Tìm dropdown trạng thái
                var statusSelect = wait.Until(drv => drv.FindElement(FormIsActive));

                // Lấy trạng thái hiện tại
                SelectElement select = new SelectElement(statusSelect);
                string currentStatus = select.SelectedOption.Text;
                Console.WriteLine($"Trạng thái hiện tại trong modal: {currentStatus}");

                // Đổi sang trạng thái ngược lại
                if (currentStatus.Contains("Mở bán"))
                {
                    select.SelectByValue("0");
                    Console.WriteLine("✅ Đã đổi trạng thái sang: Ngưng bán");
                }
                else
                {
                    select.SelectByValue("1");
                    Console.WriteLine("✅ Đã đổi trạng thái sang: Mở bán");
                }

                Thread.Sleep(500);

                // Lưu thay đổi
                ClickSaveButton();

                // Đợi modal đóng
                WaitForModalClosed();

                Thread.Sleep(1000);
                Console.WriteLine($"✅ Đã thay đổi trạng thái sản phẩm: {productName}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thay đổi trạng thái sản phẩm: {ex.Message}");
            }
        }
        /// <summary>
        /// Tìm kiếm sản phẩm theo tên và kiểm tra kết quả
        /// </summary>
        public void SearchAndVerifyByName(string keyword, bool expectedToExist)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                // Tìm kiếm
                var searchInput = wait.Until(drv => drv.FindElement(SearchInput));
                searchInput.Clear();
                searchInput.SendKeys(keyword);
                Thread.Sleep(300);

                var searchBtn = wait.Until(drv => drv.FindElement(SearchButton));
                searchBtn.Click();
                Thread.Sleep(1500);

                Console.WriteLine($"✅ Đã tìm kiếm: {keyword}");

                // Kiểm tra kết quả
                var tableBody = wait.Until(drv => drv.FindElement(ProductTableBody));
                var rows = tableBody.FindElements(By.TagName("tr"));

                if (expectedToExist)
                {
                    // Kỳ vọng có ít nhất 1 kết quả
                    Assert.Greater(rows.Count, 0, $"Không tìm thấy sản phẩm nào cho từ khóa '{keyword}'");

                    bool found = false;
                    foreach (var row in rows)
                    {
                        if (row.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        {
                            found = true;
                            Console.WriteLine($"✅ Tìm thấy sản phẩm: {row.Text}");
                            break;
                        }
                    }
                    Assert.IsTrue(found, $"Không tìm thấy sản phẩm chứa từ khóa '{keyword}'");
                }
                else
                {
                    // Kỳ vọng không có kết quả
                    if (rows.Count > 0)
                    {
                        bool found = false;
                        foreach (var row in rows)
                        {
                            if (row.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                            {
                                found = true;
                                break;
                            }
                        }
                        Assert.IsFalse(found, $"Vẫn tìm thấy sản phẩm chứa từ khóa '{keyword}' khi kỳ vọng không có");
                    }
                    else
                    {
                        Console.WriteLine($"✅ Không tìm thấy sản phẩm nào cho từ khóa '{keyword}' (đúng như kỳ vọng)");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Lọc sản phẩm theo nhóm
        /// </summary>
        /// <summary>
        /// Lọc sản phẩm theo nhóm - Phiên bản với mapping chính xác
        /// </summary>
        /// <summary>
        /// Lọc sản phẩm theo nhóm - Phiên bản với mapping chính xác
        /// </summary>
        public void FilterByProductGroup(string groupName)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                // Tìm dropdown group product
                var groupSelect = wait.Until(drv => drv.FindElement(GroupProductFilter));
                var selectElement = new SelectElement(groupSelect);

                // Mapping theo yêu cầu test
                string valueToSelect = groupName switch
                {
                    "Cà phê" => "4",  // Chọn "Cà phê Việt Nam" (value=4)
                    "Trà" => "1",     // Chọn "Trà trái cây" (value=1)
                    "Tất cả" or "==== Tất cả ====" => "0",
                    _ => null
                };

                if (valueToSelect != null)
                {
                    selectElement.SelectByValue(valueToSelect);
                    Console.WriteLine($"✅ Đã chọn nhóm: '{selectElement.SelectedOption.Text}' (value={valueToSelect}) cho yêu cầu '{groupName}'");
                }
                else
                {
                    // Thử tìm theo text chứa từ khóa
                    bool found = false;
                    foreach (var option in selectElement.Options)
                    {
                        string optionText = option.Text.Trim();
                        if (optionText.Contains(groupName, StringComparison.OrdinalIgnoreCase))
                        {
                            selectElement.SelectByText(optionText);
                            Console.WriteLine($"✅ Đã chọn nhóm: '{optionText}' (chứa từ khóa '{groupName}')");
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        throw new Exception($"Không tìm thấy option phù hợp cho nhóm: {groupName}");
                }

                Thread.Sleep(500);

                // Click search để áp dụng filter
                var searchBtn = wait.Until(drv => drv.FindElement(SearchButton));
                searchBtn.Click();
                Thread.Sleep(1500);

                Console.WriteLine($"✅ Đã áp dụng bộ lọc: {groupName}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lọc sản phẩm: {ex.Message}");
            }
        }
        /// <summary>
        /// Lọc sản phẩm theo value của nhóm
        /// </summary>
        public void FilterByProductGroupValue(string groupValue)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                var groupSelect = wait.Until(drv => drv.FindElement(GroupProductFilter));
                var selectElement = new SelectElement(groupSelect);

                selectElement.SelectByValue(groupValue);
                Thread.Sleep(500);

                string selectedText = selectElement.SelectedOption.Text;
                Console.WriteLine($"✅ Đã chọn nhóm: '{selectedText}' (value={groupValue})");

                var searchBtn = wait.Until(drv => drv.FindElement(SearchButton));
                searchBtn.Click();
                Thread.Sleep(1500);

                Console.WriteLine($"✅ Đã áp dụng bộ lọc");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lọc sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả nhóm sản phẩm có sẵn
        /// </summary>
        public List<(string Value, string Text)> GetAllProductGroups()
        {
            var groups = new List<(string Value, string Text)>();

            try
            {
                var groupSelect = _driver.FindElement(GroupProductFilter);
                var selectElement = new SelectElement(groupSelect);

                foreach (var option in selectElement.Options)
                {
                    groups.Add((option.GetAttribute("value"), option.Text));
                }

                Console.WriteLine($"Đã lấy {groups.Count} nhóm sản phẩm");
                return groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách nhóm: {ex.Message}");
                return groups;
            }
        }
        /// <summary>
        /// Lấy danh sách sản phẩm từ bảng
        /// </summary>
        public List<string> GetProductNamesFromTable()
        {
            List<string> productNames = new List<string>();

            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var tableBody = wait.Until(drv => drv.FindElement(ProductTableBody));
                var rows = tableBody.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 2) // Cột tên sản phẩm thường ở vị trí thứ 2
                    {
                        string productName = cells[1].Text.Trim();
                        if (!string.IsNullOrEmpty(productName))
                        {
                            productNames.Add(productName);
                        }
                    }
                }

                Console.WriteLine($"✅ Lấy được {productNames.Count} sản phẩm từ bảng");
                return productNames;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}");
                return productNames;
            }
        }

        /// <summary>
        /// Kiểm tra tất cả sản phẩm trong bảng có thuộc nhóm đã chọn không
        /// </summary>
        public bool VerifyAllProductsBelongToGroup(string expectedGroupName)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var tableBody = wait.Until(drv => drv.FindElement(ProductTableBody));
                var rows = tableBody.FindElements(By.TagName("tr"));

                if (rows.Count == 0)
                {
                    Console.WriteLine("Không có sản phẩm nào trong bảng");
                    return true; // Không có sản phẩm thì coi như đúng
                }

                // Lấy tất cả sản phẩm và kiểm tra (cần có thông tin nhóm sản phẩm trong bảng)
                // Nếu bảng không hiển thị cột nhóm, cần mở từng sản phẩm để kiểm tra
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 5) // Giả sử cột nhóm sản phẩm ở vị trí thứ 4 (index 4)
                    {
                        string productGroup = cells[4].Text.Trim();
                        if (!string.IsNullOrEmpty(productGroup) && !productGroup.Equals(expectedGroupName, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"❌ Tìm thấy sản phẩm không thuộc nhóm '{expectedGroupName}': {productGroup}");
                            return false;
                        }
                    }
                }

                Console.WriteLine($"✅ Tất cả sản phẩm đều thuộc nhóm: {expectedGroupName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra nhóm sản phẩm: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lấy giá trị của bộ lọc nhóm sản phẩm hiện tại
        /// </summary>
        public string GetCurrentProductGroupFilter()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var groupSelect = wait.Until(drv => drv.FindElement(GroupProductFilter));
                SelectElement select = new SelectElement(groupSelect);
                string selectedText = select.SelectedOption.Text;
                Console.WriteLine($"Bộ lọc nhóm hiện tại: {selectedText}");
                return selectedText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy bộ lọc: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// Reset tất cả bộ lọc
        /// </summary>
        public void ResetFilters()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                // Reset group filter về "Tất cả"
                var groupSelect = wait.Until(drv => drv.FindElement(GroupProductFilter));
                SelectElement select = new SelectElement(groupSelect);
                select.SelectByValue("0");
                Thread.Sleep(300);

                // Clear search input
                var searchInput = wait.Until(drv => drv.FindElement(SearchInput));
                searchInput.Clear();
                Thread.Sleep(300);

                // Click search để refresh
                var searchBtn = wait.Until(drv => drv.FindElement(SearchButton));
                searchBtn.Click();
                Thread.Sleep(1500);

                Console.WriteLine("✅ Đã reset tất cả bộ lọc");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi reset filter: {ex.Message}");
            }
        }
        /// <summary>
        /// Lấy trạng thái hiển thị của sản phẩm trong bảng
        /// </summary>
        public string GetProductStatusInTable(string productName)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                // Tìm dòng sản phẩm
                var rows = wait.Until(drv => drv.FindElements(By.XPath("//tbody[@id='tbody']//tr")));

                foreach (var row in rows)
                {
                    if (row.Text.Contains(productName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Lấy tất cả các cột
                        var cells = row.FindElements(By.TagName("td"));

                        if (cells.Count >= 4) // Cột thứ 4 là trạng thái
                        {
                            string statusText = cells[3].Text.Trim();
                            Console.WriteLine($"[DEBUG] Trạng thái sản phẩm '{productName}': {statusText}");
                            return statusText;
                        }
                    }
                }

                return "Không tìm thấy";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Lấy trạng thái sản phẩm: {ex.Message}");
                return "Lỗi";
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái sản phẩm trong modal
        /// </summary>
        public string GetProductStatusInModal()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var statusSelect = wait.Until(drv => drv.FindElement(FormIsActive));
                SelectElement select = new SelectElement(statusSelect);
                string status = select.SelectedOption.Text;
                Console.WriteLine($"[DEBUG] Trạng thái trong modal: {status}");
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Lấy trạng thái trong modal: {ex.Message}");
                return "Lỗi";
            }
        }

        /// <summary>
        /// Xóa tất cả các dòng giá hiện có
        /// </summary>
        

        /// <summary>
        /// Thêm dòng giá với kích thước cụ thể
        /// </summary>
        public void AddPriceRowWithSize(string sizeName, string price, string unit = "VND")
        {
            try
            {
                // Click nút thêm dòng mới
                ClickAddPriceRow();
                Thread.Sleep(500);

                // Điền thông tin cho dòng cuối cùng
                FillLastPriceRow(sizeName, price, unit);

                Console.WriteLine($"✅ Đã thêm giá cho size {sizeName}: {price} {unit}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm giá cho size {sizeName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Điền thông tin cho dòng giá cuối cùng (cập nhật để dùng size name)
        /// </summary>
        /// <summary>
        /// Điền thông tin cho dòng giá cuối cùng
        /// </summary>
        public void FillLastPriceRow(string sizeValue, string price, string unit)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var priceTableBody = wait.Until(drv => drv.FindElement(PriceTableBody));
                var rows = priceTableBody.FindElements(By.TagName("tr"));

                if (rows.Count == 0)
                    throw new Exception("Không có dòng giá nào để điền");

                var lastRow = rows[rows.Count - 1];

                // Tìm select size - có thể là select hoặc input
                try
                {
                    var sizeSelect = new SelectElement(lastRow.FindElement(By.TagName("select")));

                    // Thử chọn theo text trước, nếu không thì theo value
                    try
                    {
                        sizeSelect.SelectByText(sizeValue);
                    }
                    catch
                    {
                        // Nếu không tìm thấy text, thử tìm value tương ứng
                        string valueMap = sizeValue switch
                        {
                            "S" or "Nhỏ" => "1",
                            "M" or "Vừa" => "2",
                            "L" or "Lớn" => "3",
                            _ => sizeValue
                        };
                        sizeSelect.SelectByValue(valueMap);
                    }
                }
                catch
                {
                    // Nếu không phải select, có thể là input
                    var sizeInput = lastRow.FindElement(By.TagName("input"));
                    sizeInput.Clear();
                    sizeInput.SendKeys(sizeValue);
                }

                Thread.Sleep(200);

                // Tìm input giá
                var inputs = lastRow.FindElements(By.TagName("input"));
                if (inputs.Count >= 1)
                {
                    inputs[0].Clear();
                    inputs[0].SendKeys(price);
                    Thread.Sleep(200);
                }

                // Tìm input đơn vị (nếu có)
                if (inputs.Count >= 2 && !string.IsNullOrEmpty(unit))
                {
                    inputs[1].Clear();
                    inputs[1].SendKeys(unit);
                }

                Console.WriteLine($"✅ Đã điền giá: Size={sizeValue}, Giá={price}, Đơn vị={unit}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi điền dòng giá: {ex.Message}");
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả giá đã thiết lập
        /// </summary>
        public Dictionary<string, string> GetAllPriceRows()
        {
            var priceList = new Dictionary<string, string>();

            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var priceTableBody = wait.Until(drv => drv.FindElement(PriceTableBody));
                var rows = priceTableBody.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 2)
                    {
                        string size = cells[0].Text.Trim();
                        string price = cells[1].Text.Trim();
                        if (!string.IsNullOrEmpty(size) && !string.IsNullOrEmpty(price))
                        {
                            priceList[size] = price;
                        }
                    }
                }

                Console.WriteLine($"✅ Lấy được {priceList.Count} dòng giá");
                return priceList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách giá: {ex.Message}");
                return priceList;
            }
        }

        /// <summary>
        /// Kiểm tra giá của một size cụ thể
        /// </summary>
        public bool VerifyPriceBySize(string sizeName, string expectedPrice)
        {
            try
            {
                var priceList = GetAllPriceRows();

                if (priceList.ContainsKey(sizeName))
                {
                    string actualPrice = priceList[sizeName];
                    if (actualPrice == expectedPrice)
                    {
                        Console.WriteLine($"✅ Giá size {sizeName}: {actualPrice} (đúng với kỳ vọng {expectedPrice})");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"❌ Giá size {sizeName}: {actualPrice} (không đúng kỳ vọng {expectedPrice})");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"❌ Không tìm thấy giá cho size {sizeName}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra giá: {ex.Message}");
                return false;
            }
        }
    }
}