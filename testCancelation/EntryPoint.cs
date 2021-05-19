using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;

namespace testCancelation
{
    class EntryPoint
    {
        static void Main(string[] args)
        {

            // Ask user for information
            Console.WriteLine("Enter driving licence: ");
            string drivingLicence = Console.ReadLine();
            Console.WriteLine("Enter postcode: ");
            string postcode = Console.ReadLine();
            // Gather system information
            string localDate = DateTime.Now.ToString("dd/MM/yyyy"); // Current 

            // Declaring driver
            ref IWebDriver cDriver = ref Global.cDriver;


            //NAVIGATION --> CAR TEST
            cDriver.Navigate().GoToUrl("https://driverpracticaltest.dvsa.gov.uk/application");

            //!!CAPTCHA HANDLING
            while (waitscreenExists()) { }
            while (captchaExists()) { }

            IWebElement carTestButton = cDriver.FindElement(By.Name("testTypeCar"));
            carTestButton.Click();

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
            while (captchaExists()) { }

            //NAVIGATION: --> INPUT DATES
            IWebElement calendarField = cDriver.FindElement(By.Id("test-choice-calendar"));
            IWebElement calendarSubmit = cDriver.FindElement(By.Id("driving-licence-submit"));

            calendarField.SendKeys(localDate);
            calendarSubmit.Click();

            //!!CAPTCHA HANDLING
            while (captchaExists()) { }

            //Navigation --> INPUT TEST CENTRE

            IWebElement postCodeField = cDriver.FindElement(By.Id("test-centres-input"));
            IWebElement postCodeSubmit = cDriver.FindElement(By.Id("test-centres-submit"));

            postCodeField.SendKeys(postcode);
            postCodeSubmit.Click();

            //!!CAPTCHA HANDLING
            while (captchaExists()) { }

            ////////////////////////////////////////////////////////////////////////////////
            /////////////////////////  LOGGED IN <=> VIEWING TESTS /////////////////////////
            ////////////////////////////////////////////////////////////////////////////////


            while (true)
            {
                var testCentreDetails = cDriver.FindElements(By.ClassName("test-centre-details"));

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

            cDriver.Quit();
        }

        static bool captchaExists()
        {

            // Declaring driver
            IWebDriver cDriver = Global.cDriver;

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

        static bool waitscreenExists()
        {

            // Declaring driver
            IWebDriver cDriver = Global.cDriver;

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

        static void Write_To_Txt(string stringContent)
        {
            string pwd = (Directory.GetCurrentDirectory()) + "\\output.txt";

            List<string> lines = new List<string>();
            lines = File.ReadAllLines(pwd).ToList();

            lines.Add(stringContent);
            File.WriteAllLines(pwd, lines);
        }


    }   

    class Global
    {
        // Declaring driver
        public static IWebDriver cDriver = new ChromeDriver();
    }

}
