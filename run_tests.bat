rem Run tests with open cover
packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -filter:"+[*]*" -target:"nunit-console-x86 /noshadow /domain:single CGbR.Tests\bin\Release\CGbR.Tests.dll CGbR.GeneratorTests\bin\Release\CGbR.GeneratorTests.dll" -output:coverage.xml

rem Publish coverage report
packages\coveralls.io.1.3.4\tools\coveralls.net.exe --opencover coverage.xml