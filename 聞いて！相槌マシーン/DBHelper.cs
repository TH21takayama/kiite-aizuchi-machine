using System;
using System.Data.SQLite;

namespace 聞いて_相槌マシーン
{
    public static class DBHelper
    {
        private static string dbPath = "相槌DB.sqlite";
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        // 初期化（DBとテーブル作成）
        public static void Initialize()
        {
            if (!System.IO.File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT UNIQUE NOT NULL,
                        Password TEXT NOT NULL
                    );";
                using (var cmd = new SQLiteCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            // ✅ 設定テーブル作成
            InitializeSettingsTable();
        }

        // ✅ UserSettingsテーブル作成
        public static void InitializeSettingsTable()
        {
            using (var conn = new SQLiteConnection(connectionString))
            { 
                conn.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS UserSettings (
                        UserId INTEGER PRIMARY KEY,
                        LastVoice TEXT,
                        LastTone TEXT,
                        FOREIGN KEY(UserId) REFERENCES Users(Id)
                    );";
                using (var cmd = new SQLiteCommand(createTableQuery, conn))
                { 
                    cmd.ExecuteNonQuery(); 
                }
            }
        }

        // ユーザー認証
        public static bool AuthenticateUser(string username, string password)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username=@user AND Password=@pass";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // ユーザー登録
        public static bool RegisterUser(string username, string password)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (Username, Password) VALUES (@user, @pass)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (SQLiteException ex)
                    {
                        // ユーザー名重複など
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }
        }

        // ✅ 設定保存
        public static void SaveUserSettings(string username, string voice, string tone)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string getUserId = "SELECT Id FROM Users WHERE Username=@user";
                long userId;
                using (var cmd = new SQLiteCommand(getUserId, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    userId = (long)cmd.ExecuteScalar();
                }

                string query = @"
                    INSERT INTO UserSettings(UserId, LastVoice, LastTone)
                    VALUES(@id, @voice, @tone)
                    ON CONFLICT(UserId) DO UPDATE SET LastVoice=@voice, LastTone=@tone;";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.Parameters.AddWithValue("@voice", voice);
                    cmd.Parameters.AddWithValue("@tone", tone);
                    cmd.ExecuteNonQuery();
                }
            }
        }
         // ✅ 設定読み込み
        public static (string Voice, string Tone) LoadUserSettings(string username)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string getUserId = "SELECT Id FROM Users WHERE Username=@user";
                long userId;
                using (var cmd = new SQLiteCommand(getUserId, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    var result = cmd.ExecuteScalar();
                    if (result == null) return (null, null);
                    userId = (long)result;
                }

                string query = "SELECT LastVoice, LastTone FROM UserSettings WHERE UserId=@id";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string voice = reader["LastVoice"] as string;
                            string tone = reader["LastTone"] as string;
                            return (voice, tone);
                        }
                    }
                }
            }
            return (null, null);
        }
    }
}