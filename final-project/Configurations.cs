namespace final_project
{
    internal enum Configurations
    {
        WIDTH = 273, // Map width (Divisible by 7)
        HEIGHT = 77, // Map height (Divisible by 7)
        PATTERN_LENGTH = 3
    }

    // ASCII Art Tokens
    internal class Token
    {
        public static readonly string whitespace = "       \n       \n       \n       \n       \n       \n       ";
        public static readonly string topBottomWall = "       \n       \n _____ \n|_____|\n       \n       \n       ";
        public static readonly string leftRightWall = "  ___  \n |   | \n |   | \n |   | \n |   | \n |___| \n       ";
        public static readonly string boundary = " _____ \n|     |\n|     |\n|     |\n|     |\n|_____|\n       ";
        public static readonly string player = "  ___  \n /. .\\ \n|  `  |\n \\_-_/ \n /| |\\ \n  |_|  \n  / \\  ";
        public static readonly string door1 = "  ___  \n /   \\ \n|  *  |\n|  1  |\n|     |\n \\___/ \n       ";
        public static readonly string door2 = "  ___  \n /   \\ \n|  *  |\n|  2  |\n|  *  |\n \\___/ \n       ";
        public static readonly string door3 = "  ___  \n /   \\ \n|  *  |\n|* 3 *|\n|     |\n \\___/ \n       ";
        public static readonly string door4 = "  ___  \n /   \\ \n|  *  |\n|* 4 *|\n|  *  |\n \\___/ \n       ";
    }
}