## A practice exercise to test replication from RavenDB to SQL Server for a simple single-document/table example. 

Populates a very simple RavenDB entity "Minions" with test documents, sets up a Sql replication profile for that entity which then executes against the target Sql table of the same name.

#### This sample requires the following prerequisites:
1. A RavenDB instance running locally on port 8080, containing a database called "MyLittleDatabase" with SQL Replication bundle enabled
2. A SQL Server instance running on localhost with a database called "RobotMinions"
3. A SQL login with write access to the "RobotMinions" database which matches the identity under which the RavenDB instance is running
