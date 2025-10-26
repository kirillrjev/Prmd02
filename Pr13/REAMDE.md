# Практическая работа №13
## Тема: Поиск элементов на странице с помощью различных локаторов
### Вариант: 7

**Задание:** Найти поле логина по XPath и проверить, что оно отображается.

---

### Ход работы

1. Создан проект типа `NUnit Test Project (.NET 6)`.
2. Установлены NuGet-пакеты:
   - `Selenium.WebDriver`
   - `Selenium.WebDriver.ChromeDriver`
   - `Selenium.Support`
3. Создан драйвер Chrome в классе `WebDriverFactory`.
4. Реализован тест `FindLoginFieldByXPath`, который:
   - Открывает страницу входа `https://example.com/login`
   - Находит поле логина по XPath `//input[@id='username']`
   - Проверяет, что элемент отображается
   - Делает скриншот и сохраняет в `/images/xpath_test_result.png`

---

### Пример кода

```csharp
IWebElement username = driver.FindElement(By.XPath("//input[@id='username']"));
Assert.IsTrue(username.Displayed, "Поле логина не найдено");

## Drivers/WebDriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LocatorAutomation.Drivers
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
## Tests/XPathLocatorTest.cs
using NUnit.Framework;
using OpenQA.Selenium;
using LocatorAutomation.Drivers;
using System.IO;

namespace LocatorAutomation.Tests
{
    [TestFixture]
    public class XPathLocatorTest
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = WebDriverFactory.Create();
            _driver.Navigate().GoToUrl("https://example.com/login");
        }

        [Test]
        public void FindLoginFieldByXPath()
        {
            // Поиск поля логина по XPath
            IWebElement usernameField = _driver.FindElement(By.XPath("//input[@id='username']"));

            Assert.IsTrue(usernameField.Displayed, "Поле логина не найдено или не отображается");

            // Скриншот
            Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            Directory.CreateDirectory("images");
            screenshot.SaveAsFile("images/xpath_test_result.png");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
