using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeleniumResultsParser
{
  /// <summary>
  /// Results parsed of a single testcase in a suite
  /// </summary>
  public class TestResult
  {    
    /// <summary>
    /// Title of this testcase
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Did it pass
    /// </summary>
    public bool Passed { get; set; }
    /// <summary>
    /// ErrorMessage / step of test which failed
    /// </summary>
    public string ErrorMessage { get; set; }
  }
}
