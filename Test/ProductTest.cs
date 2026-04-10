using NUnit.Framework;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Pages;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class ProductTest : BaseTest
    {
        private LoginPage _loginPage;
        private ProductPage _productPage;
        private JsonDataProvider _jsonDataProvider;

        [SetUp]
        public void Setup()
        {
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _productPage = new ProductPage(Driver, Config, ExcelProvider);
            _jsonDataProvider = new JsonDataProvider(Config);
        }

        [Test]
        public void TC28_Product_IncreaseQuantity()
        {
            string testCaseId = "TC28";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC28_IncreaseQuantity(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC29_Product()
        {
            string testCaseId = "TC29";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC29_AdjustQuantity(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC31_Product()
        {
            string testCaseId = "TC31";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC31_RelatedProductNavigation(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC32_Product()
        {
            string testCaseId = "TC32";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteT32_AddToCartAlert(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC37_Product()
        {
            string testCaseId = "TC37";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC37_IncreaseToLimit(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC39_Product()
        {
            string testCaseId = "TC39";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC39_CheckProductPrice(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC40_Product()
        {
            string testCaseId = "TC40";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC40_CheckProductName(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC41_Product()
        {
            string testCaseId = "TC41";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC41_CheckProductSizes(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC46_Product()
        {
            string testCaseId = "TC46";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC46_CheckPriceChangesBySize(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC45_Product()
        {
            string testCaseId = "TC45";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC45_AddMultipleProducts(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC47_Product()
        {
            string testCaseId = "TC47";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteT47_BannerRedirect(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC50_Product()
        {
            string testCaseId = "TC50";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC50_RatingProduct(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC51_Product()
        {
            string testCaseId = "TC51";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC51_Product_CheckOtherImage(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC53_Product()
        {
            string testCaseId = "TC53";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC53_IncreaseQuantityAndCheckPrice(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC55_Product()
        {
            string testCaseId = "TC55";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC55_IncreaseThenDecreaseCheckPrice(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC56_Product()
        {
            string testCaseId = "TC56";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC56_CheckProductSizesAndPrice(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC57_Product()
        {
            string testCaseId = "TC57";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC57_CheckPriceDecreaseOnSizeChange(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TC58_Product()
        {
            string testCaseId = "TC58";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Chạy hàm test TC27 =====
                _productPage.ExecuteTC58_CheckAlertWhenNoSizeSelected(testCaseId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


    }
}