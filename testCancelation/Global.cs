using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;

namespace testCancelation
{
    class Global
    {
        // Declaring driver
        public static IWebDriver cDriver = new ChromeDriver();

    }
}