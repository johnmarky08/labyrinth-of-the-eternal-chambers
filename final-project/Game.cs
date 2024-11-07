namespace final_project
{
    internal class Game
    {
        public static int playerX = 1, playerY = 1;
        static bool gameOver = false;

        internal static void Execute()
        {
            Console.CursorVisible = false;
            Map.InitializeMap();
            Map.GenerateBoundaries();
            Map.DrawMap();
            StartGameLoop();
        }

        static void StartGameLoop()
        {
            while (Bomb.playerLives > 0 && !gameOver)
            {
                Bomb.Update();

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        gameOver = true;
                        return;
                    }

                    MovePlayer(key);
                    Map.DrawMap();
                }

                Thread.Sleep(50);  // Optional: Slight delay to prevent high CPU usage
            }

            // Game Over
            Console.Clear();
            Console.SetCursorPosition(0, 10);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t\t      ___           ___           ___           ___                    ___           ___           ___           ___     \r\n\t\t     /\\  \\         /\\  \\         /\\__\\         /\\  \\                  /\\  \\         /\\__\\         /\\  \\         /\\  \\    \r\n\t\t    /::\\  \\       /::\\  \\       /::|  |       /::\\  \\                /::\\  \\       /:/  /        /::\\  \\       /::\\  \\   \r\n\t\t   /:/\\:\\  \\     /:/\\:\\  \\     /:|:|  |      /:/\\:\\  \\              /:/\\:\\  \\     /:/  /        /:/\\:\\  \\     /:/\\:\\  \\  \r\n\t\t  /:/  \\:\\  \\   /::\\~\\:\\  \\   /:/|:|__|__   /::\\~\\:\\  \\            /:/  \\:\\  \\   /:/__/  ___   /::\\~\\:\\  \\   /::\\~\\:\\  \\ \r\n\t\t /:/__/_\\:\\__\\ /:/\\:\\ \\:\\__\\ /:/ |::::\\__\\ /:/\\:\\ \\:\\__\\          /:/__/ \\:\\__\\  |:|  | /\\__\\ /:/\\:\\ \\:\\__\\ /:/\\:\\ \\:\\__\\\r\n\t\t \\:\\  /\\ \\/__/ \\/__\\:\\/:/  / \\/__/~~/:/  / \\:\\~\\:\\ \\/__/          \\:\\  \\ /:/  /  |:|  |/:/  / \\:\\~\\:\\ \\/__/ \\/_|::\\/:/  /\r\n\t\t  \\:\\ \\:\\__\\        \\::/  /        /:/  /   \\:\\ \\:\\__\\             \\:\\  /:/  /   |:|__/:/  /   \\:\\ \\:\\__\\      |:|::/  / \r\n\t\t   \\:\\/:/  /        /:/  /        /:/  /     \\:\\ \\/__/              \\:\\/:/  /     \\::::/__/     \\:\\ \\/__/      |:|\\/__/  \r\n\t\t    \\::/  /        /:/  /        /:/  /       \\:\\__\\                 \\::/  /       ~~~~          \\:\\__\\        |:|  |    \r\n\t\t     \\/__/         \\/__/         \\/__/         \\/__/                  \\/__/                       \\/__/         \\|__|    \r\n\t\t");
            Console.WriteLine("\n\n\n\n\n\n\n\n\t\t    ____                                                    __                   __                        _  __        \r\n\t\t   / __ \\ _____ ___   _____ _____   ____ _ ____   __  __   / /__ ___   __  __   / /_ ____     ___   _  __ (_)/ /_       \r\n\t\t  / /_/ // ___// _ \\ / ___// ___/  / __ `// __ \\ / / / /  / //_// _ \\ / / / /  / __// __ \\   / _ \\ | |/_// // __/       \r\n\t\t / ____// /   /  __/(__  )(__  )  / /_/ // / / // /_/ /  / ,<  /  __// /_/ /  / /_ / /_/ /  /  __/_>  < / // /_ _  _  _ \r\n\t\t/_/    /_/    \\___//____//____/   \\__,_//_/ /_/ \\__, /  /_/|_| \\___/ \\__, /   \\__/ \\____/   \\___//_/|_|/_/ \\__/(_)(_)(_)\r\n\t\t                                               /____/               /____/                                              \r\n\t\t");
            Console.ReadKey();
        }

        static void MovePlayer(ConsoleKey key)
        {
            int newX = playerX, newY = playerY;

            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    newY = Math.Max(1, playerY - 1);
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    newY = Math.Min(Map.height - 2, playerY + 1);
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    newX = Math.Max(1, playerX - 1);
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    newX = Math.Min(Map.width - 2, playerX + 1);
                    break;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    Bomb.Plant();
                    return;
            }

            // Check if the new position is not a wall
            if (Map.map[newY, newX] != Map.boundaryToken && Map.map[newY, newX] != (char)Configurations.WALL_LEFT_RIGHT && Map.map[newY, newX] != (char)Configurations.WALL_TOP_BOTTOM)
            {
                playerX = newX;
                playerY = newY;
            }
        }
    }
}
