using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace server
{
    public class DbHelper
    {
        private readonly string _databaseFile;
        private readonly string _databaseFilePath = Directory.GetCurrentDirectory();
        private readonly string _connString;

     
        public DbHelper(string databaseFile)
        {
            _databaseFile = _databaseFilePath + @"\..\..\..\" + databaseFile;
            _connString = $"Data Source={databaseFile};Version=3;";
        }

        public void CreateDatabase()
        {
            if (!File.Exists(_databaseFile))
            {
                SQLiteConnection.CreateFile(_databaseFile);
            }

            using (SQLiteConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                CreatePhotosTable(conn);
            }
        }

        private void CreatePhotosTable(SQLiteConnection dbConn)
        {
            var tableExistsQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='photos';";
            using (SQLiteCommand cmd = new SQLiteCommand(tableExistsQuery, dbConn))
            {
                string result = (string)cmd.ExecuteScalar();

                if (string.IsNullOrEmpty(result))
                {
                    string createTableQuery = "CREATE TABLE photos (id INTEGER PRIMARY KEY AUTOINCREMENT, userid TEXT, photo TEXT, description TEXT, tags TEXT)";
                    using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, dbConn))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void CreatePicture(string userid, string img64, string desc, string tags)
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();

                string sql = $"insert into photos (userid, photo, description, tags) values ('{userid}', '{img64}', '{desc}', '{tags}')";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
            
            }
        }

        public List<string> GetPictures(string userid)
        {
            List<string> list = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                string sql = $"select * from photos where userid='{userid}'";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dynamic dd = new DynamicDictionary();
                            dd.photo = reader["photo"];
                            list.Add(dd);
                     
                        }
                    }
                }
            }
            return list;
        }

        public List<string> GetPictures()
        {
            List<string> list = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                string sql = $"select * from photos";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dynamic dd = new DynamicDictionary();
                            dd.photo = reader["photo"];
                            list.Add(dd.photo);
                        }
                    }
                }
            }
            return list;
        }

        public List<object> GetPicturess()
        {
            List<object> list = new List<object>();
            using (SQLiteConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                string sql = $"select * from photos";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dynamic dd = new DynamicDictionary();
                            dd.Id = reader["id"];
                            dd.UserId = reader["userid"];
                            dd.Photo = reader["photo"];
                            dd.Description = reader["description"];
                            dd.Tags = reader["tags"];

                            var model = new {
                                Id = reader["id"],
                                UserId = reader["userid"],
                                Photo = reader["photo"],
                                Description = reader["description"],
                                Tags = reader["tags"]
                            };

                            list.Add(model);
                           
                        }
                    }
                }
            }
            return list;
        }

        private void CreateUsersTable(SQLiteConnection dbConn)
        {
            var tableExistsQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='users';";
            using (SQLiteCommand cmd = new SQLiteCommand(tableExistsQuery, dbConn))
            {
                string result = (string)cmd.ExecuteScalar();

                if (string.IsNullOrEmpty(result))
                {
                    string createTableQuery = "CREATE TABLE users (id INTEGER PRIMARY KEY AUTOINCREMENT, userid TEXT, issuedat INTEGER, expires INTEGER)";
                    using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, dbConn))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void CreateUser(string userid, long issuedat, long expires)
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();

                string sql = $"insert into users (userid, issuedat, expires) values ('{userid}', '{issuedat}', '{expires}')";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
             
            }
        }
    }
}
