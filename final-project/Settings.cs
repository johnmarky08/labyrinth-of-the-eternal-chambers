namespace final_project
{
    internal enum Configurations
    {
        WIDTH = 273, // Map width (Divisible by 7)
        HEIGHT = 70, // Map height (Divisible by 7)
        BOMB_TICKS = 5, // Bomb timeout before exploding
        PLAYER_LIVES = 3, // Max player lives
        MAX_BOMBS_ALLOWED = 3 // Max current bombs allowed in map.
    }

    // ASCII Art Tokens
    internal class Token()
    {
        public static readonly string topBottomWall = "       \n       \n _____ \n|_____|\n       \n       \n       ";
        public static readonly string leftRightWall = "  ___  \n |   | \n |   | \n |   | \n |   | \n |___| \n       ";
        public static readonly string boundary = " _____ \n|     |\n|     |\n|     |\n|     |\n|_____|\n       ";
        public static readonly string bomb = "       \n    __ \n  _/_/ \n / _ \\ \n| (_) |\n \\___/ \n       ";
        public static readonly string bombExplosion = "       \n   _   \n__/ \\__\n\\     /\n/_   _\\\n  \\_/  \n       ";
        public static readonly string player = "  ___  \n /. .\\ \n|  `  |\n \\_-_/ \n /| |\\ \n  |_|  \n  / \\  ";
    }
}