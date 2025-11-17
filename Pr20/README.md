## BasePage.cs
using OpenQA.Selenium;

namespace BaseClasses.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void Click(By locator) => Driver.FindElement(locator).Click();

        public void EnterText(By locator, string text)
        {
            var element = Driver.FindElement(locator);
            element.Clear();
            element.SendKeys(text);
        }

        public string GetText(By locator) => Driver.FindElement(locator).Text;

        public bool IsDisplayed(By locator) => Driver.FindElement(locator).Displayed;
    }
}
## LoginPage.cs (наследование BasePage)
using OpenQA.Selenium;

namespace BaseClasses.Pages
{
    public class LoginPage : BasePage
    {
        private By UsernameField => By.Id("username");
        private By PasswordField => By.Id("password");
        private By LoginButton => By.Id("loginBtn");
        private By ErrorMessage => By.Id("errorMsg");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void EnterUsername(string username) => EnterText(UsernameField, username);
        public void EnterPassword(string password) => EnterText(PasswordField, password);
        public void ClickLogin() => Click(LoginButton);
        public string GetErrorMessage() => GetText(ErrorMessage);
    }
}
## BaseTest.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace BaseClasses.Tests
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver Driver;

        public BaseTest()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void Dispose()
        {
            Driver.Quit();
        }
    }
}
## LoginTests.cs
using BaseClasses.Pages;
using Xunit;

namespace BaseClasses.Tests
{
    public class LoginTests : BaseTest
    {
        [Fact]
        public void InvalidLoginShowsError()
        {
            Driver.Navigate().GoToUrl("https://example.com/login");

            var loginPage = new LoginPage(Driver);
            loginPage.EnterUsername("wrongUser");
            loginPage.EnterPassword("wrongPass");
            loginPage.ClickLogin();

            string error = loginPage.GetErrorMessage();

            Assert.Equal("Invalid username or password", error);
        }

        [Fact]
        public void ValidLoginRedirectsToDashboard()
        {
            Driver.Navigate().GoToUrl("https://example.com/login");

            var loginPage = new LoginPage(Driver);
            loginPage.EnterUsername("admin");
            loginPage.EnterPassword("admin123");
            loginPage.ClickLogin();

            Assert.Contains("dashboard", Driver.Url);
        }
    }
}
# Практическая работа №20
## Тема: Создание базового класса (BasePage и BaseTest)
### Вариант: 4

### Цель:
Создать базовые классы для автоматизации тестирования (BasePage и BaseTest), реализовать LoginPage через Page Object Model, написать тесты на корректный и некорректный вход.

---

## Структура проекта

- **BasePage.cs** – базовые методы для страниц (Click, EnterText, GetText).
- **BaseTest.cs** – инициализация и завершение WebDriver.
- **LoginPage.cs** – Page Object для страницы логина.
- **LoginTests.cs** – тесты на вход в систему.

---

## Основные функции BasePage:
```csharp
public void Click(By locator) => Driver.FindElement(locator).Click();
public void EnterText(By locator, string text) => Driver.FindElement(locator).SendKeys(text);
public string GetText(By locator) => Driver.FindElement(locator).Text;
