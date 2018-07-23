using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSharp_test_expample
{
    internal class CartPage : Page
    {
        public CartPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }
        internal CartPage WaitLoadPageCart()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#order_confirmation-wrapper td.item")));
            return this;
        }
        internal CartPage DeleteProduct()
        {
            ReadOnlyCollection<IWebElement> rows = driver.FindElements(By.CssSelector("#order_confirmation-wrapper td.item"));
            for (int i = rows.Count; i > 0; i--)
            {
                if (AssertCssValue(".viewport>.items", "margin-left", "0px") == true)
                {
                    driver.FindElement(By.CssSelector(".items [value=Remove]")).Click();
                    ReadOnlyCollection<IWebElement> deleteRow = driver.FindElements(By.CssSelector("#order_confirmation-wrapper td.item"));
                    wait.Until(ExpectedConditions.StalenessOf(deleteRow[deleteRow.Count - 1]));
                }
            }           
            return this;
        }   
        private bool AssertCssValue(string locator, string cssProperty, string targetValue)
        {
            bool x;
            for (;;)
            {
                if (driver.FindElement(By.CssSelector(locator)).GetCssValue(cssProperty) == targetValue)
                {
                    x = true;
                    break;
                }
                else
                    x = false;
                Thread.Sleep(500);
            }
            return x;
        }
    }
}
