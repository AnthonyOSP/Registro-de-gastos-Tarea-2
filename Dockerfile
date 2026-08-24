
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Registro de gastos.csproj", "./"]
RUN dotnet restore "Registro de gastos.csproj"

COPY . .
RUN dotnet publish "Registro de gastos.csproj" -c Release -o /app/publish --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV PORT=8080
EXPOSE 8080

# ASPNETCORE_HTTP_PORTS (no ASPNETCORE_URLS) porque la imagen base ya define
# ASPNETCORE_HTTP_PORTS=8080 por su cuenta; usar ambas variables a la vez hace
# que el host tire una advertencia de que una pisa a la otra.
ENTRYPOINT ["sh", "-c", "ASPNETCORE_HTTP_PORTS=$PORT dotnet \"Registro de gastos.dll\""]
