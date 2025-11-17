# Практическая работа №17
## Тема: Обработка алертов и всплывающих окон (popups) в Selenium с xUnit
### Вариант: 4

**Задание:**  
Обработать все типы алертов: Alert, Confirm и Prompt. Проверить тексты и результаты действий.

---

## 🔧 Ход работы

1. Создан проект **xUnit Test Project (.NET)**.
2. Установлены NuGet-пакеты:
   - `Selenium.WebDriver`
   - `Selenium.WebDriver.ChromeDriver`
   - `Selenium.Support`
3. Настроен WebDriver в классе `WebDriverFactory`.
4. Реализован класс страницы `AlertPage.cs` с кнопками алертов.
5. Написаны три теста в `AlertTests.cs`:
   - Accept простого Alert
   - Dismiss Confirm
   - Ввод текста и Accept Prompt
6. Выполнено снятие скриншотов после каждого теста.

---

## 🔍 Примеры кода

### Alert
```csharp
IAlert alert = driver.SwitchTo().Alert();
Assert.Equal("This is an alert", alert.Text);
alert.Accept();
## using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AlertHandlingTests.Drivers
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
## WebDriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AlertHandlingTests.Drivers
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
## AlertPage.cs
using OpenQA.Selenium;

namespace AlertHandlingTests.Pages
{
    public class AlertPage
    {
        private readonly IWebDriver _driver;

        public AlertPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement AlertButton => _driver.FindElement(By.Id("alertButton"));
        private IWebElement ConfirmButton => _driver.FindElement(By.Id("confirmButton"));
        private IWebElement PromptButton => _driver.FindElement(By.Id("promptButton"));
        private IWebElement PromptResult => _driver.FindElement(By.Id("promptResult"));

        public void OpenAlert() => AlertButton.Click();
        public void OpenConfirm() => ConfirmButton.Click();
        public void OpenPrompt() => PromptButton.Click();

        public string GetPromptResult() => PromptResult.Text;
    }
}
## AlertTests.cs
using Xunit;
using OpenQA.Selenium;
using AlertHandlingTests.Drivers;
using AlertHandlingTests.Pages;
using AlertHandlingTests.Utils;

namespace AlertHandlingTests.Tests
{
    public class AlertTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly AlertPage _alertPage;

        public AlertTests()
        {
            _driver = WebDriverFactory.Create();
            _driver.Navigate().GoToUrl("https://example.com/alerts");
            _alertPage = new AlertPage(_driver);
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        [Fact]
        public void HandleAlert()
        {
            _alertPage.OpenAlert();
            IAlert alert = _driver.SwitchTo().Alert();

            Assert.Equal("This is an alert", alert.Text);
            alert.Accept();

            ScreenshotHelper.TakeScreenshot(_driver, "alert_ok.png");
        }

        [Fact]
        public void HandleConfirmDismiss()
        {
            _alertPage.OpenConfirm();
            IAlert confirm = _driver.SwitchTo().Alert();

            Assert.Equal("Do you want to proceed?", confirm.Text);
            confirm.Dismiss();

            ScreenshotHelper.TakeScreenshot(_driver, "confirm_cancel.png");
        }

        [Fact]
        public void HandlePrompt()
        {
            _alertPage.OpenPrompt();
            IAlert prompt = _driver.SwitchTo().Alert();

            Assert.Equal("Please enter your name", prompt.Text);
            prompt.SendKeys("Test User");
            prompt.Accept();

            string result = _alertPage.GetPromptResult();
            Assert.Equal("Hello, Test User!", result);

            ScreenshotHelper.TakeScreenshot(_driver, "prompt_ok.png");
        }
    }
}
## ScreenshotHelper.cs
using OpenQA.Selenium;
using System.IO;

namespace AlertHandlingTests.Utils
{
    public static class ScreenshotHelper
    {
        public static void TakeScreenshot(IWebDriver driver, string fileName)
        {
            Directory.CreateDirectory("images");
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile($"images/{fileName}", ScreenshotImageFormat.Png);
        }
    }
}
