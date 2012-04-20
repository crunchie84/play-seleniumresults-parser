using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using SeleniumResultsParser;

namespace UnitTests
{
  [TestFixture]
  public class FileSelectorTests
  {
    [Test]
    public void GivenDirectoryListingWithTestsInside_ShouldReturn_SeleniumTestFiles()
    {
      DirectoryInfo testResultsDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-result-with-seleniumtests"));
      IEnumerable<FileInfo> seleniumResultFiles = TestResultsParser.GetSeleniumResultFiles(testResultsDir.GetFiles());

      //verify we only have 1 file returned
      Assert.AreEqual(1, seleniumResultFiles.Count(), "only 1 file is expected, 'Application.test.html.failed.html'");
      
      //verify that it is the actual file we expected
      var result = (from file in seleniumResultFiles where file.Name == "Application.test.html.failed.html" select file).FirstOrDefault();      
      Assert.IsNotNull(result);
      Assert.AreEqual("Application.test.html.failed.html", result.Name);      
    }

    [Test]
    public void GivenDirectoryListingNoTestsInside_ShouldReturn_NoSeleniumTestFiles()
    {
      DirectoryInfo testResultsDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-result-no-seleniumtests"));
      IEnumerable<FileInfo> seleniumResultFiles = TestResultsParser.GetSeleniumResultFiles(testResultsDir.GetFiles());

      //verify we only have 1 file returned
      Assert.AreEqual(0, seleniumResultFiles.Count(), "No selenium tests are in this dir. Expected to be returned 0 testfiles");
    }
  }
}
