# PK-Zywave-RateLimiter
Zywave coding challenge

Project Overview
"This is an API to limit the number of request in perticular duration. Different limits can be set to different users"

Key features of this project include:

Architecture: Implements Clean Architecture(Onion) and caching for maintainability and performance.
Data Access:  In-Memory Database (Static collection).
monitoring: Logs have been maintained in the local text file.
unit testing is implemented to ensure code quality.

Tech Stack
Framework: .NET 8.0
Language: C# 
Database: Im-Memory Database, Static collection
Testing:  NUnit, Moq 
Logging : local text file [Filename and path: c:/LogyyyyMMddhhmmss].

Getting Started
1. Prerequisites: Ensure .NET SDK 9.0+ is installed.
2. Visual studio 2022
3. Clone the repo
4. Run Visual studio in Admin Mode [Required to write log file on C: drive]
5. Set Starting Project- RateLimiterWeb
6. Run the project - Swagger page will open with two API endpoints
   a. api/RateLimiter/ValidateRequest
       Accepts Username
       returns satuscode 200 and 429 depending upon Rate limits
   Example :[User1, user2 and user3 already exist in DB]
             provide any one of them
             Also once added few more user you can use them to validate rate as per limits given
   b. api/RateLimiter/UpdateRate
       Accepts RateParams objects with Username and limits
       returns statuscode 200 and 500 as per result.
       This can be used to Update  record if exists or will create new one for given UserName.
   Example : {
                "userName": "string",
                "capacity": 0, //integer - maximum number of requests  
                "duration": 0 //Timelimit in seconds
              }




