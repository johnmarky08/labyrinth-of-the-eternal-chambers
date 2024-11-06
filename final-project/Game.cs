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
            Console.SetCursorPosition(0, 2);
            Console.WriteLine("░██████╗░░█████╗░███╗░░░███╗███████╗  ░█████╗░██╗░░░██╗███████╗██████╗░  ██╗" +
                            "\n██╔════╝░██╔══██╗████╗░████║██╔════╝  ██╔══██╗██║░░░██║██╔════╝██╔══██╗  ██║" +
                            "\n██║░░██╗░███████║██╔████╔██║█████╗░░  ██║░░██║╚██╗░██╔╝█████╗░░██████╔╝  ██║" +
                            "\n██║░░╚██╗██╔══██║██║╚██╔╝██║██╔══╝░░  ██║░░██║░╚████╔╝░██╔══╝░░██╔══██╗  ╚═╝" +
                            "\n╚██████╔╝██║░░██║██║░╚═╝░██║███████╗  ╚█████╔╝░░╚██╔╝░░███████╗██║░░██║  ██╗" +
                            "\n░╚═════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚══════╝  ░╚════╝░░░░╚═╝░░░╚══════╝╚═╝░░╚═╝  ╚═╝");
            Console.WriteLine("\n\nPress any key to exit...");
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
