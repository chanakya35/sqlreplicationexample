using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Database.Bundles.SqlReplication;

namespace SqlReplicationExample
{
    class Program
    {
        /// <summary>
        /// This sample requires the following prerequisites:
        /// (a) A RavenDB instance running locally on port 8080, containing a database called "MyLittleDatabase" with SQL Replication bundle enabled
        /// (b) A SQL Server instance running on localhost with a database called "RobotMinions"
        /// (c) A SQL login with write access to the "RobotMinions" database which matches the identity under which the RavenDB instance is running
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            CreateSqlTable.Run();
            InsertSampleRavenData();
            CreateAndRunReplicationProfile();
        }

        private static void CreateAndRunReplicationProfile()
        {
            using (var session = (new DocumentStore() {ConnectionStringName = "RavenDb"}.Initialize().OpenSession()))
            {
                session.Store(new SqlReplicationConfig
                {
                    Id = "Raven/SqlReplication/Configuration/Minions",
                    Name = "Minions",
                    ConnectionString = ConfigurationManager.ConnectionStrings["RobotMinions"].ConnectionString,
                    FactoryName = "System.Data.SqlClient",
                    RavenEntityName = "Minions",
                    SqlReplicationTables =
                    {
                        new SqlReplicationTable
                        {
                            TableName = "Minions",
                            DocumentKeyColumn = "Id"
                        }
                    },
                    Script = @"
                        var minionData = {
                            Id: documentId,
                            Name: this.Name,
                            Age: this.Age,
                            RegisterDateTime: this.RegisterDateTime
                        };

                        replicateToMinions(minionData);                        
                    "
                });
                session.SaveChanges();
            }
        }

        static void InsertSampleRavenData(int numberOfMinions = 10)
        {
            using (var session = (new DocumentStore() {ConnectionStringName = "RavenDb"}.Initialize().OpenSession()))
            {
                for (int i = 0; i < numberOfMinions; i++)
                {
                    session.Store(new Minion
                    {
                        Name = "Robie " + new Random().Next(),
                        Age = new Random().Next(23, 35),
                        RegisterDateTime = DateTime.UtcNow
                    });
                }
                session.SaveChanges();
            }
        }
    }
}
