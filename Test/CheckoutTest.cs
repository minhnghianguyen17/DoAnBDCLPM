using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Pages;
using System.Threading;
using System.Linq;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class CheckoutTest : BaseTest
    {
        private CheckoutPage _checkoutPage;
        private LoginPage _loginPage;
        private JsonDataProvider _jsonDataProvider;

        [SetUp]
        public void Setup()
        {
            _checkoutPage = new CheckoutPage(Driver, Config, ExcelProvider);
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _jsonDataProvider = new JsonDataProvider(Config);
        }

        [Test]
        public void TC29_Checkout_EmptyPhone_Workflow()
        {
            string testCaseId = "TC29";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC29_EmptyPhoneFlow(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Test]
        public void TC30_Checkout_InvalidPhone_Workflow()
        {
            string testCaseId = "TC30";
            try
            {
                // 1. Vào trang và Đăng nhập
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng test SĐT 1234
                _checkoutPage.ExecuteTC30_InvalidPhone_OnlyStep2(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void ExecuteTC31_EmptyNameFlow()
        {
            string testCaseId = "TC31";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC31_EmptyNameFlow(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }



        [Test]
        public void ExecuteTC40_EmptyAddressFlow()
        {
            string testCaseId = "TC40";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC40_EmptyAddressFlow(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }



        [Test]
        public void ExecuteTC45_FullValidFlow()
        {
            string testCaseId = "TC45";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC45_FullValidFlow(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void ExecuteTC48_FullValidFlow()
        {
            string testCaseId = "TC48";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC48_VNPayFlow(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC47_VerifyCartTotalCalculationFlow()
        {
            string testCaseId = "TC47";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC47_CheckTotalMoney(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Test]
        public void TC51_DeleteAndCheck()
        {
            string testCaseId = "TC51";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC51_DeleteAndCheckTotal(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Test]
        public void TC53_EmtyCart()
        {
            string testCaseId = "TC53";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC53_EmptyCartCheckTotal(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Test]
        public void TC56_CheckEmptyCart()
        {
            string testCaseId = "TC56";
            try
            {
                // 1. Vào trang và Đăng nhập (Dùng account cứng id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);
                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Chạy luồng tự động nhập (Y chang Cart)
                _checkoutPage.ExecuteTC56_CheckEmptyCartMessage(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}