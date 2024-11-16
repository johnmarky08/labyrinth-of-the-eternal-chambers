namespace final_project
{
    internal class Logic
    {
        public static string currentPattern = "";
        public static string pattern = "";
        public static bool CheckPattern(int nextRoom)
        {
            if (nextRoom == GetNextNumber(currentPattern)) return true;

            return false;
        }

        private static int GetNextNumber(string input)
        {
            if (input == null)
                return pattern[0];
            // Check if the input matches the start of the pattern
            else if (pattern.StartsWith(input))
            {
                // Return the next character in the pattern as an integer
                return int.Parse(pattern[input.Length].ToString());
            }
            // Return 0 if the input does not match the start of the pattern
            return 0;
        }
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
