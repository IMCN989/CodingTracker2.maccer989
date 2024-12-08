using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DataAccessLibrary
{

    public class SqliteCrud
    {
        private readonly string _connectionString;
        private SqliteDataAccess db = new SqliteDataAccess();

        public SqliteCrud(string connectionString)
        {
            _connectionString = connectionString;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS CodingSession ( 
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTime TEXT,
                DurationInMinutes INTEGER,
                DateTime TEXT)";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void CreateContact(CodingSessionModel session)
        {
            // Save the session
            string sql = "insert into CodingSession (StartTime, EndTime, DurationInMinutes, DateTime) values (@StartTime, @EndTime, @DurationInMinutes, @DateTime);";
            db.SaveData(sql,
                        new { session.StartTime, session.EndTime, session.DurationInMinutes, session.DateTime},
                        _connectionString);
        }

        public List<CodingSessionModel> GetAllSessions()
        {
            string sql = "select Id, StartTime, EndTime, DurationInMinutes, DateTime from CodingSession";

            return db.LoadData<CodingSessionModel, dynamic>(sql, new { }, _connectionString);
        }

        public bool CheckRecordExists(int recordId)
        {
            bool output = false;
            string sql = $"select Id from CodingSession where Id = {recordId};";
            var checkQuery = db.LoadData<CodingSessionModel, dynamic>(sql,
                new { Id = recordId },
                _connectionString);

            if (checkQuery.Count > 0)
            {
                Console.WriteLine($"Record with ID {recordId} exists.");
                output = true;
            }
            else
            {
                Console.WriteLine($"Record with ID {recordId} does not exist.");
            }
            return output;

        }

        public void UpdateCodingSession( int recordId, string startTime, string endTime, int duration, string dateTime)
        {
            string sql = $"select exists (select 1 from CodingSession where Id = {recordId});";
            var checkQuery = db.LoadData<CodingSessionModel, dynamic>(sql,
                new { Id = recordId },
                _connectionString);

            if (checkQuery.Count == 0)
            {
                Console.WriteLine($"\n\nrecord with Id {recordId} doesnt exist.\n\n");
            }
            else
            {
                sql = $"update CodingSession set StartTime = '{startTime}', EndTime = '{endTime}', DurationInMinutes = '{duration}', DateTime = '{dateTime}'  where Id = '{recordId}'";
                db.SaveData(sql, recordId, _connectionString);
            }  
        }

        public void RemoveSession(int recordId)
        {
            string sql = $"select exists (select 1 from CodingSession where Id = {recordId});";
            var checkQuery = db.LoadData<CodingSessionModel, dynamic>(sql,
                new { Id = recordId },
                _connectionString);

            if (checkQuery.Count == 0)
            {
                Console.WriteLine($"\n\nrecord with Id {recordId} doesnt exist.\n\n");
            }
            else
            {
                sql = $"delete from CodingSession where Id = {recordId}";
                db.SaveData(sql, new { Id = recordId }, _connectionString);
            }
        }
    }
}