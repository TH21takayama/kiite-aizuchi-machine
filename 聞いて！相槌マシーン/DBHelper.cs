using System;
using System.Data.SQLite;

public static class DBHelper
{
    private static string dbPath = "Data Source=app.db";

    /// <summary>
    /// DB初期化（テーブル作成）
    /// </summary>
    public static void Initialize()
    {
        using (var conn = new SQLiteConnection(dbPath))
        {
            conn.Open();
            string createUserTable = @"CREATE TABLE IF NOT EXISTS Users (
                                        Username TEXT PRIMARY KEY,
                                        Password TEXT,
                                        Voice TEXT,
                                        Tone TEXT,
                                        JimakuOn INTEGER DEFAULT 1,
                                        ImageOn INTEGER DEFAULT 1)";
            using (var cmd = new SQLiteCommand(createUserTable, conn))
            {
                cmd.ExecuteNonQuery();
            }

            string createChatLogsTable = @"CREATE TABLE IF NOT EXISTS ChatLogs (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Username TEXT,
                                            FileName TEXT,
                                            FilePath TEXT,
                                            SavedAt TEXT
                              )";
            using (var cmd = new SQLiteCommand(createChatLogsTable, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// ユーザー認証
    /// </summary>
    public static bool AuthenticateUser(string username, string password)
    {
        using (var conn = new SQLiteConnection(dbPath))
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Users WHERE Username=@u AND Password=@p";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }

    /// <summary>
    /// 新規ユーザー登録
    /// </summary>
    public static bool RegisterUser(string username, string password)
    {
        using (var conn = new SQLiteConnection(dbPath))
        {
            conn.Open();
            string query = "INSERT INTO Users (Username, Password, Voice, Tone, JimakuOn, ImageOn) VALUES (@u, @p, '', '', 1, 1)";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false; // ユーザー名重複など
                }
            }
        }
    }

    /// <summary>
    /// ユーザー設定を保存（声・スタイル・字幕・画像）
    /// </summary>
    public static void SaveUserSettings(string username, string voice, string tone, bool jimakuOn, bool imageOn)
    {
        using (var conn = new SQLiteConnection(dbPath))
        {
            conn.Open();
            string query = "UPDATE Users SET Voice=@v, Tone=@t, JimakuOn=@j, ImageOn=@i WHERE Username=@u";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@v", voice);
                cmd.Parameters.AddWithValue("@t", tone);
                cmd.Parameters.AddWithValue("@j", jimakuOn ? 1 : 0);
                cmd.Parameters.AddWithValue("@i", imageOn ? 1 : 0);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.ExecuteNonQuery();
            }
        }
    }
    /// <summary>
    /// 🔥 ユーザー設定を初期化（リセット）
    /// </summary>
    public static void ResetUserSettings(string username)
    {
        using (var conn = new SQLiteConnection(dbPath))
        {
            conn.Open();
            string query = @"UPDATE Users 
                             SET Voice='', 
                                 Tone='', 
                                 JimakuOn=1, 
                                 ImageOn=1
                             WHERE Username=@u";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// ユーザー設定を取得
    /// </summary>
    public static (string Voice, string Tone, bool JimakuOn, bool ImageOn) GetUserSettings(string username)
    {
        using (var conn = new SQLiteConnection(dbPath))
        {
            conn.Open();
            string query = "SELECT Voice, Tone, JimakuOn, ImageOn FROM Users WHERE Username=@u";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            reader["Voice"].ToString(),
                            reader["Tone"].ToString(),
                            Convert.ToInt32(reader["JimakuOn"]) == 1,
                            Convert.ToInt32(reader["ImageOn"]) == 1
                        );
                    }
                }
            }
        }
        return ("", "", true, true); // デフォルト値
    }

    //チャットの保存用メソッド
    public static void SaveChatLog(string username, string fileName, string filePath)
    {
        using (var conn = new SQLiteConnection(dbPath))
        {
            conn.Open();
            string query = @"INSERT INTO ChatLogs (Username, FileName, FilePath, SavedAt)
                         VALUES (@u, @f, @p, @s)";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@f", fileName);
                cmd.Parameters.AddWithValue("@p", filePath);
                cmd.Parameters.AddWithValue("@s", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
        }
    }
}