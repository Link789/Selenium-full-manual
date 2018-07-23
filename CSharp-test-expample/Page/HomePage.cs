using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace CSharp_test_expample
{
    internal class HomePage:Page
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }
        internal HomePage Open()
        {
            driver.Url = "http://localhost/litecart";
            return this;
        }
        public void OpenProduct()
        {
            driver.FindElement(By.CssSelector(".box a.link")).Click();         
        }
    }
}
