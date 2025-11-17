## DriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace ComplexUITests.Drivers
{
    public static class DriverFactory
    {
        public static IWebDriver GetDriver(string browser)
        {
            IWebDriver driver;

            switch (browser.ToLower())
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;

                case "firefox":
                    driver = new FirefoxDriver();
                    break;

                default:
                    throw new ArgumentException($"Браузер {browser} не поддерживается");
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return driver;
        }
    }
}
## BasePage.cs 
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace ComplexUITests.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;
        private WebDriverWait Wait;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement Find(By locator)
        {
            Wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return Driver.FindElement(locator);
        }

        public void Click(By locator) => Find(locator).Click();

        public void EnterText(By locator, string text)
        {
            var element = Find(locator);
            element.Clear();
            element.SendKeys(text);
        }

        public string GetText(By locator) => Find(locator).Text;
    }
}
## LoginPage.cs 
using OpenQA.Selenium;

namespace ComplexUITests.Pages
{
    public class LoginPage : BasePage
    {
        private By UsernameField => By.Id("username");
        private By PasswordField => By.Id("password");
        private By LoginButton => By.Id("loginBtn");
        private By ErrorMessage  => By.Id("errorMsg");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void Login(string username, string password)
        {
            EnterText(UsernameField, username);
            EnterText(PasswordField, password);
            Click(LoginButton);
        }

        public string GetErrorMessage() => GetText(ErrorMessage);
    }
}
## DashboardPage.cs 
using OpenQA.Selenium;

namespace ComplexUITests.Pages
{
    public class DashboardPage : BasePage
    {
        private By WelcomeMessage => By.Id("welcomeText");
        private By SettingsLink   => By.Id("settingsLink");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public string GetWelcomeMessage() => GetText(WelcomeMessage);

        public void OpenSettings() => Click(SettingsLink);
    }
}
## SettingsPage.cs
using OpenQA.Selenium;

namespace ComplexUITests.Pages
{
    public class SettingsPage : BasePage
    {
        private By FrameLocator => By.Id("settingsFrame");
        private By SaveButton   => By.Id("saveBtn");
        private By SuccessAlert => By.Id("alertSuccess");

        public SettingsPage(IWebDriver driver) : base(driver) { }

        public void SwitchToFrame()
        {
            var frame = Driver.FindElement(FrameLocator);
            Driver.SwitchTo().Frame(frame);
        }

        public void SaveChanges()
        {
            Click(SaveButton);
        }

        public string GetAlertText()
        {
            Driver.SwitchTo().DefaultContent();
            return GetText(SuccessAlert);
        }
    }
}
## BaseTest.cs
using ComplexUITests.Drivers;
using OpenQA.Selenium;
using Xunit;
using System;

namespace ComplexUITests.Tests
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver Driver;

        public BaseTest(string browser)
        {
            Driver = DriverFactory.GetDriver(browser);
        }

        public void Dispose()
        {
            Driver.Quit();
        }
    }
}
## Комплексный тест ComplexScenarioTests.cs
using ComplexUITests.Pages;
using Xunit;

namespace ComplexUITests.Tests
{
    public class ComplexScenarioTests : BaseTest
    {
        public ComplexScenarioTests() : base("chrome") { }

        [Theory]
        [InlineData("chrome")]
        [InlineData("firefox")]
        public void FullScenario_Login_Navigate_SaveSettings(string browser)
        {
            Driver = Drivers.DriverFactory.GetDriver(browser);

            Driver.Navigate().GoToUrl("https://example-app.com/login");

            var loginPage = new LoginPage(Driver);
            loginPage.Login("admin", "admin123");

            var dashboardPage = new DashboardPage(Driver);
            Assert.Contains("Welcome", dashboardPage.GetWelcomeMessage());

            dashboardPage.OpenSettings();

            var settingsPage = new SettingsPage(Driver);
            settingsPage.SwitchToFrame();
            settingsPage.SaveChanges();

            string alertText = settingsPage.GetAlertText();

            Assert.Equal("Settings saved successfully!", alertText);

            Driver.Quit();
        }
    }
}
# Практическая работа №22
## Тема: Создание комплексного UI-теста с использованием POM и всех изученных техник
### Вариант: 7

---

## Цель работы
Создать комплексный автоматизированный UI-тест с использованием:
- Page Object Model
- BasePage / BaseTest
- WebDriverWait
- iframe
- alert
- кросс-браузерного тестирования
- нескольких страниц (Login → Dashboard → Settings)

---

## Структура проекта
ComplexUITests/
- Drivers/ — фабрика драйверов  
- Pages/ — классы страниц  
- Tests/ — комплексный тест  
- images/ — скриншоты  
- readme.md  

---

## Описание сценария

1. Открыть страницу логина.
2. Ввести логин + пароль.
3. Перейти на Dashboard.
4. Открыть Settings.
5. Переключиться в iframe.
6. Сохранить настройки.
7. Проверить появление уведомления.
8. Повторить в Chrome и Firefox.

---

## Пример теста

```csharp
[Theory]
[InlineData("chrome")]
[InlineData("firefox")]
public void FullScenario_Login_Navigate_SaveSettings(string browser)
{
    Driver = DriverFactory.GetDriver(browser);
    Driver.Navigate().GoToUrl("https://example-app.com/login");

    var login = new LoginPage(Driver);
    login.Login("admin", "admin123");

    var dashboard = new DashboardPage(Driver);
    dashboard.OpenSettings();

    var settings = new SettingsPage(Driver);
    settings.SwitchToFrame();
    settings.SaveChanges();

    Assert.Equal("Settings saved successfully!", settings.GetAlertText());
    Driver.Quit();
}
