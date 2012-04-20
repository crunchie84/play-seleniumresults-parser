Selenium Results Parser (Play 1.2.4)
===========================
This repository contains:
  - Library to parse the Selenium HTML resultfiles to metadata c# Objects;
  - A console app which uses this metadata to inform TeamCity of Selenium Tests + Results (passed/failed).

The Selenium HTML results are generated via the Play Framework (Java, 1.2.4) when running the command:
```
play.bat auto-test
```

The need was created because these HTML files are not readable as build metadata for TeamCity.

Requirements
===========================
Tested and granted the official Works On My Machine Seal of Approval:
  - Visual Studio 2010 to compile
  - .Net4 to run

Usage
===========================
Usage of the (compiled) TeamCity DataImporter tool is easy; run it as build step in TeamCity with the working directory pointing to your Play Framework root directory and it will output TeamCity Service messages to the console which TeamCity will interpret.

```
C:\TeamCityAgent\WorkDir\1ecfe9489fb51e4869af\MyPlayProject C:\TeamCityTools\SeleniumImporter\PlaySeleniumDataImporter.exe
```

Contents
===========================

Included in this repository are the following c# projects:

SeleniumResultsParser 
-------------
Library which contains the logic for finding & parsing of Selenium Testresult files to IEnumerable<TestResult> objects

UnitTests 
------------- 
Tests to verify library works as intended

PlaySeleniumDataImporter
-------------
Console application which interacts with the library and outputs [TeamCity Service Messages](http://confluence.jetbrains.net/display/TCD7/Build+Script+Interaction+with+TeamCity)

License information
===========================
This tool & parser were created by [Mark van Straten - Q42.nl](http://www.q42.nl/mark-van-straten and is hosted at [https://github.com/crunchie84/play-seleniumresults-parser/downloads](https://github.com/crunchie84/play-seleniumresults-parser/downloads)
This software is kindly granted to the community and licensed under GNU GPL-3.0 - see supplied LICENSE.TXT for details