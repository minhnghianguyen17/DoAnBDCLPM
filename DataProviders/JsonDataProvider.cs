using Newtonsoft.Json;
using SeleniumNUnitExcelAutomation.Models;
using SeleniumNUnitExcelAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeleniumNUnitExcelAutomation.DataProviders
{
    public class JsonDataProvider
    {
        private readonly ExcelConfig _config;

        public JsonDataProvider(ExcelConfig config)
        {
            _config = config;
        }

        // ===== ACCOUNT METHODS =====
        public List<AccountModel> LoadAllAccounts()
        {
            string fullPath = GetFullJsonPath("Accounts.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON Account: {fullPath}");

            string json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<List<AccountModel>>(json) ?? new List<AccountModel>();
        }

        public AccountModel GetValidAccount()
        {
            var accounts = LoadAllAccounts();
            return accounts.FirstOrDefault(a => a.IsActive == 1 && a.IsDelete == false);
        }

        public AccountModel GetAccountById(int id)
        {
            var accounts = LoadAllAccounts();
            return accounts.FirstOrDefault(a => a.ID == id);
        }

        public AccountModel GetAccountByEmail(string email)
        {
            var accounts = LoadAllAccounts();
            return accounts.FirstOrDefault(a =>
                a.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        // ===== REGISTER METHODS =====
        /// <summary>
        /// Đọc toàn bộ danh sách đăng ký từ Register.json
        /// </summary>
        public List<RegisterModel> LoadAllRegistrations()
        {
            string fullPath = GetFullJsonPath("Register.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON Register: {fullPath}");

            string json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<List<RegisterModel>>(json) ?? new List<RegisterModel>();
        }

        /// <summary>
        /// Lấy dữ liệu đăng ký theo ID
        /// </summary>
        public RegisterModel GetRegistrationById(int reId)
        {
            var registrations = LoadAllRegistrations();
            return registrations.FirstOrDefault(r => r.ReId == reId);
        }

        // ===== LOGOUT METHODS =====
        /// <summary>
        /// Đọc toàn bộ danh sách tài khoản đăng xuất từ Logout.json
        /// </summary>
        public List<LogoutModel> LoadAllLogouts()
        {
            string fullPath = GetFullJsonPath("Logout.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON Logout: {fullPath}");

            string json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<List<LogoutModel>>(json) ?? new List<LogoutModel>();
        }

        /// <summary>
        /// Lấy dữ liệu đăng xuất theo ID
        /// </summary>
        public LogoutModel GetLogoutById(int loId)
        {
            var logouts = LoadAllLogouts();
            return logouts.FirstOrDefault(l => l.LoID == loId);
        }

        // ===== ADMIN ACCOUNT METHODS =====
        /// <summary>
        /// Đọc toàn bộ danh sách tài khoản admin từ AdminAccount.json
        /// </summary>
        public List<AdminAccountModel> LoadAllAdminAccounts()
        {
            string fullPath = GetFullJsonPath("AdminAccount.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON AdminAccount: {fullPath}");

            string json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<List<AdminAccountModel>>(json) ?? new List<AdminAccountModel>();
        }

        /// <summary>
        /// Lấy dữ liệu admin account theo index (vị trí trong danh sách)
        /// </summary>
        public AdminAccountModel GetAdminAccountByIndex(int index)
        {
            var adminAccounts = LoadAllAdminAccounts();
            if (index >= 0 && index < adminAccounts.Count)
                return adminAccounts[index];
            return null;
        }

        /// <summary>
        /// Lấy dữ liệu admin account theo AccId
        /// </summary>
        public AdminAccountModel GetAdminAccountByAccId(int accId)
        {
            var adminAccounts = LoadAllAdminAccounts();
            return adminAccounts.FirstOrDefault(a => a.AccId == accId);
        }

        /// <summary>
        /// Lấy đường dẫn đầy đủ của file JSON
        /// </summary>
        private string GetFullJsonPath(string fileName)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(projectPath, "TestData", "Json", fileName);
        }

        // ===== ADMIN PRODUCT METHODS =====
        public List<AdminProductModel> LoadAllAdminProducts()
        {
            string fullPath = GetFullJsonPath("AdminProduct.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON AdminProduct: {fullPath}");

            string json = File.ReadAllText(fullPath);
            dynamic data = JsonConvert.DeserializeObject(json);
            var products = new List<AdminProductModel>();

            if (data != null && data["AdminProducts"] != null)
            {
                foreach (var item in data["AdminProducts"])
                {
                    // 🔥 Convert null to 0 hoặc default value
                    int productId = item["productId"] ?? 0;
                    int productTypeId = item["productTypeId"] ?? 0;
                    int isActive = item["isActive"] ?? 0;

                    products.Add(new AdminProductModel
                    {
                        ProductId = productId,
                        ProductCode = item["productCode"]?.ToString() ?? "",
                        ProductName = item["productName"]?.ToString() ?? "",
                        ProductTypeId = productTypeId,
                        Note = item["note"]?.ToString() ?? "",
                        IsActive = isActive
                    });
                }
            }

            return products;
        }

        public AdminProductModel GetAdminProductByIndex(int index)
        {
            var products = LoadAllAdminProducts();
            if (index >= 0 && index < products.Count)
                return products[index];
            return null;
        }

        public AdminProductModel GetAdminProductByName(string productName)
        {
            var products = LoadAllAdminProducts();
            return products.FirstOrDefault(p =>
                p.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase));
        }

        public AdminProductModel GetAdminProductByCode(string productCode)
        {
            var products = LoadAllAdminProducts();
            return products.FirstOrDefault(p =>
                p.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase));
        }

        // ===== ADMIN PRODUCT PRICE METHODS =====
        public List<AdminProductPriceModel> LoadAllAdminProductPrices()
        {
            string fullPath = GetFullJsonPath("AdminProduct.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON AdminProduct: {fullPath}");

            string json = File.ReadAllText(fullPath);
            dynamic data = JsonConvert.DeserializeObject(json);
            var prices = new List<AdminProductPriceModel>();

            if (data != null && data["AdminProducts"] != null)
            {
                int priceId = 0;
                foreach (var item in data["AdminProducts"])
                {
                    if (item["price"] != null)
                    {
                        priceId++;

                        // 🔥 Convert null to 0 hoặc default value
                        int productId = item["productId"] ?? 0;
                        int sizeId = item["price"]["sizeId"] ?? 0;
                        string priceValue = item["price"]["priceValue"]?.ToString() ?? "0";
                        string unit = item["price"]["unit"]?.ToString() ?? "VNĐ";

                        prices.Add(new AdminProductPriceModel
                        {
                            ProductPriceId = priceId,
                            ProductId = productId,
                            SizeId = sizeId,
                            Price = priceValue,
                            Unit = unit
                        });
                    }
                }
            }

            return prices;
        }

        public AdminProductPriceModel GetAdminProductPriceByIndex(int index)
        {
            var prices = LoadAllAdminProductPrices();
            if (index >= 0 && index < prices.Count)
                return prices[index];
            return null;
        }

        // ===== ADMIN TOPPING METHODS =====
        public List<AdminToppingModel> LoadAllAdminToppings()
        {
            string fullPath = GetFullJsonPath("AdminProduct.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON AdminProduct: {fullPath}");

            string json = File.ReadAllText(fullPath);
            dynamic data = JsonConvert.DeserializeObject(json);
            var toppings = new List<AdminToppingModel>();

            if (data != null && data["Toppings"] != null)
            {
                foreach (var item in data["Toppings"])
                {
                    // 🔥 Convert null to 0 hoặc default value
                    int toppingId = item["toppingId"] ?? 0;
                    int price = item["price"] ?? 0;
                    int isActive = item["isActive"] ?? 0;

                    toppings.Add(new AdminToppingModel
                    {
                        ToppingId = toppingId,
                        ToppingCode = item["toppingCode"]?.ToString() ?? "",
                        ToppingName = item["toppingName"]?.ToString() ?? "",
                        Price = price,
                        IsActive = isActive
                    });
                }
            }

            return toppings;
        }

        public AdminToppingModel GetAdminToppingByIndex(int index)
        {
            var toppings = LoadAllAdminToppings();
            if (index >= 0 && index < toppings.Count)
                return toppings[index];
            return null;
        }

        public AdminToppingModel GetAdminToppingByName(string toppingName)
        {
            var toppings = LoadAllAdminToppings();
            return toppings.FirstOrDefault(t =>
                t.ToppingName.Equals(toppingName, StringComparison.OrdinalIgnoreCase));
        }

        // ===== ADMIN PRODUCT TYPE METHODS =====
        public List<AdminProductTypeModel> LoadAllAdminProductTypes()
        {
            string fullPath = GetFullJsonPath("AdminProduct.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON AdminProduct: {fullPath}");

            string json = File.ReadAllText(fullPath);
            dynamic data = JsonConvert.DeserializeObject(json);
            var productTypes = new List<AdminProductTypeModel>();

            if (data != null && data["ProductTypes"] != null)
            {
                foreach (var item in data["ProductTypes"])
                {
                    // 🔥 Convert null to 0 hoặc default value
                    int productTypeId = item["productTypeId"] ?? 0;
                    int isActive = item["isActive"] ?? 0;

                    productTypes.Add(new AdminProductTypeModel
                    {
                        ProductTypeId = productTypeId,
                        ProductTypeCode = item["productTypeCode"]?.ToString() ?? "",
                        ProductTypeName = item["productTypeName"]?.ToString() ?? "",
                        IsActive = isActive
                    });
                }
            }

            return productTypes;
        }

        public AdminProductTypeModel GetAdminProductTypeByIndex(int index)
        {
            var types = LoadAllAdminProductTypes();
            if (index >= 0 && index < types.Count)
                return types[index];
            return null;
        }

        // ===== ADMIN SIZE METHODS =====
        public List<AdminSizeModel> LoadAllAdminSizes()
        {
            string fullPath = GetFullJsonPath("AdminProduct.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON AdminProduct: {fullPath}");

            string json = File.ReadAllText(fullPath);
            dynamic data = JsonConvert.DeserializeObject(json);
            var sizes = new List<AdminSizeModel>();

            if (data != null && data["Sizes"] != null)
            {
                foreach (var item in data["Sizes"])
                {
                    // 🔥 Convert null to 0 hoặc default value
                    int sizeId = item["sizeId"] ?? 0;
                    int isActive = item["isActive"] ?? 0;

                    sizes.Add(new AdminSizeModel
                    {
                        SizeId = sizeId,
                        SizeCode = item["sizeCode"]?.ToString() ?? "",
                        SizeName = item["sizeName"]?.ToString() ?? "",
                        IsActive = isActive
                    });
                }
            }

            return sizes;
        }

        public AdminSizeModel GetAdminSizeByIndex(int index)
        {
            var sizes = LoadAllAdminSizes();
            if (index >= 0 && index < sizes.Count)
                return sizes[index];
            return null;
        }

        public dynamic GetAdminProductTestCase(int index)
        {
            string fullPath = GetFullJsonPath("AdminProduct.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Không tìm thấy file AdminProduct.json");

            string json = File.ReadAllText(fullPath);
            dynamic data = JsonConvert.DeserializeObject(json);

            return data["Products"][index];
        }

        // ===== ADMIN PRODUCT EDIT DATA METHODS =====
        public dynamic GetAdminProductEditDataByIndex(int index)
        {
            string fullPath = GetFullJsonPath("AdminProduct.json");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file JSON AdminProduct: {fullPath}");

            string json = File.ReadAllText(fullPath);
            dynamic data = JsonConvert.DeserializeObject(json);

            if (data != null && data["AdminProductsForEdit"] != null)
            {
                var editDataList = data["AdminProductsForEdit"];
                if (index >= 0 && index < editDataList.Count)
                    return editDataList[index];
            }

            return null;
        }


    }


}