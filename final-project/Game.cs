namespace final_project
{
    internal class Game
    {
        static readonly int boardWidth = (int)Configurations.WIDTH;
        static readonly int boardHeight = (int)Configurations.HEIGHT;
        static readonly char[,] board = new char[boardHeight, boardWidth];
        static readonly char playerToken = (char)Configurations.PLAYER_TOKEN;
        static readonly char bombToken = (char)Configurations.BOMB_TOKEN;
        static int playerX = 1, playerY = 1;
        static int playerLives = 3;
        static readonly List<Bomb> bombs = [];  // Fixed initialization

        internal static void Execute()
        {
            Console.CursorVisible = false;
            InitializeBoard();
            DrawBoard();
            StartGameLoop();
        }

        static void InitializeBoard()
        {
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    if (y == 0 || y == boardHeight - 1)
                        board[y, x] = (char)Configurations.WALL_TOP_BOTTOM;
                    else if (x == 0 || x == boardWidth - 1)
                        board[y, x] = (char)Configurations.WALL_LEFT_RIGHT;
                    else
                        board[y, x] = ' ';
                }
            }
        }

        static void DrawBoard()
        {
            Console.Clear();
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(board[y, x]);
                }
            }

            // Draw player
            Console.SetCursorPosition(playerX, playerY);
            Console.Write(playerToken);

            // Draw bombs
            foreach (var bomb in bombs)
            {
                Console.SetCursorPosition(bomb.X, bomb.Y);
                Console.Write(bombToken);
            }

            // Display lives
            Console.SetCursorPosition(0, boardHeight + 1);
            Console.WriteLine($"Lives: {playerLives}");
        }

        static void StartGameLoop()
        {
            while (playerLives > 0)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    MovePlayer(key);
                }

                UpdateBombs();
                DrawBoard();
                Thread.Sleep(200);
            }

            // Game Over
            Console.SetCursorPosition(0, boardHeight + 2);
            Console.WriteLine("Game Over! Press any key to exit.");
            Console.ReadKey();
        }

        // Control the player and place the bomb
        static void MovePlayer(ConsoleKey key)
        {
            int newX = playerX, newY = playerY;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    newY = Math.Max(1, playerY - 1);
                    break;
                case ConsoleKey.DownArrow:
                    newY = Math.Min(boardHeight - 2, playerY + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    newX = Math.Max(1, playerX - 1);
                    break;
                case ConsoleKey.RightArrow:
                    newX = Math.Min(boardWidth - 2, playerX + 1);
                    break;
                case ConsoleKey.Spacebar:
                    PlaceBomb();
                    return;
            }

            // Check if the new position is not a wall
            if (!new char[] { (char)Configurations.WALL_LEFT_RIGHT, (char)Configurations.WALL_TOP_BOTTOM }.Contains(board[newY, newX]))
            {
                playerX = newX;
                playerY = newY;
            }
        }

        // Places a bomb at the player's current position
        static void PlaceBomb()
        {
            bombs.Add(new Bomb(playerX, playerY));
        }

        // Update bombs
        static void UpdateBombs()
        {
            List<Bomb> explodedBombs = [];

            foreach (var bomb in bombs)
            {
                if (bomb.IsTimerUp())
                {
                    ExplodeBomb(bomb);
                    explodedBombs.Add(bomb);
                }
            }

            // Remove exploded bombs
            foreach (var bomb in explodedBombs)
            {
                bombs.Remove(bomb);
            }
        }

        static void ExplodeBomb(Bomb bomb)
        {
            // Create the cross pattern for the explosion
            int centerX = bomb.X;
            int centerY = bomb.Y;
            bool playerCaughtInExplosion = false;

            // Vertical and horizontal explosion lines
            for (int y = centerY - 2; y <= centerY + 2; y++)
            {
                for (int x = centerX - 1; x <= centerX + 1; x++)
                {
                    if (x >= 0 && y >= 0 && x < boardWidth && y < boardHeight)
                    {
                        // Check if the explosion should be blocked by walls
                        if (!new char[] { (char)Configurations.WALL_LEFT_RIGHT, (char)Configurations.WALL_TOP_BOTTOM }.Contains(board[y, x]))
                        {
                            if ((x == centerX && y == centerY) ||
                                (x == centerX && (y == centerY - 1 || y == centerY + 1)) ||
                                (y == centerY && (x == centerX - 1 || x == centerX + 1)))
                            {
                                Console.SetCursorPosition(x, y);
                                Console.Write((char)Configurations.EXPLOSION);

                                // Check if the player is caught in the explosion
                                if (x == playerX && y == playerY)
                                {
                                    playerCaughtInExplosion = true;
                                }
                            }
                        }
                    }
                }
            }

            Thread.Sleep(200);

            for (int y = centerY - 2; y <= centerY + 2; y++)
            {
                for (int x = centerX - 1; x <= centerX + 1; x++)
                {
                    if (x >= 0 && y >= 0 && x < boardWidth && y < boardHeight)
                    {
                        // Reset the explosion area to empty
                        if (!new char[] { (char)Configurations.WALL_LEFT_RIGHT, (char)Configurations.WALL_TOP_BOTTOM }.Contains(board[y, x]))
                        {
                            board[y, x] = ' ';
                        }
                    }
                }
            }

            // If the player is caught in the explosion, reduce lives
            if (playerCaughtInExplosion)
            {
                playerLives--;
                Console.SetCursorPosition(0, boardHeight + 1);
                Console.WriteLine($"Lives: {playerLives}");
            }
        }


        // Bomb class to handle bomb logic (timer and position)
        class Bomb(int x, int y)
        {
            public int X { get; } = x;
            public int Y { get; } = y;
            private readonly DateTime placedTime = DateTime.Now;
            private static readonly TimeSpan ExplosionDelay = TimeSpan.FromSeconds(3); // Bomb delay

            public bool IsTimerUp()
            {
                return DateTime.Now - placedTime > ExplosionDelay;
            }
        }
    }
}
