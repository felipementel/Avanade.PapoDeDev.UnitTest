# Avanade.PapoDeDev.UnitTest

## Pré-requisitos

1. dotnet tool install --global dotnet-reportgenerator-globaltool

2. https://marketplace.visualstudio.com/items?itemName=FortuneNgwenya.FineCodeCoverage

## Needs to know

1. https://github.com/danielpalme/ReportGenerator#getting-started
2. https://danielpalme.github.io/ReportGenerator/

### Libraries
1. https://github.com/xunit/xunit
2. https://github.com/Moq/moq4/wiki/Quickstart
3. https://github.com/bchavez/Bogus

## How to run
 ..\src\Avanade.PapoDeDev.UnitTest.API> dotnet run --configuration Release
## How to run local test

> Access the src folder

> Access the src folder

> first command
>
>   ..\src\Avanade.PapoDeDev.UnitTest.Domain.Test>  dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=CoverageResults/


> seccond command   
    ..\src\Avanade.PapoDeDev.UnitTest.Domain.Test> dotnet C:\Users\f.pimentel.augusto\.nuget\packages\reportgenerator\4.8.7\tools\net5.0\ReportGenerator.dll "-reports:coverage.opencover.xml" "-targetdir:coveragereport" -reporttypes:HtmlInline_AzurePipelines_Dark;Latex "-historydir:history" "-assemblyfilters:" "-title:Project Price Announcement Report"