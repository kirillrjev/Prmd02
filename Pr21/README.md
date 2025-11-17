## DriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace CrossBrowserTests.Drivers
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
## BaseTest.cs
using OpenQA.Selenium;
using System;
using Xunit;
using CrossBrowserTests.Drivers;

namespace CrossBrowserTests.Tests
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
## HomePageTests.cs (Theory + InlineData)
using Xunit;
using CrossBrowserTests.Drivers;
using OpenQA.Selenium;

namespace CrossBrowserTests.Tests
{
    public class HomePageTests
    {
        [Theory]
        [InlineData("chrome")]
        [InlineData("firefox")]
        public void HomePage_TitleIsCorrect(string browser)
        {
            IWebDriver driver = DriverFactory.GetDriver(browser);

            driver.Navigate().GoToUrl("https://example.com");

            string title = driver.Title;

            Assert.Equal("Example Domain", title);

            driver.Quit();
        }
    }
}
# Практическая работа №21
## Тема: Настройка кросс-браузерного тестирования (Chrome, Firefox)
### Вариант: 3

---

## Цель работы
- Научиться запускать Selenium тесты в нескольких браузерах.
- Реализовать фабрику драйверов DriverFactory.
- Написать параметризованные тесты через [Theory].
- Проверить заголовок страницы https://example.com.

---

## Используемые технологии
- .NET + xUnit
- Selenium WebDriver
- ChromeDriver + GeckoDriver

---

## Структура проекта
CrossBrowserTests/
- Drivers/ — фабрика драйверов  
- Tests/ — тесты  
- images/ — скриншоты  
- readme.md  

---

## Пример кода теста:

```csharp
[Theory]
[InlineData("chrome")]
[InlineData("firefox")]
public void HomePage_TitleIsCorrect(string browser)
{
    Driver = DriverFactory.GetDriver(browser);
    Driver.Navigate().GoToUrl("https://example.com");
    Assert.Equal("Example Domain", Driver.Title);
    Driver.Quit();
}
