using OpenQA.Selenium;
using System;
using System.IO;

namespace SeleniumNUnitExcelAutomation.Utilities
{
    public static class ScreenshotHelper
    {
        /// <summary>
        /// Chụp ảnh màn hình khi test FAIL và trả về đường dẫn ảnh
        /// </summary>
        public static string TakeScreenshot(IWebDriver driver, string testCaseId, string folder = @"D:\ScreenShotFail")
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"{testCaseId}_{timestamp}.png";
            string fullPath = Path.Combine(folder, fileName);

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            Console.WriteLine($"Screenshot saved: {fullPath}");
            return fullPath;
        }
    }
}