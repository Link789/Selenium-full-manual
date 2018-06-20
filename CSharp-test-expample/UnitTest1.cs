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
            IWebDriver driver = new FirefoxDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(11));
            driver.Navigate().GoToUrl("http://software-testing.ru");
            driver.Quit();
            driver = null;
           
        }
        public void TestMethod2()
        {

        }
        public void TestMethod3()
        {

        }
    }
}
