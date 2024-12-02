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
                "CREATE TABLE IF NOT EXISTS Players (Id INTEGER PRIMARY KEY, Name TEXT UNIQUE, HighScore INTEGER);",
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
        /// <param name="highScore">Initialize a high score for them.</param>
        public static void InsertPlayer(string name, int highScore)
        {
            if (GetPlayerHighScore(name) >= 0) return;

            using SQLiteConnection connection = new(connectionString);
            connection.Open();

            SQLiteCommand insertCommand = new(
                "INSERT OR IGNORE INTO Players (Name, HighScore) VALUES (@name, @highScore);",
                connection
            );
            insertCommand.Parameters.AddWithValue("@name", name);
            insertCommand.Parameters.AddWithValue("@highScore", highScore);
            insertCommand.ExecuteNonQuery();
            
            connection.Close();
            SaveData();
        }

        /// <summary>
        /// Overwrites the high score of the selected player if it is higher than before.
        /// </summary>
        /// <param name="name">The player name.</param>
        /// <param name="newHighScore">New high score of the player.</param>
        public static void UpdateHighScore(string name, int newHighScore)
        {
            using SQLiteConnection connection = new(connectionString);
            connection.Open();

            SQLiteCommand updateCommand = new(
                "UPDATE Players SET HighScore = @newHighScore WHERE Name = @name AND HighScore < @newHighScore;",
                connection
            );
            updateCommand.Parameters.AddWithValue("@name", name);
            updateCommand.Parameters.AddWithValue("@newHighScore", newHighScore);
            int rowsAffected = updateCommand.ExecuteNonQuery();

            connection.Close();
            SaveData();
        }

        /// <summary>
        /// Reads the current high score of the selected player.
        /// </summary>
        /// <param name="name">The player name.</param>
        /// <returns>The current high score of the player.</returns>
        public static int GetPlayerHighScore(string name)
        {
            using SQLiteConnection connection = new(connectionString);
            connection.Open();

            SQLiteCommand selectCommand = new("SELECT HighScore FROM Players WHERE Name = @name;", connection);
            selectCommand.Parameters.AddWithValue("@name", name);

            object result = selectCommand.ExecuteScalar();

            connection.Close();

            // If player not found, return -1
            if (result == null || result == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(result);
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
    }
}
