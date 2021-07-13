using System;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace TestProject.Utility
{
    public class ExtentReportGen
    {

        public static string ReportingFolder = Environment.CurrentDirectory;

        public static AventStack.ExtentReports.ExtentReports extentReports;
        public static ExtentTest extentTest;
        public static ExtentHtmlReporter htmlReporter;
        public static string GeneratedReportPath;
        public static string GeneratedReportPathIndex;
       
        #region ---Extent Reports Helper Methods---

        
        //Get path of the Project Bin Folder
        public static string GetProjectPath()
        {
            Uri path = null;
            path = new Uri(Assembly.GetCallingAssembly().CodeBase);

            string actualPath = new FileInfo(path.AbsolutePath).Directory.FullName;
            string path1 = actualPath.Remove(actualPath.IndexOf("bin\\"));
            return path1;
        }

        //Get path of the Test Results folder in the project
        public static string GetFolderPath(string TestFolderName)
        {
            string projectPath = GetProjectPath();
            string resultsFolder = Path.Combine(projectPath, TestFolderName);
            return resultsFolder;
        }

        //Initialize and create Extent Report
        public static void InitializeReport()
        {
            try
            {
                extentReports = new AventStack.ExtentReports.ExtentReports();

                string resultsFolder = GetFolderPath("TestResults\\Test_" + DateTime.Now.ToString("dd-MM-yyyy"));
                if (!Directory.Exists(resultsFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(resultsFolder);
                }
                GeneratedReportPath = resultsFolder + "\\dashboard.html";
                GeneratedReportPathIndex = resultsFolder + "\\index.html";
                htmlReporter = new ExtentHtmlReporter(GeneratedReportPath);
                extentReports.AttachReporter(htmlReporter);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //Prints exception details in Reports
        public static void PrintException(ExtentTest test, Exception e)
        {
            string msg = e.Message + " Occurred at " + e.TargetSite;
            try
            {
                Assert.Fail(msg);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            test.Error(MarkupHelper.CreateLabel(msg, ExtentColor.Orange));
        }
       
        public static void LogMessage(ExtentTest test, string msg)
        {
            Console.WriteLine(msg);
            test.Info(MarkupHelper.CreateLabel(msg, ExtentColor.Lime));
        }

        //Prints Pass Message in Reports
        public static void PassMessage(ExtentTest test, string msg)
        {
            Console.WriteLine(msg);
            test.Pass(MarkupHelper.CreateLabel(msg, ExtentColor.Green));
        }

        //Prints Fail Message in Reports
        public static void FailMessage(ExtentTest test, string msg)
        {
            Console.WriteLine(msg);
            test.Fail(MarkupHelper.CreateLabel(msg, ExtentColor.Red));
        }
        public static void SkipMessage(ExtentTest test, string msg)
        {
            Console.WriteLine(msg);
            test.Skip(MarkupHelper.CreateLabel(msg, ExtentColor.Orange));
        }
        public static void WarnMessage(ExtentTest test, string msg)
        {
            Console.WriteLine(msg);
            test.Warning(MarkupHelper.CreateLabel(msg, ExtentColor.Orange));
        }

        #endregion
    }

}
