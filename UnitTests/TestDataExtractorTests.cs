using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using SeleniumResultsParser;
using System.Xml;
using System.Xml.Linq;

namespace UnitTests
{
  [TestFixture]
  public class TestDataExtractorTests
  {
    [Test]
    public void GivenXmlFile_ShouldParseTo_TestResults()
    {
      FileInfo htmlFile = new FileInfo(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-result-with-seleniumtests"), "Application.test.html.failed.html"));
      XDocument inputFile = TestResultsParser.ConvertToXhtml(htmlFile);//tested in other tests for correct behaviour

      IEnumerable<TestResult> testResults = TestResultsParser.ExtractTestData(inputFile);
      Assert.AreEqual(3, testResults.Count(), "input html contains 3 tests");
      Assert.AreEqual(2, (from test in testResults where test.Passed select test).Count(), "2 tests passed");
      Assert.IsNotEmpty(from test in testResults where !test.Passed select test.ErrorMessage, "Tests which have failed should return a descriptive errorMessage");
    }
  }
}