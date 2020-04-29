using System;
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
            "INSERT INTO tcup (id,displayname,connected) Values (@Id,'SomeRandomCupName',true)";

        private string sqlCheckCup = "SELECT * FROM tcup WHERE id = @Id";
        private string sqlCupConnected = "UPDATE tcup SET connected = true WHERE id = @Id";
        private string sqlGetCups = "SELECT * FROM tcup";
        private string sqlInsertTemperature = "INSERT INTO ttemperature (id, temp) values (@Id, @Temp)";
        private string sqlCupDisconnected = "UPDATE tcup SET connected = false WHERE id = @Id";
        private string sqlCupUpdate = "UPDATE tcup SET displayname = @DisplayName, mintemp = @MinTemp, maxtemp = @MaxTemp WHERE id = @Id";
        
        private JsonSerializerOptions _jsonOptions;

        public DbService()
        {
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        private NpgsqlConnection GetDbConnection()
        {
            return new NpgsqlConnection("User ID=postgres;Password=dininfo1;Host=167.172.184.103;Database=postgres;Port=5432");
        }
        
        public Cup ConnectCup(string jsonStr)
        {
            var cup = JsonSerializer.Deserialize<Cup>(jsonStr, _jsonOptions);
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

            return cup;
        }

        public void InsertTemperature(string jsonStr)
        {
            var temp = JsonSerializer.Deserialize<TemperatureTO>(jsonStr,_jsonOptions);
            var temperature = temp.Transform();
            using var conn = GetDbConnection();
            conn.Execute(sqlInsertTemperature, temperature);
        }

        public void DisconnectCup(string jsonStr)
        {
            var cup = JsonSerializer.Deserialize<Cup>(jsonStr, _jsonOptions);
            using (var connection = GetDbConnection())
            {
                var existingCup = connection.QueryFirstOrDefault<Cup>(sqlCheckCup, cup);
                if (existingCup != null)
                {
                    connection.Execute(sqlCupDisconnected, cup);
                }
                else
                {
                    connection.Execute(sqlCupInsert, cup);
                    connection.Execute(sqlCupDisconnected, cup);
                }
            }
        }

        public Cup GetCup(string id)
        {
            using (var connection = GetDbConnection())
            {
                var parameters = new {Id = id};
                return connection.QueryFirstOrDefault<Cup>(sqlCheckCup,parameters);
            }
        }

        public void UpdateCup(string id, CupFormData cup)
        {
            using (var connection = GetDbConnection())
            {
                var parameters = new {Id = id,DisplayName = cup.InputName, MaxTemp = cup.MaxTemp, MinTemp = cup.MinTemp};
                connection.Execute(sqlCupUpdate, parameters);
            }
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