FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY src/Cookbook ./src/Cookbook
WORKDIR /app/src/Cookbook

RUN dotnet restore Cookbook.sln

RUN dotnet new tool-manifest --force
RUN dotnet tool install dotnet-ef --version 8.0.0

WORKDIR /app/src/Cookbook/CookbookWebApi
EXPOSE 5000

CMD ["sh", "-c", "dotnet tool run dotnet-ef database update && dotnet run --urls=http://0.0.0.0:5000"]