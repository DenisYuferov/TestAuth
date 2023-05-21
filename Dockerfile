FROM mcr.microsoft.com/dotnet/sdk:7.0 AS StageLayer
WORKDIR /TestAuth
COPY . .
WORKDIR /TestAuth/TestAuth.WebApi/
RUN dotnet publish "TestAuth.WebApi.csproj" -c Release -r debian.10-x64 -o /out

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=StageLayer /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "TestAuth.WebApi.dll", "TestAuth"]