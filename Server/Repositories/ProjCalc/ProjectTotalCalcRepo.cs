using Server.PW1;
using Core;
using Npgsql;
using System;
using System.Xml.Linq;

namespace Server.Repositories.ProjCalc
{
    public class ProjectTotalCalcRepo : IProjectTotalCalcRepo
    {
        private const string conString =
    "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
    "Port=5432;" +
    "Database=LarsenInstallation;" +
    "Username=neondb_owner;" +
    $"Password={PASSWORD.PW1};" +
    "Ssl Mode=Require;" +
    "Trust Server Certificate=true;";
    

    public ProjectTotalCalcRepo()
        {

        }

        public List<Calculation> GetAll()
        {
            var result = new List<Calculation>();
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"select * from calculations";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var calcid = reader.GetInt32(0);
                        var projectid = reader.GetInt32(1);
                        var total_material_cost = reader.GetDecimal(2);
                        var total_hourly_cost = reader.GetDecimal(3);
                        var total_customer_price = reader.GetDecimal(4);
                        var total_earnings = reader.GetDecimal(5);
                        var created_at = reader.GetDateTime(6);

                        Calculation c = new Calculation
                        {
                            CalcId = calcid,
                            ProjectId = projectid,
                            TotalMaterialCost = total_material_cost,
                            TotalHourlyCost = total_hourly_cost,
                            TotalCustomerPrice = total_customer_price,
                            TotalEarnings = total_earnings,
                            CreatedAt = created_at
                        };
                        result.Add(c);
                    }
                }
            }
            return result;
        } 

    } 
}
