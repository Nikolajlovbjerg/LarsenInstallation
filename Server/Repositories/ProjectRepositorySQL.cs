using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Server.PW1;
using Microsoft.Extensions.Configuration;
using Core;

namespace Server.Repositories
{
    public class ProjectRepositorySQL : IProjectRepository
    {
        private readonly string _conString =
            "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
            "Port=5432;" +
            "Database=LarsenInstallation;" +
            "Username=neondb_owner;" +
            $"Password={PASSWORD.PW1};" +
            "Ssl Mode=Require;" +
            "Trust Server Certificate=true;";

        public async Task<int> CreateProjectAsync(Project project)
        {
            using var conn = new NpgsqlConnection(_conString);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO projects (name, datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbjedsmand_timepris)
                                VALUES (@name, @datecreated, @svend, @lærling, @konsulent, @arbejdsmand)
                                RETURNING projectid;";
            cmd.Parameters.AddWithValue("name", project.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("datecreated", project.DateCreated == default ? DateTime.UtcNow : project.DateCreated);
            cmd.Parameters.AddWithValue("svend", project.SvendTimePris);
            cmd.Parameters.AddWithValue("lærling", project.LærlingTimePris);
            cmd.Parameters.AddWithValue("konsulent", project.KonsulentTimePris);
            cmd.Parameters.AddWithValue("arbejdsmand", project.ArbjedsmandTimePris);
            var res = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(res);
        }

        public async Task CreateProjectWithDataAsync(Project project, DataTable? hoursTable, DataTable? materialsTable)
        {
            using var conn = new NpgsqlConnection(_conString);
            await conn.OpenAsync();
            using var tx = await conn.BeginTransactionAsync();
            try
            {
                int projectId;
                // Insert project
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"INSERT INTO projects (name, datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbjedsmand_timepris)
                                        VALUES (@name, @datecreated, @svend, @lærling, @konsulent, @arbejdsmand)
                                        RETURNING projectid;";
                    cmd.Parameters.AddWithValue("name", project.Name ?? string.Empty);
                    cmd.Parameters.AddWithValue("datecreated", project.DateCreated == default ? DateTime.UtcNow : project.DateCreated);
                    cmd.Parameters.AddWithValue("svend", project.SvendTimePris);
                    cmd.Parameters.AddWithValue("lærling", project.LærlingTimePris);
                    cmd.Parameters.AddWithValue("konsulent", project.KonsulentTimePris);
                    cmd.Parameters.AddWithValue("arbejdsmand", project.ArbjedsmandTimePris);
                    projectId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                // Insert hours
                if (hoursTable != null && hoursTable.Rows.Count > 0)
                {
                    foreach (DataRow row in hoursTable.Rows)
                    {
                        DateTime? dato = TryParseDate(hoursTable, row, "Dato");
                        DateTime? stoptid = TryParseDate(hoursTable, row, "Stoptid");
                        decimal timer = TryParseDecimal(hoursTable, row, "Timer");
                        var type = hoursTable.Columns.Contains("Type") ? row["Type"]?.ToString() : null;
                        decimal kostpris = TryParseDecimal(hoursTable, row, "Kostpris");

                        using var cmd = conn.CreateCommand();
                        cmd.Transaction = tx;
                        cmd.CommandText = @"INSERT INTO projecthours (projectid, dato, stoptid, timer, type, kostpris, raw_row)
                                            VALUES (@projectid,@dato,@stoptid,@timer,@type,@kostpris,@rawrow)";
                        cmd.Parameters.AddWithValue("projectid", projectId);
                        cmd.Parameters.AddWithValue("dato", (object)dato ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("stoptid", (object)stoptid ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("timer", timer);
                        cmd.Parameters.AddWithValue("type", (object)type ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("kostpris", kostpris);
                        cmd.Parameters.AddWithValue("rawrow", string.Join("|", row.ItemArray.Select(x => x?.ToString() ?? "")));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // Insert materials
                if (materialsTable != null && materialsTable.Rows.Count > 0)
                {
                    foreach (DataRow row in materialsTable.Rows)
                    {
                        var beskrivelse = materialsTable.Columns.Contains("Beskrivelse") ? row["Beskrivelse"]?.ToString() : null;
                        decimal kostpris = TryParseDecimal(materialsTable, row, "Kostpris");
                        decimal antal = TryParseDecimal(materialsTable, row, "Antal");
                        decimal total = TryParseDecimal(materialsTable, row, "Total");
                        decimal avance = TryParseDecimal(materialsTable, row, "Avance.1");
                        decimal dækningsgrad = TryParseDecimal(materialsTable, row, "Dækningsgrad");

                        using var cmd = conn.CreateCommand();
                        cmd.Transaction = tx;
                        cmd.CommandText = @"INSERT INTO projectmaterials (projectid, beskrivelse, kostpris, antal, total, avance, dækningsgrad, raw_row)
                                            VALUES (@projectid,@beskrivelse,@kostpris,@antal,@total,@avance,@dækningsgrad,@rawrow)";
                        cmd.Parameters.AddWithValue("projectid", projectId);
                        cmd.Parameters.AddWithValue("beskrivelse", (object)beskrivelse ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("kostpris", kostpris);
                        cmd.Parameters.AddWithValue("antal", antal);
                        cmd.Parameters.AddWithValue("total", total);
                        cmd.Parameters.AddWithValue("avance", avance);
                        cmd.Parameters.AddWithValue("dækningsgrad", dækningsgrad);
                        cmd.Parameters.AddWithValue("rawrow", string.Join("|", row.ItemArray.Select(x => x?.ToString() ?? "")));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // Simple calculation and insert to calculations
                decimal totalMaterialCost = 0;
                decimal totalHourlyCost = 0;

                using (var cmdMat = conn.CreateCommand())
                {
                    cmdMat.Transaction = tx;
                    cmdMat.CommandText = "SELECT COALESCE(SUM(total),0) FROM projectmaterials WHERE projectid=@pid";
                    cmdMat.Parameters.AddWithValue("pid", projectId);
                    var matObj = await cmdMat.ExecuteScalarAsync();
                    totalMaterialCost = Convert.ToDecimal(matObj ?? 0);
                }

                using (var cmdHour = conn.CreateCommand())
                {
                    cmdHour.Transaction = tx;
                    cmdHour.CommandText = "SELECT COALESCE(SUM(timer * COALESCE(kostpris,0)),0) FROM projecthours WHERE projectid=@pid";
                    cmdHour.Parameters.AddWithValue("pid", projectId);
                    var hourObj = await cmdHour.ExecuteScalarAsync();
                    totalHourlyCost = Convert.ToDecimal(hourObj ?? 0);
                }

                using (var cmdCalc = conn.CreateCommand())
                {
                    cmdCalc.Transaction = tx;
                    cmdCalc.CommandText = @"INSERT INTO calculations (projectid, total_material_cost, total_hourly_cost, total_customer_price, total_earnings)
                                            VALUES (@pid,@mat,@hour,@cust,@earnings)";
                    decimal totalCustomerPrice = totalMaterialCost + totalHourlyCost;
                    decimal totalEarnings = 0;
                    cmdCalc.Parameters.AddWithValue("pid", projectId);
                    cmdCalc.Parameters.AddWithValue("mat", totalMaterialCost);
                    cmdCalc.Parameters.AddWithValue("hour", totalHourlyCost);
                    cmdCalc.Parameters.AddWithValue("cust", totalCustomerPrice);
                    cmdCalc.Parameters.AddWithValue("earnings", totalEarnings);
                    await cmdCalc.ExecuteNonQueryAsync();
                }

                await tx.CommitAsync();
            }
            catch
            {
                try { await tx.RollbackAsync(); } catch { }
                throw;
            }
        }

        private static DateTime? TryParseDate(DataTable dt, DataRow row, string columnName)
        {
            if (!dt.Columns.Contains(columnName)) return null;
            var obj = row[columnName];
            if (obj == null || obj == DBNull.Value) return null;
            if (DateTime.TryParse(obj.ToString(), out var d)) return d;
            // Excel may store as double (OADate)
            if (double.TryParse(obj.ToString(), out var od) && od > 0)
            {
                try { return DateTime.FromOADate(od); } catch { }
            }
            return null;
        }

        private static decimal TryParseDecimal(DataTable dt, DataRow row, string columnName)
        {
            if (!dt.Columns.Contains(columnName)) return 0;
            var obj = row[columnName];
            if (obj == null || obj == DBNull.Value) return 0;
            if (decimal.TryParse(obj.ToString(), out var d)) return d;
            if (double.TryParse(obj.ToString(), out var dd)) return Convert.ToDecimal(dd);
            return 0;
        }
    }
}
