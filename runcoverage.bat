mkdir coverage

packages\OpenCover.4.6.166\tools\OpenCover.Console.exe -mergeoutput -register:user -excludebyattribute:*.ExcludeFromCodeCoverage*^ -target:"packages\NUnit.Runners.2.6.4\tools\nunit-console.exe" "-targetargs:\"SimpleInjector.NET.Tests.Unit\bin\Debug\SimpleInjector.Tests.Unit.dll\" -noshadow" -filter:"+[*]SimpleInjector.* -[*.Tests.*]*" -output:coverage\coverage.xml

packages\ReportGenerator.2.3.2.0\tools\ReportGenerator.exe "-reports:coverage\coverage.xml" "-targetdir:coverage\report" "-filters:-*.Tests*;" "-historydir:coverage\history"

coverage\report\index.htm