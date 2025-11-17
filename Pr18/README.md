# Практическая работа №18
## Тема: Работа с iframes в Selenium WebDriver
### Вариант: 4

**Задание:**  
Переключиться на iframe, ввести текст в поле внутри него, затем вернуться к основному контенту и проверить кнопку на главной странице.

---

## 🔧 Ход работы

1. Создан проект **xUnit Test Project (.NET)**.  
2. Установлены NuGet-пакеты:
   - `Selenium.WebDriver`
   - `Selenium.WebDriver.ChromeDriver`
   - `Selenium.Support`
3. Реализован класс `IframePage` с методами для работы с iframe.  
4. Написаны тесты для трёх способов переключения:
   - По имени
   - По индексу
   - По веб-элементу
5. Добавлен тест для вложенных iframe.
6. После каждого теста выполняется скриншот страницы.

---

## 🧪 Примеры кода

### Переключение по имени
```csharp
_driver.SwitchTo().Frame("frame1");
_driver.FindElement(By.Id("inputField")).SendKeys("Hello iframe");
_driver.SwitchTo().DefaultContent();
Assert.True(_driver.FindElement(By.Id("mainButton")).Displayed);
## WebDriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace IframeTests.Drivers
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
## IframePage.cs
using OpenQA.Selenium;

namespace IframeTests.Pages
{
    public class IframePage
    {
        private readonly IWebDriver _driver;

        public IframePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement InputField => _driver.FindElement(By.Id("inputField"));
        public IWebElement MainButton => _driver.FindElement(By.Id("mainButton"));
        public IWebElement IframeElement => _driver.FindElement(By.TagName("iframe"));

        public void SwitchToFrameByName(string frameName)
        {
            _driver.SwitchTo().Frame(frameName);
        }

        public void SwitchToFrameByIndex(int index)
        {
            _driver.SwitchTo().Frame(index);
        }

        public void SwitchToFrameByElement()
        {
            _driver.SwitchTo().Frame(IframeElement);
        }

        public void SwitchToDefaultContent()
        {
            _driver.SwitchTo().DefaultContent();
        }

        public void EnterTextInFrame(string text)
        {
            InputField.SendKeys(text);
        }

        public bool IsMainButtonVisible() => MainButton.Displayed;
    }
}
## IframeHandlingTests.cs
using Xunit;
using OpenQA.Selenium;
using IframeTests.Drivers;
using IframeTests.Pages;
using IframeTests.Utils;

namespace IframeTests.Tests
{
    public class IframeHandlingTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly IframePage _iframePage;

        public IframeHandlingTests()
        {
            _driver = WebDriverFactory.Create();
            _driver.Navigate().GoToUrl("https://example.com/iframe");
            _iframePage = new IframePage(_driver);
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        [Fact]
        public void SwitchToIframeByNameTest()
        {
            _iframePage.SwitchToFrameByName("frame1");
            _iframePage.EnterTextInFrame("Hello iframe (by name)");
            _iframePage.SwitchToDefaultContent();

            Assert.True(_iframePage.IsMainButtonVisible());
            ScreenshotHelper.TakeScreenshot(_driver, "frame_name.png");
        }

        [Fact]
        public void SwitchToIframeByIndexTest()
        {
            _iframePage.SwitchToFrameByIndex(0);
            _iframePage.EnterTextInFrame("Hello iframe (by index)");
            _iframePage.SwitchToDefaultContent();

            ScreenshotHelper.TakeScreenshot(_driver, "frame_index.png");
        }

        [Fact]
        public void SwitchToIframeByElementTest()
        {
            _iframePage.SwitchToFrameByElement();
            _iframePage.EnterTextInFrame("Hello iframe (by element)");
            _iframePage.SwitchToDefaultContent();

            ScreenshotHelper.TakeScreenshot(_driver, "frame_element.png");
        }

        [Fact]
        public void NestedIframeTest()
        {
            // Переключение на вложенные фреймы
            _driver.SwitchTo().Frame("outerFrame");
            _driver.SwitchTo().Frame("innerFrame");

            IWebElement nestedInput = _driver.FindElement(By.Id("nestedInput"));
            nestedInput.SendKeys("Nested iframe test");

            _driver.SwitchTo().DefaultContent();

            ScreenshotHelper.TakeScreenshot(_driver, "nested_frame.png");
        }
    }
}
## ScreenshotHelper.cs
using OpenQA.Selenium;
using System.IO;

namespace IframeTests.Utils
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
