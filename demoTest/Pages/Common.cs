using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoTest.Pages
{
    public class Common
    {
        public static byte[] CaptureScreenshot(IWebDriver driver)
        {
            return ((ITakesScreenshot)driver).GetScreenshot().AsByteArray;
        }

        public static void RunBatchFile(string batchFilePath)
        {
            var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C \"" + batchFilePath + "\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true, // Redirect error output
                UseShellExecute = false,
                CreateNoWindow = true // Set to true for background execution without a window
            };

            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                // Capture the output and errors
                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();

                // Wait for the process to exit
                process.WaitForExit();

                // Check the exit code
                if (process.ExitCode != 0)
                {
                    Console.WriteLine("Batch file execution failed with exit code: " + process.ExitCode);
                    Console.WriteLine("Error Output: " + errorOutput);
                    // Handle the failure accordingly, e.g., logging or throwing an exception
                    throw new Exception("Batch file execution failed: " + errorOutput);
                }

                // If needed, process the output
                Console.WriteLine("Batch output: " + output);
            }
        }
    }
}
