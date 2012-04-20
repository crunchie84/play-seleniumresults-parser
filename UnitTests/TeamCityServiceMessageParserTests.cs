using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using SeleniumResultsParser;
using System.Xml;
using System.Xml.Linq;
using PlaySeleniumDataImporter;

namespace UnitTests
{
  /// <summary>
  /// There are a few escaping requirements for the service messages. see http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity
  /// </summary>
  [TestFixture]
  public class TeamCityServiceMessageParserTests
  {
    [Test]
    public void FunkyCharacters_ShouldBeEscaped_Correctly()
    {
      string input    = "first line\r\nsecond line apostrope=' verticalbar=| openingbracket=[ closingbracket=]";
      string expected = "first line|r|nsecond line apostrope=|' verticalbar=|| openingbracket=|[ closingbracket=|]";
      string output = TeamCityServiceMessageParser.Escape(input);
      Assert.AreEqual(expected, output, "Special chars for service message should be escaped with | chars");
    }
  }
}