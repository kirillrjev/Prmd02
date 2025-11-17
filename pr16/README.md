# Практическая работа №16
## Тема: Сравнение работы Implicit vs Explicit Waits
### Вариант: 3

**Задание:**  
Сравнить стабильность и время выполнения тестов с использованием Implicit и Explicit Wait при поиске динамического элемента.

---

## Ход работы

1. Создан NUnit-проект `WaitComparisonTests`.
2. Установлены NuGet-пакеты:
   - Selenium.WebDriver
   - Selenium.WebDriver.ChromeDriver
   - Selenium.Support
3. Настроен драйвер с `ImplicitWait` и `ExplicitWait`.
4. Написаны два теста:
   - `ImplicitWaitTest` — ждёт появление элемента глобально.
   - `ExplicitWaitTest` — ждёт кликабельность элемента локально через `ExpectedConditions`.
5. Сравнено время выполнения и стабильность.

---

## Код примера (Explicit Wait)
```csharp
WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
IWebElement dynamicElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dynamicElement")));
dynamicElement.Click();

## Drivers/WebDriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WaitComparisonTests.Drivers
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

namespace WaitComparisonTests.Pages
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

        public IWebElement GetDynamicElementImplicit() =>
            _driver.FindElement(By.Id("dynamicElement"));

        public IWebElement GetDynamicElementExplicit() =>
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("dynamicElement")));
    }
}
## Utils/ScreenshotHelper.cs
using OpenQA.Selenium;
using System.IO;

namespace WaitComparisonTests.Utils
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
## Tests/ImplicitWaitTest.cs
using NUnit.Framework;
using OpenQA.Selenium;
using WaitComparisonTests.Drivers;
using WaitComparisonTests.Pages;
using WaitComparisonTests.Utils;
using System.Diagnostics;

namespace WaitComparisonTests.Tests
{
    [TestFixture]
    public class ImplicitWaitTest
    {
        private IWebDriver _driver;
        private DynamicPage _page;

        [SetUp]
        public void Setup()
        {
            _driver = WebDriverFactory.Create();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); // Implicit Wait
            _driver.Navigate().GoToUrl("https://example.com/dynamic");
            _page = new DynamicPage(_driver);
        }

        [Test]
        public void TestImplicitWaitPerformance()
        {
            var stopwatch = Stopwatch.StartNew();

            var element = _page.GetDynamicElementImplicit();
            element.Click();

            stopwatch.Stop();
            ScreenshotHelper.TakeScreenshot(_driver, "implicit_result.png");

            TestContext.WriteLine($"⏱ Время выполнения (Implicit Wait): {stopwatch.ElapsedMilliseconds} мс");
            Assert.IsTrue(element.Displayed, "Элемент не отображается");
        }

        [TearDown]
        public void TearDown() => _driver.Quit();
    }
}
## Tests/ExplicitWaitTest.cs
using NUnit.Framework;
using OpenQA.Selenium;
using WaitComparisonTests.Drivers;
using WaitComparisonTests.Pages;
using WaitComparisonTests.Utils;
using System.Diagnostics;

namespace WaitComparisonTests.Tests
{
    [TestFixture]
    public class ExplicitWaitTest
    {
        private IWebDriver _driver;
        private DynamicPage _page;

        [SetUp]
        public void Setup()
        {
            _driver = WebDriverFactory.Create();
            _driver.Navigate().GoToUrl("https://example.com/dynamic");
            _page = new DynamicPage(_driver);
        }

        [Test]
        public void TestExplicitWaitPerformance()
        {
            var stopwatch = Stopwatch.StartNew();

            var element = _page.GetDynamicElementExplicit();
            element.Click();

            stopwatch.Stop();
            ScreenshotHelper.TakeScreenshot(_driver, "explicit_result.png");

            TestContext.WriteLine($"⏱ Время выполнения (Explicit Wait): {stopwatch.ElapsedMilliseconds} мс");
            Assert.IsTrue(element.Displayed, "Элемент не отображается");
        }

        [TearDown]
        public void TearDown() => _driver.Quit();
    }
}
