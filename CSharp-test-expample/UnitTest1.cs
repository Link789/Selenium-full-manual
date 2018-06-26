using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace CSharp_test_expample
{
    [TestClass]
    public class UnitTest1
    { 
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
            IWebDriver driver = new FirefoxDriver(options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("http://localhost/litecart/admin");
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("sidebar")));
            driver.Quit();
            driver = null;
        }

        
    }
}
