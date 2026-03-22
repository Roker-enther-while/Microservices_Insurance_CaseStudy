@echo off
echo Building Microservices...

call dotnet build ../ProductService.Api/ProductService.Api.csproj
call dotnet build ../PricingService.Api/PricingService.Api.csproj
call dotnet build ../PaymentService.Api/PaymentService.Api.csproj

echo Building Application Services...
call dotnet build ../ProductService/ProductService.csproj
call dotnet build ../PricingService/PricingService.csproj
call dotnet build ../PaymentService/PaymentService.csproj

echo Building API Gateway and Client...
call dotnet build ../AgentPortalApiGateway/AgentPortalApiGateway.csproj
call dotnet build ../BlazorWasmClient/BlazorWasmClient.csproj

echo Building Tests...
call dotnet build ../ProductService.Test/ProductService.Test.csproj
call dotnet build ../PricingService.Test/PricingService.Test.csproj
call dotnet build ../PaymentService.Test/PaymentService.Test.csproj

echo Running Tests...
call dotnet test ../ProductService.Test/ProductService.Test.csproj
call dotnet test ../PricingService.Test/PricingService.Test.csproj
call dotnet test ../PaymentService.Test/PaymentService.Test.csproj

echo Done.