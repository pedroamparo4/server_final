using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace MicroService.Helpers
{
    public class PhotoDbHelper
    {
        private readonly string _databaseFile;
        private readonly string _databaseFilePath = Directory.GetCurrentDirectory();
        private readonly string _connString;
        private object _mutex = new object();

        // "testphoto.sqlite"
        public PhotoDbHelper(string databaseFile)
        {
            _databaseFile = _databaseFilePath + @"\..\..\..\" + databaseFile;
            _connString = $"Data Source={databaseFile};Version=3;";
        }

        public object GetImageByPhotoId(string photoId)
        {
            object o = new object();

            lock(_mutex)
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connString))
                {
                    conn.Open();
                    string sql = $"select * from photos where photoid='{photoId}'";
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                o = reader["photo"];
                                Console.WriteLine("64 from db");
                            }
                        }
                    }
                }
                return o;
            }
        }
            

        public void insertTags(string photoId, string desc, string tags)
        {
            lock (_mutex)
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connString))
                {
                    conn.Open();

                    string sql = $"insert into photos (description, tags) values ('{desc}', '{tags}') where photoid='{photoId}'";
                    SQLiteCommand command = new SQLiteCommand(sql, conn);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Done inserting tags");
                }
            }
        }



    }
}
