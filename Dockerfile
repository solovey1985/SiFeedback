FROM microsoft/dotnet:2.1-sdk AS build-env
#BUILD 
WORKDIR /sifeedback

#restore
COPY SiFeedback.Web/SiFeedback.Web.csproj ./SiFeedback.Web/
RUN dotnet restore SiFeedback.Web/SiFeedback.Web.csproj
COPY SiFeedback.Tests/SiFeedback.Tests.csproj ./SiFeedback.Tests/
RUN dotnet restore SiFeedback.Tests/SiFeedback.Tests.csproj

#install Node.js
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash - && apt-get install -y nodejs

#copy src
COPY . .

#test
#ENV TEAMCITY_PROJECT_NAME=notfake
#RUN dotnet test SiFeedback.Tests/SiFeedback.Tests.csproj

#publish
RUN dotnet publish SiFeedback.Web/SiFeedback.Web.csproj -o  /publish

#RUNTIME
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
COPY  --from=build-env /publish /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "SiFeedback.Web.dll"]