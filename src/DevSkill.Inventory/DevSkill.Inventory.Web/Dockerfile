FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN apt update && apt install -y nodejs

COPY ["DevSkill.Inventory.Worker/*.csproj", "DevSkill.Inventory.Worker/"]
COPY ["DevSkill.Inventory.Application/*.csproj", "DevSkill.Inventory.Application/"]
COPY ["DevSkill.Inventory.Domain/*.csproj", "DevSkill.Inventory.Domain/"]
COPY ["DevSkill.Inventory.Infrastructure/*.csproj", "DevSkill.Inventory.Infrastructure/"]

RUN dotnet restore "DevSkill.Inventory.Worker/DevSkill.Inventory.Worker.csproj"

COPY . .

WORKDIR "/src/DevSkill.Inventory.Worker"
RUN dotnet build "DevSkill.Inventory.Worker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DevSkill.Inventory.Worker.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "DevSkill.Inventory.Worker.dll"]
