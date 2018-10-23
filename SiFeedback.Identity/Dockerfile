FROM microsoft/dotnet:2.1-sdk AS build-env
#BUILD 
WORKDIR /sifeedback

#restore
COPY SiFeedback.Identity.csproj ./
RUN dotnet restore SiFeedback.Identity.csproj

#copy src
COPY . .

#publish
RUN dotnet publish ./SiFeedback.Identity.csproj -o  /publish

#RUNTIME
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
COPY  --from=build-env /publish /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "SiFeedback.Identity.dll"]