# Kitchen Manager
A web app that can track your pantry inventory, manage your shopping list, and help with meal prep and planning.

# Description
Note: This project is a pre-alpha work-in-progress, so as we continue to work on it it will grow in features and functionality.
This web application is a tool that will be helpful to track your inventory and manage your shopping list, and eventually help with meal prep and planning - essentially an all-in-one kitchen management platform.
We chose ASP.NET (Core) 5 and Entity Framework Core 5 for this project because they are relatively new with updated features, but have been around a while so we can get answers if we run into a problem.
It has been an interesting learning experience creating this project from scratch, taking aspects of many online tutorials and tips and putting them together with our vision of what we want it to be.

# To Run the Project
Download the project with your perferred version of Visual Studio.  
If you are missing any important components Visual Studio will prompt you to install them.  
You will need to create an appsettings.json file with the code below and put it in the root directory of the project next to the appsettings.Development.json file.  
```
{
  "Logging": {
    "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
    }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "KMData": "Data Source=YourDBHere;Initial Catalog=KitchenManagerDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    }
}
```
For Windows Users:  
Find the "KMData" property in appsettings.json and replace the YourDBHere part of "Data Source=YourDBHere; with the name of any SQL database installed on your machine which can be found in the SQL Server Object Explorer window.  
For MacOS Users:  
You will need to download a Docker SQL server container and use that instead. Find instructions for how to do that here: https://www.twilio.com/blog/using-sql-server-on-macos-with-docker  
Finally, click Run, and Visual Studo will automatically restore the nuGet packages and run the project.  
Currently the Front End is not implemented yet so the project will open to a SwaggerUI page.  
Here you can test API calls and retrieve, update, or delete the seeded data from the database using various API endpoints.  

# Contributions
Currently myself, Christian Carroll, and my collegue Quentin Thomas are the only two working on this project.  
I am focusing on the Back End/API side of the project and Quentin will be focusing on the React Front End, however we will be working closely with one another on both parts to ensure we are both happy with the code, and so we can both learn more about ASP.NET Core and React.

# Development Roadmap
- Finish API Back End
- Add Front End pages
- Add shopping list management
- Add meal planning calendar
- Add recipe managment
- Host web app online so anyone can access and use it
- polish look and feel
