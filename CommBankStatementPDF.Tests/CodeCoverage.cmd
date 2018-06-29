@echo off
@echo off

cls
cls


REM "..\..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -target:"..\..\..\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" -filter:"+[CommBankStatementPDF.Business]CommBankStatementPDF.Business*" -targetargs:"CommBankStatementPDF.Tests.dll " -output:".\CodeCoverage\_CodeCoverageResult.xml"

"..\..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -target:"..\..\..\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" -filter:"+[*]CommBankStatementPDF.*" -targetargs:"CommBankStatementPDF.Tests.dll " -output:"_CodeCoverageResult.xml" -register:user


rem -filter:"+[ProjToTest]ProjToTest*" -excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" -register:user -output:"_CodeCoverageResult.xml"



