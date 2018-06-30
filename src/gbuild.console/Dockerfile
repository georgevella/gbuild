FROM microsoft/dotnet:2.1-sdk as build

WORKDIR /src

COPY GBuild.sln ./
COPY gbuild.console/gbuild.console.csproj ./gbild.console.csproj
COPY gbuild.core/gbuild.core.csproj ./gbild.core.csproj

RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /out

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "gbuild.console.dll"]