# Практическая работа №12
## Тема: Настройка проекта и написание первого UI-теста
### Вариант: 5

**Задание:** Проверить видимость всех пунктов меню навигации на главной странице.

---

### Ход работы

1. Создан проект типа `NUnit Test Project (.NET 6)`.
2. Установлены NuGet-пакеты:
   - `Selenium.WebDriver`
   - `Selenium.WebDriver.ChromeDriver`
   - `Selenium.Support`
3. Реализована структура проекта с каталогами `/Drivers`, `/Pages`, `/Tests`, `/Utils`.
4. Написан тест `CheckNavigationMenuItemsVisible`, проверяющий наличие всех пунктов меню.
5. Добавлен скриншот успешного прохождения теста в `/images`.

---

### Пример кода

```csharp
IWebElement menuItem = driver.FindElement(By.Id("menuHome"));
Assert.IsTrue(menuItem.Displayed, "Пункт меню Home не отображается");


## WebDriverFactory.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UIAutomationTests.Drivers
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
## MainPage.cs
using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class MainPage
    {
        private readonly IWebDriver _driver;

        public MainPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement HomeMenu => _driver.FindElement(By.Id("menuHome"));
        public IWebElement AboutMenu => _driver.FindElement(By.Id("menuAbout"));
        public IWebElement ContactMenu => _driver.FindElement(By.Id("menuContact"));
        public IWebElement ProductsMenu => _driver.FindElement(By.Id("menuProducts"));
    }
}
##NavigationMenuTest.cs
using NUnit.Framework;
using OpenQA.Selenium;
using UIAutomationTests.Drivers;
using UIAutomationTests.Pages;
using System.IO;

namespace UIAutomationTests.Tests
{
    [TestFixture]
    public class NavigationMenuTest
    {
        private IWebDriver _driver;
        private MainPage _mainPage;

        [SetUp]
        public void Setup()
        {
            _driver = WebDriverFactory.Create();
            _driver.Navigate().GoToUrl("https://example.com");
            _mainPage = new MainPage(_driver);
        }

        [Test]
        public void CheckNavigationMenuItemsVisible()
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(_mainPage.HomeMenu.Displayed, "Пункт меню 'Home' не отображается");
                Assert.IsTrue(_mainPage.AboutMenu.Displayed, "Пункт меню 'About' не отображается");
                Assert.IsTrue(_mainPage.ContactMenu.Displayed, "Пункт меню 'Contact' не отображается");
                Assert.IsTrue(_mainPage.ProductsMenu.Displayed, "Пункт меню 'Products' не отображается");
            });

            // Скриншот
            Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            Directory.CreateDirectory("images");
            screenshot.SaveAsFile("images/menu_test_result.png");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
