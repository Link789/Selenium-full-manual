using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.ObjectModel;

namespace CSharp_test_expample
{
    [TestClass]
    public class UnitTest1
    {
        public IWebDriver driver;
        [TestMethod]
        public void TestMethod1()
        {
            //задание 3
            //IWebDriver driver = new FirefoxDriver();
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //FirefoxOptions options = new FirefoxOptions();
            //options.UseLegacyImplementation = true;
            //driver = new FirefoxDriver(options);

            FirefoxOptions options = new FirefoxOptions();
            options.BrowserExecutableLocation = @"C:\Program Files\Firefox Nightly\firefox.exe";
            driver = new FirefoxDriver(options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("http://localhost/litecart/admin");
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("sidebar")));
            driver.Quit();
            driver = null;
        }
        [TestMethod]
        public void TestMethod2()
        {
            driver = new FirefoxDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("http://localhost/litecart/admin");
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            ReadOnlyCollection<IWebElement> linksMenu = driver.FindElements(By.CssSelector("ul#box-apps-menu li#app->a"));
            string[] listLinks = new string[linksMenu.Count];

            for (int i = 0; i < linksMenu.Count; i++)
            {
                listLinks[i] = linksMenu[i].GetAttribute("href");
            }

            foreach (string link in listLinks)
            {
                driver.Navigate().GoToUrl(link);
                Assert.IsTrue(AreElementPresent(By.CssSelector("td#content h1")));

                ReadOnlyCollection<IWebElement> linksSubMenu = driver.FindElements(By.CssSelector("li.selected li>a"));
                string[] subListLinks = new string[linksSubMenu.Count];

                for (int i = 0; i < linksSubMenu.Count; i++)
                {
                    subListLinks[i] = linksSubMenu[i].GetAttribute("href");
                }

                foreach (string l in subListLinks)
                {
                    driver.Navigate().GoToUrl(l);
                    Assert.IsTrue(AreElementPresent(By.CssSelector("td#content h1")));
                }
            }
            CloseDriver(driver);
        }
        [TestMethod]
        public void TestMethod3()
        {
            driver = new FirefoxDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("http://localhost/litecart");
            ReadOnlyCollection<IWebElement> images = driver.FindElements(By.CssSelector("div.image-wrapper"));
            foreach(IWebElement sticker in images)
            {
                Assert.IsTrue(AreElementPresent(By.CssSelector("div"),sticker));
            }
 
            CloseDriver(driver);
        }

        public bool AreElementPresent(By locator, IWebElement element)
        {
            return element.FindElements(locator).Count == 1;
        }

        public bool AreElementPresent(By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }
        public void CloseDriver(IWebDriver nameDriver)
        {
            nameDriver.Quit();
        }

    }
}