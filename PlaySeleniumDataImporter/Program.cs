using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SeleniumResultsParser;
using System.Xml.Linq;

namespace PlaySeleniumDataImporter
{
  class Program
  {

    /// <summary>
    /// We expect our application to be ran with the working directory pointing to our Play Project. 
    /// This way we can append 'test-result' as directory to the current working dir to find the test-result files
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
      Console.Out.WriteLine("Starting dataimporter with workingdir=" + Directory.GetCurrentDirectory());
      //this app needs to be ran with the current working dir in the /test-result/ directory of your play project
      DirectoryInfo testResultsDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "test-result"));
      if (!testResultsDir.Exists)
      {
        Console.Error.WriteLine("Could not find 'test-result' subdirectory in working dir: " + testResultsDir);
        Console.Error.WriteLine("Please verify that the current working directory points to the Play Framework project root directory & your test have been ran via play auto-test/test");
        Environment.Exit(2);
      }

      var files = testResultsDir.GetFiles();
      Console.Out.WriteLine(string.Format("Found {0} files in the \test-result directory, going to filter these for Selenium .html testresult files", files.Count()));
      
      int seleniumSuitesFound = 0;
      int testsFound = 0;
      foreach (FileInfo seleniumTestSuiteFile in TestResultsParser.GetSeleniumResultFiles(files))
      {
        seleniumSuitesFound++;
        XDocument seleniumXDocTestSuite = TestResultsParser.ConvertToXhtml(seleniumTestSuiteFile);

        //which testsuite is this? Shortcut: just remove last 2 parts (html and passed/failed of filename)
        string[] parts = seleniumTestSuiteFile.Name.Split('.');
        string title = string.Join(".", parts.Take(parts.Count() - 2));

        try
        {
          Console.Out.WriteLine("##teamcity[testSuiteStarted name='SeleniumTestSuite']");
          try
          {
            Console.Out.WriteLine(string.Format("##teamcity[testSuiteStarted name='{0}']", title));
            foreach (TestResult tr in TestResultsParser.ExtractTestData(seleniumXDocTestSuite))
            {
              testsFound++;
              Console.Out.WriteLine(string.Format("##teamcity[testStarted name='{0}']", tr.Title));
              if(!tr.Passed){
                string error = TeamCityServiceMessageParser.Escape(
                    string.Format("{0}{1}Check Selenium testsuite '{2}' in artifacts folder to see details & stacktrace", tr.ErrorMessage, Environment.NewLine, seleniumTestSuiteFile.Name)
                  );//assumption that you configured TeamCity to place /test-result/* in artifacts folder of your build                
                Console.Out.WriteLine(string.Format("##teamcity[testFailed name='{0}' message='{1}' details='{2}']", TeamCityServiceMessageParser.Escape(tr.Title), "Selenium test failed", error));
              }                
              Console.Out.WriteLine(string.Format("##teamcity[testFinished name='{0}']", tr.Title));
            }
          }
          finally
          {
            Console.Out.WriteLine(string.Format("##teamcity[testSuiteFinished name='{0}']", title));
          }
        }
        finally
        {
          Console.Out.WriteLine("##teamcity[testSuiteFinished name='SeleniumTestSuite']");
        }
      }
      Console.Out.WriteLine(string.Format("Found {0} Selenium TestSuites containing {1} tests and reported those to TeamCity via service messages", seleniumSuitesFound, testsFound));
    }
  }
}