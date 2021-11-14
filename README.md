# Admin System
A system for teachers to perform administrative functions for their students. Teachers and students are identified by their email addresses.


## Background
Web API: C#/ .NET Framework.

Database: Microsoft SQL Server.


## Steps to run the solution
1. Download the solution and open using Microsoft Visual Studio.
2. Generate database from the model (AdminSystemModel.edmx).
3. Run UnitTestProject followed by AdminSystem project to test the API endpoints.


## Assumptions
1. 400 Bad request will be returned when the request body is null.
2. 500 Internal server error will be returned when database errors or other errors occurred.
3. Teachers and students will be registered in database when the teacher register multiple students.
4. 400 Bad request will be returned when a teacher register the same student.
5. 400 Bad request will be returned when a teacher wanted to suspend the same student.
