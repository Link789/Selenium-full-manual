using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace CSharp_test_expample
{
    internal class ProductPage:Page
    {
        public ProductPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }
        internal ProductPage SelectSize()
        {
            try
            {
                IWebElement options = driver.FindElement(By.Name("options[Size]"));
                SelectElement selectSoldOutStatus = new SelectElement(options);
                selectSoldOutStatus.SelectByValue("Small");
            }
            catch { }
            return this;
        }
        internal ProductPage AddProduct()
        {
            driver.FindElement(By.Name("add_cart_product")).Click();         
            return this;
        }
        public IWebElement GetCountWebElement()
        {
            IWebElement count = driver.FindElement(By.CssSelector(".content .quantity"));
            return count;
        }
        public int GetCountCart()
        {
            IWebElement count = driver.FindElement(By.CssSelector(".content .quantity"));       
            return Convert.ToInt32(count.GetAttribute("textContent"));
        }
        public void WaitAdd(IWebElement count, int count_after)
        {
            wait.Until(ExpectedConditions.TextToBePresentInElement(count, (count_after+1).ToString()));
        }
        public void GoToCart()
        {
            driver.FindElement(By.LinkText("Checkout »")).Click();
        }
        
    }
}
