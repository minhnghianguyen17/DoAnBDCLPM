using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumNUnitExcelAutomation.DataProviders;
using SeleniumNUnitExcelAutomation.Pages;
using System.Threading;

namespace SeleniumNUnitExcelAutomation.Tests
{
    [TestFixture]
    public class AccountDetailsTest : BaseTest
    {
        private AccountDetailsPage _accountPage; // Dùng AccountDetailsPage đúng
        private LoginPage _loginPage;
        private JsonDataProvider _jsonDataProvider;

        [SetUp]
        public void Setup()
        {
            _accountPage = new AccountDetailsPage(Driver, Config, ExcelProvider);
            _loginPage = new LoginPage(Driver, Config, ExcelProvider);
            _jsonDataProvider = new JsonDataProvider(Config);
        }

        [Test]
        public void TC64_AccountDetails_DisplayAllFields()
        {
            string testCaseId = "TC64";
            try
            {
                // ===== 1. Vào trang và login =====
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2); // account id 2
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // ===== 2. Click icon account (bi-person-fill) -> chọn "Cá nhân" =====
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);

                // ===== 3. Chạy TC64 check hiển thị các field =====
                _accountPage.ExecuteTC64_AccountDetailsDisplay(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }



        [Test]
        public void TC66_AccountDetails_UploadAvatar_Workflow()
        {
            string testCaseId = "TC66";
            try
            {
                // 1. Vào trang và login (dùng account id 2)
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);

                // 3. Upload avatar
                _accountPage.ExecuteTC66_UploadAvatar(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        [Test]
        public void TC67_AccountDetails_CancelUploadAvatar()
        {
            string testCaseId = "TC67";
            try
            {
                // 1. Vào trang và login
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);

                // 3. Click upload nhưng không chọn file
                _accountPage.ExecuteTC67_CancelUploadAvatar(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }



        [Test]
        public void TC68_Account()
        {
            string testCaseId = "TC68";
            try
            {
                // 1. Vào trang và login
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);

               
                _accountPage.ExecuteTC68_UpdateAccountInfo(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        [Test]
        public void TC69_AccountEmptyName()
        {
            string testCaseId = "TC69";
            try
            {
                // 1. Vào trang và login
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);


                _accountPage.ExecuteTC69_EmptyFullNameUpdate(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }

        [Test]
        public void TC75_AccountEmptyNumber()
        {
            string testCaseId = "TC75";
            try
            {
                // 1. Vào trang và login
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);


                _accountPage.ExecuteTC75_EmptyPhoneUpdate(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        [Test]
        public void TC80_AccountEmptyAddress()
        {
            string testCaseId = "TC80";
            try
            {
                // 1. Vào trang và login
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);


                _accountPage.ExecuteTC80_EmptyAddressUpdate(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }



        [Test]
        public void TC83_AccountPass()
        {
            string testCaseId = "TC83";
            try
            {
                // 1. Vào trang và login
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);


                _accountPage.ExecuteTC83_NavigateToPasswordTab(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }



        [Test]
        public void TC84_AccountPass()
        {
            string testCaseId = "TC84";
            try
            {
                // 1. Vào trang và login
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);


                _accountPage.ExecuteTC84_ChangePassword(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }


        [Test]
        public void TC85_AccountPassEmpty()
        {
            string testCaseId = "TC85";
            try
            {
                // 1. Vào trang và login
                Driver.Navigate().GoToUrl("https://localhost:7116/");
                Thread.Sleep(2000);

                var account = _jsonDataProvider.GetAccountById(2);
                _loginPage.LoginWithAccount(account);
                Thread.Sleep(3000);

                // 2. Click icon account -> chọn "Cá nhân"
                var js = (IJavaScriptExecutor)Driver;
                var accountIcon = Driver.FindElement(By.CssSelector("i.bi-person-fill.dropdown-toggle"));
                js.ExecuteScript("arguments[0].click();", accountIcon);
                Thread.Sleep(1000);

                var accountDetailsLink = Driver.FindElement(By.CssSelector("a.dropdown-item[href='/AccountDetails']"));
                js.ExecuteScript("arguments[0].click();", accountDetailsLink);
                Thread.Sleep(3000);


                _accountPage.ExecuteTC85_WrongOldPassword(testCaseId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"[{testCaseId}] Thất bại: {ex.Message}");
            }
        }
    }
}