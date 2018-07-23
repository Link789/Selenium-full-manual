using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.PageObjects;

using System.Collections;
using System.IO;

namespace CSharp_test_expample
{

    class Application
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private HomePage homePage;
        private CartPage cartPage;
        private ProductPage productPage;

        public Application()
        {
            driver = new FirefoxDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            homePage = new HomePage(driver);
            cartPage = new CartPage(driver);
            productPage = new ProductPage(driver);
        }

        public void Quit()
        {
            driver.Close();
            driver.Quit();
        }

        private void AddProductInCart()
        {
            homePage.Open().OpenProduct();
            productPage.SelectSize().AddProduct();
            productPage.WaitAdd(productPage.GetCountWebElement(),productPage.GetCountCart());
            
        }
        public void AddProductsInCart()
        {
            for (int i=0;i<3;i++)
                AddProductInCart();
            productPage.GoToCart();
            cartPage.WaitLoadPageCart();
        }
        public void DeleteProductsInCart()
        {
            cartPage.DeleteProduct();
        }
        
    }
}
