# TMSBlazor

First install the following;

Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Microsoft.EntityFrameworkCore.Tools


update app.json:
,"ConnectionStrings": {
    "TMSconn":"Server=bookclubsystem.database.windows.net;Database=BCS;Trusted_Connection=false;Encrypt=True;User ID=bcs;Password=password1!; Integrated Security=false;"
  }


,"ConnectionStrings": {
    "TMSconn":"Server=bookclubsystem.database.windows.net;Database=BCS;Trusted_Connection=false;Encrypt=True;User ID=bcs;Password=password1!; MultipleActiveResultSets=true"
  }




Then run;
-- specific tables

Scaffold-DbContext -connection name=TMSconn Microsoft.EntityFrameworkCore.SqlServer -OutputDir Repo/Models -table User,Club  -context TMSDbContext -contextDir Repo -DataAnnotations -F 

--what I used:
Scaffold-DbContext -connection name=TMSconn Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data -table User,Club  -context TMSDbContext -contextDir -F 

--all tables
Scaffold-DbContext -connection name=TMSconn Microsoft.EntityFrameworkCore.SqlServer -OutputDir Repos/Models  -context TMSDbContext -contextDir Repos -DataAnnotations -F


--to update the project if there is any db change run the same thing:
Scaffold-DbContext -connection name=TMSconn Microsoft.EntityFrameworkCore.SqlServer -OutputDir Repo/Models -table User,Club  -context TMSDbContext -contextDir Repo -DataAnnotations -F


then we have to apply dependeny injection in program.cs bcs we do not want the connection to be inside db context


Then to update app.json connection string update the app.json as


Then run the following;
Scaffold-DbContext -Connection Name=BCSconn Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -F

