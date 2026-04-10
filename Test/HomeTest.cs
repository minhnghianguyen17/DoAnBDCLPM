using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Pages;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class HomeTest : BaseTest
    {
        private HomePage _homePage;
        private LoginPage _loginPage;
        private JsonDataProvider _jsonDataProvider;

        [SetUp]
        public void Setup()
        {
            _homePage = new HomePage(Driver, Config, ExcelProvider);
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _jsonDataProvider = new JsonDataProvider(Config);
        }

        [Test]
        public void TC1_HomePage_DisplayAllElements()
        {
            string testCaseId = "TC1";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC1_HomePageDisplay(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        [Test]
        public void TC2_HomePage_()
        {
            string testCaseId = "TC2";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC2_NavigateShopAndBack(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
        [Test]
        public void TC4_HomePageLink()
        {
            string testCaseId = "TC4";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC4_NavigateToShop(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }

        [Test]
        public void TC5_HomePageCoffee()
        {
            string testCaseId = "TC5";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC5_SelectVietnamCoffee(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        [Test]
        public void TC6_HomePageSlide()
        {
            string testCaseId = "TC6";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC6_ClickCarouselSlide(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        [Test]
        public void TC14_HomePage()
        {
            string testCaseId = "TC14";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC14_ClickFirstImage(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
        [Test]
        public void TC15_HomePage()
        {
            string testCaseId = "TC15";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC15_ClickNextPage(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
        [Test]
        public void TC17_HomePage()
        {
            string testCaseId = "TC17";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC17_ClickFirstImage(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }

        [Test]
        public void TC23_HomePage()
        {
            string testCaseId = "TC23";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC23_CheckProductPrice(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        [Test]
        public void TC8_HomePage()
        {
            string testCaseId = "TC8";
            try
            {
                // ===== 1. Login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Kiểm tra trang Home =====
                _homePage.ExecuteTC8_AddToCartAlert(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
    }
}