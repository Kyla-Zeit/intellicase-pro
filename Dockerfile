FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/IntelliCasePro.Web/IntelliCasePro.Web.csproj", "src/IntelliCasePro.Web/"]
RUN dotnet restore "src/IntelliCasePro.Web/IntelliCasePro.Web.csproj"

COPY . .
WORKDIR /src/src/IntelliCasePro.Web
RUN dotnet publish "IntelliCasePro.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN mkdir -p /app/data

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection=Data Source=/app/data/intellicasepro.db

EXPOSE 8080

ENTRYPOINT ["dotnet", "IntelliCasePro.Web.dll"]
