Selenium Results Parser (Play 1.2.4)
===========================
This repository contains a parser for the Selenium HTML resultfiles which are generated via the Play Framework (Java, 1.2.4). The need was created because the HTML files are not readable as build metadata for TeamCity.

Useage of the (compiled) TeamCity DataImporter tool is easy; run it as build step in TeamCity with the working directory pointing to your Play Framework root directory and it will output TeamCity Service messages to the console which TeamCity will interpret.

```
C:\TeamCityAgent\WorkDir\1ecfe9489fb51e4869af\MyPlayProject C:\TeamCityTools\SeleniumImporter\PlaySeleniumDataImporter.exe
```

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
This software is licensed under GNU GPL-3.0