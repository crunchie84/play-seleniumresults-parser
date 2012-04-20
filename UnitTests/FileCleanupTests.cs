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
  public class FileCleanupTests
  {
    [Test]
    public void TakeHtml_ConvertToXhtml_ShouldWork()
    {
      FileInfo htmlFile = new FileInfo(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-result-with-seleniumtests"), "Application.test.html.failed.html"));
      XDocument result = TestResultsParser.ConvertToXhtml(htmlFile);
      Assert.IsNotNull(result, "Should be able to be converted to XML");
    }
  }
}