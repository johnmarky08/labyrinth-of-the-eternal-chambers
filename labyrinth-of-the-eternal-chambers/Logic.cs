using System.Diagnostics.Metrics;

namespace labyrinth_of_the_eternal_chambers
{
    internal class Logic
    {
        public static string currentPattern = "";
        public static string pattern = "";
        public static int timeInSeconds = 0;
        private static System.Timers.Timer? timer;

        /// <summary>
        /// Checks if the pattern the user is taking is correct.
        /// </summary>
        /// <param name="nextRoom">The next room that the player is taking.</param>
        /// <returns>Boolean that represents whether it is correct or not.</returns>
        public static bool CheckPattern(int nextRoom)
        {
            if (nextRoom == GetNextNumber()) return true;

            return false;
        }

        /// <summary>
        /// Get the next number in the pattern.
        /// </summary>
        /// <returns>The next room number.</returns>
        private static int GetNextNumber()
        {
            if (currentPattern == null)
                return pattern[0];
            // Check if the input matches the start of the pattern
            else if (pattern.StartsWith(currentPattern))
            {
                // Return the next character in the pattern as an integer
                return int.Parse(pattern[currentPattern.Length].ToString());
            }
            // Return 0 if the input does not match the start of the pattern
            return 0;
        }

        /// <summary>
        /// Generates a random pattern that the user needs to guess to win.
        /// </summary>
        public static void GeneratePattern()
        {
            Random random = new();

            for (int i = 0; i < (int)Configurations.PATTERN_LENGTH; i++)
            {
                pattern += random.Next(1, 5);
            }
        }

        /// <summary>
        /// Start the timer of the game.
        /// </summary>
        public static void StartTime()
        {
            timer = new(1000);
            timer.Elapsed += (sender, e) =>
            {
                if (!(Menu.isGuideOpen || Menu.isExitMenuOpen))
                {
                    timeInSeconds++;
                }
            };
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        /// <summary>
        /// Stop the timer of the game.
        /// </summary>
        public static void StopTime()
        {
            timer?.Stop();
            timer?.Dispose();
        }

        /// <summary>
        /// Get the time in minutes and seconds format.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <returns>The time taken by the user to finish the game in mm:ss format.</returns>
        public static string GetTime(int time)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            return $"{minutes:D2}:{seconds:D2}s";
        }
    }
}
