using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using XHTMLr;
using System.Xml.Linq;
using System.Collections;

namespace SeleniumResultsParser
{
  public class TestResultsParser
  {

    /// <summary>
    /// given the list of files, only return {testname}.{failed/passed}.html files for which no corresponding TEST{testname}.xml file exists
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    public static IEnumerable<FileInfo> GetSeleniumResultFiles(IEnumerable<FileInfo> files)
    {
      return from file in files where file.Extension.ToLower() == ".html" && !existsXmlMetaData(file, files) select file;
    }

    /// <summary>
    /// return if for the given html file a xml file exists in the <paramref name="allFiles"/> array
    /// </summary>
    /// <param name="htmlFileName"></param>
    /// <param name="allFiles"></param>
    /// <returns></returns>
    private static bool existsXmlMetaData(FileInfo htmlFile, IEnumerable<FileInfo> allFiles)
    {
      if (htmlFile.Extension.ToLower() != ".html")
        throw new ArgumentOutOfRangeException(string.Format("given html file '{0}' has no .html extension", htmlFile.Name));

      string htmlFileName = htmlFile.Name;
      if (htmlFileName.LastIndexOf('.') == htmlFileName.IndexOf('.'))
        throw new ArgumentOutOfRangeException(string.Format("given html file '{0}' contains no '.' characters. expected format = 'testname.class.{failed/passed}.html'", htmlFile.Name));

      string xmlFileName = string.Format("TEST-{0}.xml", htmlFileName.Substring(0, htmlFileName.IndexOf('.'))).ToLower();

      return (from file in allFiles where file.Name.ToLower() == xmlFileName select file).FirstOrDefault() != null;//does the xml file exist for the given html file?
    }

    /// <summary>
    /// Take the given html file and convert it to xml for parsing
    /// </summary>
    /// <param name="htmlFile"></param>
    /// <returns></returns>
    public static XDocument ConvertToXhtml(FileInfo htmlFile)
    {
      string html;
      using (StreamReader sr = new StreamReader(htmlFile.FullName))
      {
        html = sr.ReadToEnd();
      }
      string xml = XHTML.ToXml(html, XHTML.Options.Default | XHTML.Options.Pretty);


      return XDocument.Parse(xml);
    }

    /// <summary>
    /// given the input file, extract how many tests are contained in it and if they failed/succeeded
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static IEnumerable<TestResult> ExtractTestData(XDocument inputFile)
    {
      /// html/body/div[@id='results']//table maar dan met xdoc... *sighs*
      var mainDiv = (from divEl in inputFile.Element("html").Element("body").Descendants("div")
                   where divEl.Attribute("id") != null && divEl.Attribute("id").Value == "results"
                   select divEl).FirstOrDefault();
      if (mainDiv == null)
        return Enumerable.Empty<TestResult>();

      return from tableEl in mainDiv.Descendants("table") select parseTestResult(tableEl);
    }

    /// <summary>
    /// given the table element of the selenium testcase, parse it and return the data
    /// </summary>
    /// <param name="testCaseTableElement"></param>
    /// <returns></returns>
    private static TestResult parseTestResult(XElement testCaseTableElement)
    {
      if (testCaseTableElement == null)
        return null;

      /*
       * appengine selenium styling = 
       * class .status_passed = passed test
       * class .status_failed = failed test
       */
      var stepWhichFailedEl = (from el in testCaseTableElement.Descendants("tbody").Descendants("tr") where el.Attribute("class") != null && el.Attribute("class").Value == "status_failed" select el).FirstOrDefault();

      return new TestResult
      {
        Title = (from el in testCaseTableElement.Descendants("thead").Descendants("tr").Descendants("th") select el.Value).First(),//title of this testcase => thead/tr/td/text()
        Passed = stepWhichFailedEl == null,
        ErrorMessage = stepWhichFailedEl == null ? string.Empty : string.Join(" -> ", from el in stepWhichFailedEl.Descendants() select el.Value)
      };
    }
  }
}