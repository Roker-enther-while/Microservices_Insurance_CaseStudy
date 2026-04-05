$basePath = "c:\Users\dhp01\source\repos\Roker-enther-while\Microservices_Insurance_CaseStudy\DotNetMicroservices.Poc"

Start-Process "cmd.exe" -ArgumentList "/k title Eureka Server && cd /d $basePath\eureka-server && gradlew.bat bootRun"
Start-Process "cmd.exe" -ArgumentList "/k title Pricing Service && cd /d $basePath\PricingService && dotnet run"
Start-Process "cmd.exe" -ArgumentList "/k title Policy Service && cd /d $basePath\PolicyService && dotnet run"
Start-Process "cmd.exe" -ArgumentList "/k title Policy Search Service && cd /d $basePath\PolicySearchService && dotnet run"
Start-Process "cmd.exe" -ArgumentList "/k title Document Service && cd /d $basePath\DocumentService.Api && dotnet run"
