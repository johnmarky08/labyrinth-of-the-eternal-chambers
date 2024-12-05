using System.Data.SQLite;

namespace labyrinth_of_the_eternal_chambers
{

    internal class Database
    {
        private static readonly string connectionString = @"Data Source=Database\database.sqlite;Version=3;";

        /// <summary>
        /// Creates the table along with the database file itself if they are still not created.
        /// </summary>
        public static void CreateTable()
        {
            using SQLiteConnection connection = new(connectionString);
            connection.Open();

            SQLiteCommand createTableCommand = new(
                @"CREATE TABLE IF NOT EXISTS Players (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT UNIQUE,
                    HighScore INTEGER,
                    Time INTEGER
                );",
                connection
            );
            createTableCommand.ExecuteNonQuery();
            connection.Close();
            SaveData();
        }

        /// <summary>
        /// Insert the player name along with its high score into the database; it will skip if the player name already exists.
        /// </summary>
        /// <param name="name">The player name.</param>
        public static void InsertPlayer(string name)
        {
            using SQLiteConnection connection = new(connectionString);
            connection.Open();

            SQLiteCommand insertCommand = new(
                @"INSERT OR IGNORE INTO Players (Name, HighScore, Time)
                  VALUES (@name, @highScore, @time);",
                connection
            );
            insertCommand.Parameters.AddWithValue("@name", name);
            insertCommand.Parameters.AddWithValue("@highScore", null);
            insertCommand.Parameters.AddWithValue("@time", null);
            insertCommand.ExecuteNonQuery();

            connection.Close();
            SaveData();
        }

        /// <summary>
        /// Overwrites the time of the selected player if it is faster than before.
        /// </summary>
        /// <param name="name">The player name.</param>
        /// <param name="newTime">New time of the player.</param>
        public static void UpdateTime(string name, int newTime)
        {
            using SQLiteConnection connection = new(connectionString);
            connection.Open();

            SQLiteCommand updateCommand = new(
                @"UPDATE Players
                  SET Time = @newTime
                  WHERE Name = @name AND (Time > @newTime OR Time IS NULL);",
                connection
            );
            updateCommand.Parameters.AddWithValue("@name", name);
            updateCommand.Parameters.AddWithValue("@newTime", newTime);
            updateCommand.ExecuteNonQuery();

            connection.Close();
            SaveData();
        }

        /// <summary>
        /// Overwrites the high score of the selected player if it is lower than before.
        /// </summary>
        /// <param name="name">The player name.</param>
        /// <param name="newHighScore">New high score of the player.</param>
        public static void UpdateHighScore(string name, int newHighScore)
        {
            using SQLiteConnection connection = new(connectionString);
            connection.Open();

            SQLiteCommand updateCommand = new(
                @"UPDATE Players
                  SET HighScore = @newHighScore
                  WHERE Name = @name AND (HighScore > @newHighScore OR HighScore IS NULL);",
                connection
            );
            updateCommand.Parameters.AddWithValue("@name", name);
            updateCommand.Parameters.AddWithValue("@newHighScore", newHighScore);
            updateCommand.ExecuteNonQuery();

            connection.Close();
            SaveData();
        }

        /// <summary>
        /// Get each player's data.
        /// </summary>
        /// <returns>The data in ascending order of wrong doors taken.</returns>
        public static Dictionary<string, (int? HighScore, int? Time)> GetAllPlayers()
        {
            var players = new Dictionary<string, (int? HighScore, int? Time)>();

            using SQLiteConnection connection = new(connectionString);
            connection.Open();

            SQLiteCommand selectCommand = new("SELECT Name, HighScore, Time FROM Players;", connection);
            using SQLiteDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                string name = reader["Name"].ToString() ?? "";
                int? highScore = reader["HighScore"] != DBNull.Value ? Convert.ToInt32(reader["HighScore"]) : null;
                int? time = reader["Time"] != DBNull.Value ? Convert.ToInt32(reader["Time"]) : null;

                players[name] = (highScore, time);
            }

            connection.Close();

            var sortedPlayers = players.OrderBy(player => player.Value.HighScore)
                                       .ThenBy(player => player.Value.Time)
                                       .ToDictionary(player => player.Key, player => player.Value);

            return sortedPlayers;
        }

        /// <summary>
        /// Saves the database with the new data entered.
        /// </summary>
        private static void SaveData()
        {
            string outputFolderDatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "database.sqlite");
            string? projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
            string targetDirectory = Path.Combine(projectDirectory ?? "", "Database");
            string targetFilePath = Path.Combine(targetDirectory, "database.sqlite");

            try
            {
                // Copy the database file to the target directory
                File.Copy(outputFolderDatabasePath, targetFilePath, overwrite: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying database file: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Delete all the table data.
        /// </summary>
        public static void DropTable()
        {
            using SQLiteConnection connection = new(connectionString);
            connection.Open();
             
            SQLiteCommand dropCommand = new("DROP TABLE IF EXISTS Players;", connection);
            dropCommand.ExecuteNonQuery();

            Console.WriteLine("Dropped Table Sucessfully. Please restart the program.");
            connection.Close();
            SaveData();
        }

    }
}
