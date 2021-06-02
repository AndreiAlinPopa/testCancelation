using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using static testCancelation.Global;
using static testCancelation.ProcessHandling.Input;
using static testCancelation.ProcessHandling.Output;
using static testCancelation.ProcessHandling.Troubleshoot;
using static testCancelation.ProcessHandling.Navigation;

namespace testCancelation
{
    class EntryPoint
    {
        static void Main()
        {

            // Ask user for information
            string drivingLicence = testCancelation.ProcessHandling.Input.GetLicence();
            string postcode = testCancelation.ProcessHandling.Input.GetPostcode();

            // Gather system information
            string localDate = DateTime.Now.ToString("dd/MM/yyyy"); // Current 

            // Navigate to the available bookings
            testCancelation.ProcessHandling.Navigation.NavigateToAvailableBookings(drivingLicence, localDate, postcode);

            /////////////////////////  LOGGED IN <=> VIEWING TESTS /////////////////////////

            while (true)
            {
                // This type name really hurts to look at
                System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> testCentreDetails = cDriver.FindElements(By.ClassName("test-centre-details"));

                // We check the availability of car tests, and output if we find one
                CheckListings(testCentreDetails);

            }

        }

    }   

}
