using NUnit.Framework;
using SeleniumNUnitExcelAutomation.Pages;
using SeleniumNUnitExcelAutomation.DataProviders;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class OrderHistoryTest : BaseTest
    {
        private OrderHistoryPage _orderHistoryPage;
        private LoginPage _loginPage;
        private JsonDataProvider _jsonDataProvider;

        [SetUp]
        public void Setup()
        {
            _orderHistoryPage = new OrderHistoryPage(Driver, Config, ExcelProvider);
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _jsonDataProvider = new JsonDataProvider(Config);
        }

        [Test]
        public void TC61_OrderHistory_DisplayAllFields()
        {
            string testCaseId = "TC61";
            // ===== 1. Login =====
            Driver.Navigate().GoToUrl("https://localhost:7116/");
            Thread.Sleep(2000);
            // 1. Login 
            var account = _jsonDataProvider.GetAccountById(2);
            _loginPage.LoginWithAccount(account);
            Thread.Sleep(2000);

            // 2. Thực hiện TC61
            _orderHistoryPage.ExecuteTC61_CheckOrderHistory(testCaseId);
        }

        [Test]
        public void TC62_OrderHistory()
        {
            string testCaseId = "TC62";
            // ===== 1. Login =====
            Driver.Navigate().GoToUrl("https://localhost:7116/");
            Thread.Sleep(2000);
            // 1. Login 
            var account = _jsonDataProvider.GetAccountById(2);
            _loginPage.LoginWithAccount(account);
            Thread.Sleep(2000);

            // 2. Thực hiện TC61
            _orderHistoryPage.ExecuteTC62_CheckDateRange(testCaseId);
        }
    }
}
