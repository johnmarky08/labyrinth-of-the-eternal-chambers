namespace labyrinth_of_the_eternal_chambers
{
    internal class Logic
    {
        public static string currentPattern = "";
        public static string pattern = "";

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
    }
}
