# Практическая работа №15
## Тема: Обработка динамических элементов с помощью WebDriverWait
### Вариант: 4

**Задание:**  
Дождаться появления кнопки «Start», кликнуть её и проверить появление текста «Completed».

---

### Ход работы

1. Создан проект `NUnit Test Project (.NET 6)`.
2. Установлены NuGet-пакеты:
   - `Selenium.WebDriver`
   - `Selenium.WebDriver.ChromeDriver`
   - `Selenium.Support`
3. Создан класс страницы `DynamicPage` с реализацией ожиданий `WebDriverWait`.
4. Написан тест `WaitForStartButtonAndCheckCompletedText`, который:
   - Открывает страницу `https://example.com/dynamic`
   - Ждёт, пока кнопка `Start` станет кликабельной
   - Нажимает её
   - Ожидает появления текста `Completed`
   - Делает скриншот результата

---

### Пример кода

```csharp
WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
IWebElement startButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("startBtn")));
startButton.Click();

IWebElement resultText = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("result")));
Assert.AreEqual("Completed", resultText.Text);

## Drivers/WebDriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DynamicWaitTests.Drivers
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
## Pages/DynamicPage.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DynamicWaitTests.Pages
{
    public class DynamicPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public DynamicPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // Элементы страницы
        private IWebElement StartButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("startBtn")));
        private IWebElement ResultText => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("result")));

        // Методы
        public void ClickStartButton() => StartButton.Click();

        public string GetResultText() => ResultText.Text;
    }
}
## Utils/ScreenshotHelper.cs
using OpenQA.Selenium;
using System.IO;

namespace DynamicWaitTests.Utils
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
## Tests/DynamicWaitTest.cs
using NUnit.Framework;
using OpenQA.Selenium;
using DynamicWaitTests.Drivers;
using DynamicWaitTests.Pages;
using DynamicWaitTests.Utils;

namespace DynamicWaitTests.Tests
{
    [TestFixture]
    public class DynamicWaitTest
    {
        private IWebDriver _driver;
        private DynamicPage _dynamicPage;

        [SetUp]
        public void Setup()
        {
            _driver = WebDriverFactory.Create();
            _driver.Navigate().GoToUrl("https://example.com/dynamic");
            _dynamicPage = new DynamicPage(_driver);
        }

        [Test]
        public void WaitForStartButtonAndCheckCompletedText()
        {
            _dynamicPage.ClickStartButton();

            string result = _dynamicPage.GetResultText();
            Assert.AreEqual("Completed", result, "Текст после загрузки не совпадает с ожидаемым");

            ScreenshotHelper.TakeScreenshot(_driver, "wait_test_result.png");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
