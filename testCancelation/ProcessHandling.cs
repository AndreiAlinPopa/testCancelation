using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using static testCancelation.Global;

namespace testCancelation
{
    class ProcessHandling
    {
        public class Input
        {
            public static string GetLicence()
            {
                Console.WriteLine("Enter driving licence: ");
                string drivingLicence = Console.ReadLine();

                return drivingLicence;
            }

            public static string GetPostcode()
            {
                Console.WriteLine("Enter postcode: ");
                string postcode = Console.ReadLine();

                return postcode;
            }

        }

        public class Navigation
        {
            public static void NavigateToAvailableBookings(string drivingLicence, string localDate, string postcode)
            {
                cDriver.Navigate().GoToUrl("https://driverpracticaltest.dvsa.gov.uk/application");

                //!!CAPTCHA HANDLING
                while (Troubleshoot.waitscreenExists()) { }
                while (Troubleshoot.captchaExists()) { }

                IWebElement carTestButton = cDriver.FindElement(By.Name("testTypeCar"));
                carTestButton.Click();

                //!!CAPTCHA HANDLING
                while (Troubleshoot.waitscreenExists()) { }
                while (Troubleshoot.captchaExists()) { }

                //NAVIGATION: --> INPUT REQUIREMENTS
                IWebElement drivingLicenceField = cDriver.FindElement(By.Name("driverLicenceNumber"));
                IWebElement extendedTestButton = cDriver.FindElement(By.Id("extended-test-no"));
                IWebElement specialNeedsButton = cDriver.FindElement(By.Id("special-needs-none"));
                IWebElement drivingLicenceSubmitButton = cDriver.FindElement(By.Id("driving-licence-submit"));

                drivingLicenceField.SendKeys(drivingLicence);
                extendedTestButton.Click();
                specialNeedsButton.Click();
                drivingLicenceSubmitButton.Click();

                //!!CAPTCHA HANDLING
                while (Troubleshoot.captchaExists()) { }

                //NAVIGATION: --> INPUT DATES
                IWebElement calendarField = cDriver.FindElement(By.Id("test-choice-calendar"));
                IWebElement calendarSubmit = cDriver.FindElement(By.Id("driving-licence-submit"));

                calendarField.SendKeys(localDate);
                calendarSubmit.Click();

                //!!CAPTCHA HANDLING
                while (Troubleshoot.captchaExists()) { }

                //Navigation --> INPUT TEST CENTRE

                IWebElement postCodeField = cDriver.FindElement(By.Id("test-centres-input"));
                IWebElement postCodeSubmit = cDriver.FindElement(By.Id("test-centres-submit"));

                postCodeField.SendKeys(postcode);
                postCodeSubmit.Click();

                //!!CAPTCHA HANDLING
                while (Troubleshoot.captchaExists()) { }
            }

            public static void BrowseAvailableBookings()
            {
                // pass
            }
        }

        public class Output
        {
            public static void Write_To_Txt(string stringContent)
            {
                string pwd = (Directory.GetCurrentDirectory()) + "\\output.txt";

                List<string> lines = new List<string>();
                lines = File.ReadAllLines(pwd).ToList();

                lines.Add(stringContent);
                File.WriteAllLines(pwd, lines);
            }

            public static void CheckListings(System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> testCentreDetails)
            {
                foreach (int i in Enumerable.Range(0, (testCentreDetails.Count)))
                {

                    string currentCentre = testCentreDetails[i].Text;

                    if (!(currentCentre.Contains("No dates found")))
                    {
                        Write_To_Txt(currentCentre + " <-> " + DateTime.Now.ToString());
                        Console.WriteLine(currentCentre);
                    }

                }

                Thread.Sleep(5000);
                cDriver.Navigate().Refresh();
            }
        }
        
        public class Troubleshoot
            {
                public static bool captchaExists()
                {

                    // Declaring driver
                    IWebDriver cDriver = testCancelation.Global.cDriver;

                    try
                    {
                        IWebElement errorExist = cDriver.FindElement(By.Name("ROBOTS"));
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                }

                public static bool waitscreenExists()
                {

                    // Declaring driver
                    IWebDriver cDriver = testCancelation.Global.cDriver;

                    try
                    {
                        IWebElement errorExist = cDriver.FindElement(By.ClassName("queue"));
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                }

            }

     
    }
}
