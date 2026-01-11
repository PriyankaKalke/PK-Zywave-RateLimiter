# to run this dockerfile
# docker build -t RateLimiter:0.1 .  //this will create a docker image
# docker images   //verify image is created
# docker run -d -p 8080:8080 --name RateLimiterContainer RateLimiter:0.1 //
# docker ps   //to verify if container is running


#Build
FROM mcr.microsoft.com/dotnet/sdk:8.0
EXPOSE 8080

#setting working Directory in container
WORKDIR /src
COPY ["RateLimiterWeb.csproj", "."] 

#restoring packages
RUN dotnet restore "./RateLimiterWeb.csproj"

# copy everythig from project to container
COPY . .
# step into src Folder
WORKDIR "/src/."
RUN dotnet build "./RateLimiterWeb.csproj" -c Release -o /app/build

#run project
#RUN dotnet run 
CMD ["dotnet","run","--no-build","-c","Release"]

