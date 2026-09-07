# SSI Technical Assessment - Tenzin Tsundue
Tools used:
- Visual Studio 2019
- SQL Server Management Studio 22

Prerequisites:
 - .NET 5.0
 - SQL Server



## Layout

### SSITechnicalAssessment
This is the console version of application. It requests an EDI 837 file path from the user, parses it for CLM segments, and displays them in a readable format within the console.

Output/..
 - OutputWriter.cs: Class object that takes responsibility for outputting all parsed claims' information to the console.

 EDI837Files/..
  - Folder to hold sample .837 files.

How to run in CLI/Terminal:
1. From the root
```
cd SSITechnicalAssessment
```

2. Run 
```
dotnet run
```

3. Provide a file path to an .837 file (or use the sample file located in EDI837Files/SampleProfessional.837)

4. Claim info will display


### SSITechnicalAssessment.Tests
This is the Unit Test Suite. It tests file paths (ParseFileTests.cs) and file parsing for CLM Segments (ParseSegmentTests.cs).

How to run in CLI/Terminal:
1. From the root
```
cd SSITechnicalAssessment.Tests
```

2. Run 
```
dotnet test
```

3. All test case results will display


### SSITechnicalAssessment.Shared
This is a shared class library that stores Claim and Facility Models (located in Models/ClaimModels/). These Models are shared across all other projects. 

Models/ClaimModel/..
 - Claim.cs: Claim Models represent the Claim information from CLM segments.
 - Facility.cs: Facility Models represent the composite element within a CLM segment.
 
 Services/..
  - EDI837Parser.cs: This library also contains the EDI 837 parser logic. This is shared and used across all other projects.


### CreateTables.sql
This is a sql script to built the Claim and Facility Tables that are used in the web app (SSITechnicalAssessment.WebApp).


### SSITechnicalAssessment.WebApp
This is the web version of the application. It utilizes Razor web pages to build a user interface.

Users can upload .837 files to a SQL Server Database.

Users can also view all Claim information from that database in a concise table.

How to run in CLI/Terminal:
1. Ensure that you have a SQL Server Database instance already set up and are connected. You cannot move on without this. If not, Please see "SQL Server Database" under "Setup" below.

2. From the root
```
cd SSITechnicalAssessment.WebApp
```

3. Run 
```
dotnet run
```

4. FIRST TIME RUNNING APP: You may see a certificate error. This is because the application is serving in HTTPS. Follow the steps below to resolve the issue:

	a. Generate a developer certificate: 
	```
	dotnet dev-certs https
	```

	b. Add the certificate to the tristed root store:
	 ```
	 dotnet dev-certs https --trust
	 ```

5. There will be two separate urls that show up in the console prefixed by "NOW listening on: ...". Choose any one and load it in your browser (i.e. https://localhost:5001)

6. You will be met with a UI where you can view existing claim info within the database and have the option of uploading an .837 file to add more claim info.



## Setup For Web Application

### Install Microsoft.Data.SQLClient
This tool allows for us to establish a connection between our database and web application.

To install via Visual Studio:
1. Right Click the `SSITechnicalAssessment` project and Select Manage NuGet packages

2. Search for `Microsoft.Data.SQLClient` and choose version 5.0.2 from the dropdown

3. Hit Install

The package should be installed now.


### SQL Server Database
1. Create a SQL Server Instance using SSMS. Pick any Authentication Type that you like (Windows Auth, SQL Server Auth, etc.)

2. Initialize a Database in that instance (i.e. SSITechnicalAssessmentDB)

3. Locate the connection string to that database and copy it

4. Go to `SSITechnicalAssessment.WebApp/appsettings.json`
5. In appsettings.json, Paste the database name and connection string as the key-value pair inside "ConnectionStrings". It should look something like `{"ConnectionStrings": {"db-name": "conn-string"} ...}`
	
	a. IF your SQL Server enforces encryption, Modify your connection string to include "TrustServerCertificate=True" -> This avoids any SQL certificate trust errors.
	
	b. IF you are using SQL Server Authentication as well, Ensure that you fill in the "password=" value within your connection string (it may be '****' or something similar)

6. Open `SSITechnicalAssessment/CreateTables.sql` in SMSS, connected to your database.

	a. File > Open > [Locate `CreateTables.sql`]

7. Execute the script and your Tables should be built in your database.

You are now ready to launch the Web Application!
