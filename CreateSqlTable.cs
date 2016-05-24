using System.Runtime.InteropServices;

namespace SqlReplicationExample
{
    using System.Configuration;
    using System.Data.SqlClient;

    public class CreateSqlTable
    {
        public static void Run()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RobotMinions"].ConnectionString))
            {
                conn.Open();
                var isAlreadyRun = false;
                using (var cmd = new SqlCommand("SELECT TOP 1 1 FROM sys.objects WHERE Name = 'Minions'", conn))
                {
                    isAlreadyRun = (int)(cmd.ExecuteScalar() ?? 0) == 1;
                }
                if (!isAlreadyRun)
                {
                    using (var cmd = new SqlCommand(@"
                            CREATE TABLE Minions 
                            ( 
                                Id VARCHAR(50), 
                                Name VARCHAR(200), 
                                Age SMALLINT, RegisterDateTime DATETIME2, 
                                CONSTRAINT PK_Minions PRIMARY KEY (Id)
                            )", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}
