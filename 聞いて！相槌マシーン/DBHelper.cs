using System.Data.SQLite;

public static class DBHelper
{
    private static string dbPath = "Data Source=users.db";

    // DB初期化（ユーザー情報テーブル作成）
    public static void Initialize()
    {
        using (var con = new SQLiteConnection(dbPath))
        {
            con.Open();
            string sql = @"CREATE TABLE IF NOT EXISTS Users (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT UNIQUE,
                            Password TEXT
                           );";
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            cmd.ExecuteNonQuery();
        }
    }

    public static bool RegisterUser(string username, string password)
    {
        try
        {
            using (var con = new SQLiteConnection(dbPath))
            {
                con.Open();
                string sql = "INSERT INTO Users (Username, Password) VALUES (@u,@p)";
                SQLiteCommand cmd = new SQLiteCommand(sql, con);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password); // 簡易保存。後でハッシュ化も可能
                cmd.ExecuteNonQuery();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool AuthenticateUser(string username, string password)
    {
        using (var con = new SQLiteConnection(dbPath))
        {
            con.Open();
            string sql = "SELECT COUNT(*) FROM Users WHERE Username=@u AND Password=@p";
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);
            long count = (long)cmd.ExecuteScalar();
            return count > 0;
        }
    }
}
