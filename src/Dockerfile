FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["calculadora-sqia/Adapter/Driver/calculadora-sqia.API/calculadora-sqia.API.csproj", "calculadora-sqia.API/"]
COPY ["calculadora-sqia/Core/calculadora-sqia.Application/calculadora-sqia.Application.csproj", "calculadora-sqia.Application/"]
COPY ["calculadora-sqia/Adapter/Driven/calculadora-sqia.SQLServer/calculadora-sqia.SQLServer.csproj", "calculadora-sqia.SQLServer/"]
COPY ["calculadora-sqia/Core/calculadora-sqia.Core/calculadora-sqia.Core.csproj", "calculadora-sqia.Core/"]
COPY ["calculadora-sqia/Core/calculadora-sqia.Domain/calculadora-sqia.Domain.csproj", "calculadora-sqia.Domain/"]


RUN dotnet restore "calculadora-sqia.API/calculadora-sqia.API.csproj"

COPY . .

WORKDIR "calculadora-sqia/Adapter/Driver/calculadora-sqia.API"
RUN dotnet publish "calculadora-sqia.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "calculadora-sqia.API.dll"]