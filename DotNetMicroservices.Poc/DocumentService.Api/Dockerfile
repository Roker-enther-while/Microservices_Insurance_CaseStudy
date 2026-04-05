FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME

COPY PolicyService.Api/*.csproj $APP_HOME/PolicyService.Api/
COPY DocumentService.Api/*.csproj $APP_HOME/DocumentService.Api/
RUN cd $APP_HOME/DocumentService.Api && dotnet restore

COPY PolicyService.Api $APP_HOME/PolicyService.Api/
COPY DocumentService.Api $APP_HOME/DocumentService.Api/
RUN cd $APP_HOME/DocumentService.Api && dotnet build

FROM build AS publish
WORKDIR $APP_HOME/DocumentService.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5080
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /opt/app/DocumentService.Api/out ./
ENTRYPOINT ["dotnet", "DocumentService.Api.dll"]
