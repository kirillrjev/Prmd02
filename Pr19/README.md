# Практическая работа №19
## Тема: Рефакторинг тестов по паттерну POM
### Вариант: 4

**Задание:** Создать Page Object класс для страницы входа и рефакторить существующие тесты, используя методы Page Object.

**Пример кода (LoginPage.cs):**
```csharp
private IWebElement UsernameField => _driver.FindElement(By.Id("username"));
public void EnterUsername(string username) => UsernameField.SendKeys(username);
## LoginPage.cs
using OpenQA.Selenium;

namespace POMTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Локаторы
        private IWebElement UsernameField => _driver.FindElement(By.Id("username"));
        private IWebElement PasswordField => _driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => _driver.FindElement(By.Id("loginBtn"));
        private IWebElement ErrorMessage => _driver.FindElement(By.Id("errorMsg"));

        // Методы
        public void EnterUsername(string username) => UsernameField.SendKeys(username);
        public void EnterPassword(string password) => PasswordField.SendKeys(password);
        public void ClickLogin() => LoginButton.Click();
        public string GetErrorMessage() => ErrorMessage.Text;
    }
}
## LoginTests.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using POMTests.Pages;
using Xunit;

namespace POMTests.Tests
{
    public class LoginTests : IDisposable
    {
        private IWebDriver _driver;
        private LoginPage _loginPage;

        public LoginTests()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl("https://example.com/login");
            _loginPage = new LoginPage(_driver);
        }

        [Fact]
        public void InvalidLoginShowsError()
        {
            _loginPage.EnterUsername("wrongUser");
            _loginPage.EnterPassword("wrongPass");
            _loginPage.ClickLogin();

            string error = _loginPage.GetErrorMessage();
            Assert.Equal("Invalid username or password", error);
        }

        [Fact]
        public void ValidLoginRedirectsToDashboard()
        {
            _loginPage.EnterUsername("admin");
            _loginPage.EnterPassword("admin123");
            _loginPage.ClickLogin();

            Assert.Contains("dashboard", _driver.Url);
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}
## 