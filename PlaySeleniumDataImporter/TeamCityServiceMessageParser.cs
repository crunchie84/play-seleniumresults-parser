using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaySeleniumDataImporter
{
  public static class TeamCityServiceMessageParser
  {
    public static string Escape(string input)
    {
      if(!string.IsNullOrEmpty(input))
        return input.Replace("|", "||").Replace(Environment.NewLine, "|r|n").Replace("'", "|'").Replace("[", "|[").Replace("]", "|]");
      return string.Empty;
    }
  }
}
