using System.Collections.Generic;
using System.Text.Json;
using Dapper;
using Npgsql;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class DbService : IDbService
    {
        
        private string sqlCupInsert =
            "INSERT INTO tcup (id,display_name,connected) Values (@Id,'SomeRandomCupName',true)";

        private string sqlCheckCup = "SELECT * FROM tcup WHERE id = @Id";
        private string sqlCupConnected = "UPDATE tcup SET connected = true WHERE id = @Id";
        private string sqlGetCups = "SELECT * FROM tcup";
        private string InsertTemperature = "INSERT INTO ttemperature (id, tvalue) values (@Id, @Tvalue)";
        
        
        private NpgsqlConnection GetDbConnection()
        {
            return new NpgsqlConnection("User ID=postgres;Password=dininfo1;Host=167.172.184.103;Database=postgres;Port=5432");
        }
        public void ConnectCup(string jsonStr)
        {
            var cup = JsonSerializer.Deserialize<Cup>(jsonStr);
            using (var connection = GetDbConnection())
            {
                var existingCup = connection.QueryFirstOrDefault<Cup>(sqlCheckCup, cup);
                if (existingCup != null)
                {
                    var affectedRows = connection.Execute(sqlCupConnected, cup);
                }
                else
                {
                    var affectedRows = connection.Execute(sqlCupInsert, cup);
                }
            }
        }

        public void Temperature(string jsonStr)
        {
            var temperature = JsonSerializer.Deserialize<Temperature>(jsonStr);
            
            using var conn = GetDbConnection();
            conn.Execute(InsertTemperature, temperature);


        }

        public IEnumerable<Cup> GetCups()
        {
            using (var connection = GetDbConnection())
            {
                var enumerable = connection.Query<Cup>(sqlGetCups);
                return enumerable;
            }
        }
    }
}