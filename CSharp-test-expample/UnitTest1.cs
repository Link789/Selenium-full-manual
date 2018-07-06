﻿using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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
        [TestMethod]
        public void TestMethod4()
        {
            driver = new FirefoxDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("http://localhost/litecart/admin");
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            //1)
            driver.Navigate().GoToUrl("http://localhost/litecart/admin/?app=countries&doc=countries");

            ReadOnlyCollection<IWebElement> rows = driver.FindElements(By.CssSelector("form[name=countries_form] tr.row"));              
            List<string> row = new List<string>();
            foreach (IWebElement tr in rows)
            {
                row.Add(tr.FindElement(By.XPath("./td[5]/a")).GetAttribute("textContent") + "\t" +
                tr.FindElement(By.XPath("./td[5]/a")).GetAttribute("href") + "\t" + tr.FindElement(By.XPath("./td[6]")).GetAttribute("textContent"));                                       
            }
            Assert.IsTrue(InAlphabetOrder_split(row));

            foreach (string line in row)
            {
                string[] attribute = line.Split('\t');
                if (Convert.ToInt32(attribute[2]) > 0)
                {
                    driver.Navigate().GoToUrl(attribute[1]);
                    ReadOnlyCollection<IWebElement> zoneRows = driver.FindElements(By.CssSelector("table#table-zones tr"));
                    List<string> zoneRow = new List<string>();
                    foreach (IWebElement tr in zoneRows)
                    {                      
                        if (tr.FindElements(By.XPath("./td[3]")).Count > 0 && tr.FindElement(By.XPath("./td[3]")).GetAttribute("textContent") != "")
                        {
                            zoneRow.Add(tr.FindElement(By.XPath("./td[3]")).GetAttribute("textContent"));
                        }
                    }
                    Assert.IsTrue(InAlphabetOrder(zoneRow));
                }
            }
            // the end of the first part

            //2)
            driver.Navigate().GoToUrl("http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones");
            ReadOnlyCollection<IWebElement> tableRow = driver.FindElements(By.CssSelector("form[name=geo_zones_form] tr.row"));
            List<string> tRow = new List<string>();
            foreach (IWebElement tr in tableRow)
            {
                tRow.Add(tr.FindElement(By.XPath("./td[3]/a")).GetAttribute("href"));
            }
            foreach (string line in tRow)
            {
                driver.Navigate().GoToUrl(line);
                ReadOnlyCollection<IWebElement> selects = driver.FindElements(By.CssSelector("table#table-zones select[name*=zone_code]>option[selected=selected]"));
                List<string> select_text = new List<string>();
                foreach (IWebElement select in selects)
                {
                    select_text.Add(select.GetAttribute("text"));
                }
                Assert.IsTrue(InAlphabetOrder(select_text));
            }
            CloseDriver(driver); 
            // end of part two
        }
        [TestMethod]
        public void TestMethod5()
        {
            driver = new FirefoxDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string[] campaignsName = new string[2];
            string[] regularPrice = new string[5];
            string[] campaignPrice = new string[5];

            driver.Navigate().GoToUrl("http://localhost/litecart");

            IWebElement objName = driver.FindElement(By.CssSelector("#box-campaigns .name"));
            IWebElement objRegularP = driver.FindElement(By.CssSelector("#box-campaigns .regular-price"));
            IWebElement objCampaignP = driver.FindElement(By.CssSelector("#box-campaigns .campaign-price"));

            campaignsName[0]= objName.GetAttribute("textContent");
            regularPrice[0] = objRegularP.GetAttribute("textContent");
            campaignPrice[0] = objCampaignP.GetAttribute("textContent");

            regularPrice[2] = objRegularP.GetCssValue("color");
            campaignPrice[2] = objCampaignP.GetCssValue("color");

            regularPrice[3] = objRegularP.GetCssValue("text-decoration");
            campaignPrice[3] = objCampaignP.GetCssValue("font-weight");

            regularPrice[4] = objRegularP.GetCssValue("font-size");
            campaignPrice[4] = objCampaignP.GetCssValue("font-size");

            Assert.IsTrue(GreyColorline_through(regularPrice[2], regularPrice[3]));// обычная цена зачёркнутая и серая
            Assert.IsTrue(RedColorBold(campaignPrice[2], campaignPrice[3]));// акционная жирная и красная
            Assert.IsTrue(FontSize(regularPrice[4], campaignPrice[4]));// акционная цена крупнее, чем обычная

            driver.FindElement(By.CssSelector("#box-campaigns .image-wrapper")).Click();

            objName = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#box-product .title")));
            objRegularP = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#box-product .regular-price")));
            objCampaignP = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#box-product .campaign-price")));

            campaignsName[1] = objName.GetAttribute("textContent");
            regularPrice[1] = objRegularP.GetAttribute("textContent");
            campaignPrice[1] = objCampaignP.GetAttribute("textContent");

            regularPrice[2] = objRegularP.GetCssValue("color");
            campaignPrice[2] = objCampaignP.GetCssValue("color");

            regularPrice[3] = objRegularP.GetCssValue("text-decoration");
            campaignPrice[3] = objCampaignP.GetCssValue("font-weight");

            regularPrice[4] = objRegularP.GetCssValue("font-size");
            campaignPrice[4] = objCampaignP.GetCssValue("font-size");

            Assert.IsTrue(GreyColorline_through(regularPrice[2], regularPrice[3]));// обычная цена зачёркнутая и серая 
            Assert.IsTrue(RedColorBold(campaignPrice[2], campaignPrice[3]));// акционная жирная и красная
            Assert.IsTrue(FontSize(regularPrice[4], campaignPrice[4]));// акционная цена крупнее, чем обычная

            Assert.IsTrue(campaignsName[1] == campaignsName[0]);// на странице товара совпадает текст названия товара
            Assert.IsTrue(regularPrice[1] == regularPrice[0]);// на странице товара совпадают цены (обычная)
            Assert.IsTrue(campaignPrice[1] == campaignPrice[0]);// на странице товара совпадают цены (акционная)
     
            CloseDriver(driver);      
        }
        [TestMethod]
        public void TestMethod6()
        {
            string US = "United States";
            driver = new FirefoxDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
     
            driver.Navigate().GoToUrl("http://localhost/litecart");
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='box-account-login']//a[contains(.,'New customers')]"))).Click();
            driver.FindElement(By.Name("firstname")).SendKeys("Max");
            driver.FindElement(By.Name("lastname")).SendKeys("Limberman");
            driver.FindElement(By.Name("address1")).SendKeys("Sozonova st.");
            driver.FindElement(By.Name("city")).SendKeys("San Francisco");

            IWebElement country = driver.FindElement(By.Name("country_code"));
            SelectElement selectCountry = new SelectElement(country);
            IList<IWebElement> options = selectCountry.Options;

            foreach (IWebElement opt in options)
            {
                if (opt.GetAttribute("textContent") == US)
                    US = opt.GetAttribute("index");
            }           
            js.ExecuteScript("arguments[0].selectedIndex="+US+"; arguments[0].dispatchEvent(new Event('change'))", country);      
            driver.FindElement(By.Name("postcode")).SendKeys("55555");

            IWebElement zone = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("select[name=zone_code]")));
            SelectElement selectZone = new SelectElement(zone);
            selectZone.SelectByValue("CA");
            string email = RandomString(8) + "@mail.ru";
            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("phone")).SendKeys("+79509572121");
            driver.FindElement(By.Name("password")).SendKeys("QWERTY");
            driver.FindElement(By.Name("confirmed_password")).SendKeys("QWERTY");
            driver.FindElement(By.Name("create_account")).Click();// конец регистрации аккаунта

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(.,'Logout')]"))).Click();// logout
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email"))).SendKeys(email);
            driver.FindElement(By.Name("password")).SendKeys("QWERTY");
            driver.FindElement(By.CssSelector(".button-set>button")).Click();// login
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(.,'You are now logged in as')]")));// login success
            CloseDriver(driver);
        }
        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public bool FontSize(string regular, string campaign)
        {
            regular = regular.Replace("px", "");
            regular = regular.Replace(".", ",");
            campaign = campaign.Replace("px", "");
            if (Convert.ToDecimal(regular) < Convert.ToDecimal(campaign))
                return true;
            else
                return false;
        }
        public bool RedColorBold(string rgb, string outline)
        {
            rgb = rgb.Replace("rgb(", "");
            rgb = rgb.Replace(")", "");
            rgb = rgb.Replace(" ", "");
            string[] digital = rgb.Split(',');
            if (Convert.ToInt32(digital[2]) == 0 && Convert.ToInt32(digital[1]) == 0 && Convert.ToInt32(outline) >600)
                return true;
            else
                return false;
        }
        public bool GreyColorline_through(string rgb,string outline)
        {
            rgb = rgb.Replace("rgb(", "");
            rgb = rgb.Replace(")", "");
            rgb = rgb.Replace(" ", "");
            string[] digital = rgb.Split(',');         
            if (digital[0] == digital[1] && digital[0] == digital[2] && digital[1] == digital[2] && outline == "line-through")
                return true;
            else
                return false;
        }
        public bool InAlphabetOrder(List<string> array)
        {
            List<string> name_sort = new List<string>();
            foreach (string row in array)
            {              
                name_sort.Add(row);
            }
            name_sort.Sort();
            bool result = true;
            for (int i = 0; i < name_sort.Count; i++)
            {
                Console.WriteLine(array[i] + "==" + name_sort[i]);
                if (array[i] != name_sort[i])
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        public bool InAlphabetOrder_split(List <string> array)
        {            
            List<string> name =new List<string>();
            List<string> name_sort = new List<string>();
            foreach (string row in array)
            {
                string[] name_ = row.Split('\t');
                name.Add(name_[0]);
                name_sort.Add(name_[0]);
            }
            name_sort.Sort();
            bool result = true;
            for (int i = 0; i < name_sort.Count; i++)
            {
                Console.WriteLine(name[i] + "==" + name_sort[i]);
                if (name[i] != name_sort[i])
                {
                    result = false;
                    break;
                }
            }
            return result;
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