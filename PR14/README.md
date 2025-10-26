# Практическая работа №14
## Тема: Автоматизация сценария входа на сайт
### Вариант: 3

**Задание:** Проверить отображение поля «Username» на странице входа.

---

### Ход работы

1. Создан проект типа `NUnit Test Project (.NET 6)`.
2. Установлены NuGet-пакеты:
   - `Selenium.WebDriver`
   - `Selenium.WebDriver.ChromeDriver`
   - `Selenium.Support`
3. Реализована структура проекта по принципам Page Object Model:
   - `/Drivers` — инициализация WebDriver.
   - `/Pages` — описание элементов страницы логина.
   - `/Tests` — тесты.
   - `/Utils` — вспомогательные методы (скриншоты).
4. Написан тест `CheckUsernameFieldIsDisplayed`, который:
   - Открывает страницу `https://example.com/login`
   - Проверяет, что поле Username отображается
   - Делает скриншот результата.

---

### Пример кода

```csharp
IWebElement usernameField = driver.FindElement(By.Id("username"));
Assert.IsTrue(usernameField.Displayed, "Поле Username не отображается");

## Drivers/WebDriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LoginAutomation.Drivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver Create()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            return new ChromeDriver(options);
        }
    }
}
##  Pages/LoginPage.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace LoginAutomation.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // Элементы страницы
        public IWebElement UsernameField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("username")));
        public IWebElement PasswordField => _driver.FindElement(By.Id("password"));
        public IWebElement LoginButton => _driver.FindElement(By.Id("loginBtn"));
    }
}
## Utils/ScreenshotHelper.cs
using OpenQA.Selenium;
using System.IO;

namespace LoginAutomation.Utils
{
    public static class ScreenshotHelper
    {
        public static void TakeScreenshot(IWebDriver driver, string fileName)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            Directory.CreateDirectory("images");
            screenshot.SaveAsFile($"images/{fileName}", ScreenshotImageFormat.Png);
        }
    }
}
## Tests/UsernameFieldTest.cs
using NUnit.Framework;
using OpenQA.Selenium;
using LoginAutomation.Drivers;
using LoginAutomation.Pages;
using LoginAutomation.Utils;

namespace LoginAutomation.Tests
{
    [TestFixture]
    public class UsernameFieldTest
    {
        private IWebDriver _driver;
        private LoginPage _loginPage;

        [SetUp]
        public void Setup()
        {
            _driver = WebDriverFactory.Create();
            _driver.Navigate().GoToUrl("https://example.com/login");
            _loginPage = new LoginPage(_driver);
        }

        [Test]
        public void CheckUsernameFieldIsDisplayed()
        {
            Assert.IsTrue(_loginPage.UsernameField.Displayed, "Поле Username не отображается на странице");

            ScreenshotHelper.TakeScreenshot(_driver, "username_field_result.png");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
