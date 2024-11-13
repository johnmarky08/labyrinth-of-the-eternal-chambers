namespace final_project
{
    internal class Map
    {
        public static readonly int blockSize = 7;
        public static readonly int width = (int)Configurations.WIDTH;
        public static readonly int height = (int)Configurations.HEIGHT;
        public static readonly int blockWidth = width / blockSize;
        public static readonly int blockHeight = height / blockSize;
        public static readonly char[,] map = new char[height, width];
        public static readonly Random random = new();

        // Check if map[y, x] is the same with the given representationToken.
        public static bool Check(int startY, int startX, string representationToken)
        {
            string[] expectedRows = representationToken.Split('\n');

            for (int i = 0; i < expectedRows.Length; i++)
            {
                for (int j = 0; j < expectedRows[i].Length; j++)
                {
                    if ((startY + i) < map.GetLength(0) ||
                        (startX + j) < map.GetLength(1) ||
                        i < expectedRows.Length ||
                        j < expectedRows[i].Length)
                    {
                        if (map[startY + i, startX + j] != expectedRows[i][j])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        // Generate Walls.
        public static void InitializeMap()
        {
            string[] topBottomWallLines = Token.topBottomWall.Split('\n');
            string[] leftRightWallLines = Token.leftRightWall.Split('\n');

            for (int blockY = 0; blockY < blockHeight; blockY++)
            {
                for (int blockX = 0; blockX < blockWidth; blockX++)
                {
                    for (int localY = 0; localY < blockSize; localY++)
                    {
                        for (int localX = 0; localX < blockSize; localX++)
                        {
                            int mapY = blockY * blockSize + localY;
                            int mapX = blockX * blockSize + localX;

                            if (blockX == 0 || blockX == (blockWidth - 1))
                            {
                                map[mapY, mapX] = leftRightWallLines[localY][localX];
                            }
                            else if (blockY == 0 || blockY == (blockHeight - 1))
                            {
                                map[mapY, mapX] = topBottomWallLines[localY][localX];
                            }
                        }
                    }
                }
            }
        }

        // Generate random removable walls inside the map.
        public static void GenerateBoundaries()
        {
            string[] boundaryLines = Token.boundary.Split('\n');

            for (int blockY = 1; blockY < blockHeight - 1; blockY++)
            {
                for (int blockX = 1; blockX < blockWidth - 1; blockX++)
                {
                    // Randomly decide to place a boundary or leave space (chance of 30% for boundary)
                    bool placeBoundary = random.Next(0, 100) < 30;

                    for (int y = 0; y < blockSize; y++)
                    {
                        for (int x = 0; x < blockSize; x++)
                        {
                            int mapY = blockY * blockSize + y;
                            int mapX = blockX * blockSize + x;

                            if (mapY < map.GetLength(0) && mapX < map.GetLength(1))
                            {
                                map[mapY, mapX] = placeBoundary ? boundaryLines[y][x] : ' ';
                            }
                        }
                    }
                }
            }

            // Start carving a random path from a random position
            CarvePath(random.Next(1, blockHeight - 1), random.Next(1, blockWidth - 1));
        }

        // Recursive method to carve the boundary with a safe recursion depth limit
        public static void CarvePath(int blockY, int blockX, int depth = 0)
        {
            int maxDepth = 1000; // Max depth to prevent stack overflow
            if (depth > maxDepth)
                return;

            // Directions to carve: up, down, left, right (moving 2 steps to carve 7x7 blocks)
            var directions = new (int, int)[] { (-2, 0), (2, 0), (0, -2), (0, 2) };

            // Shuffle directions to ensure randomness
            directions = [.. directions.OrderBy(d => random.Next())];

            foreach (var (dy, dx) in directions)
            {
                int newY = blockY + dy, newX = blockX + dx;

                if (newY > 0 && newY < map.GetLength(0) / blockSize - 1 && newX > 0 && newX < map.GetLength(1) / blockSize - 1)
                {
                    string[] boundaryLines = Token.boundary.Split('\n');
                    bool canCarve = true;
                    for (int y = 0; y < blockSize; y++)
                    {
                        for (int x = 0; x < blockSize; x++)
                        {
                            int mapY = (newY * blockSize) + y;
                            int mapX = (newX * blockSize) + x;
                            if (map[mapY, mapX] != boundaryLines[y][x])
                            {
                                canCarve = false;
                                break;
                            }
                        }
                        if (!canCarve) break;
                    }

                    if (canCarve)
                    {
                        for (int y = 0; y < blockSize; y++)
                        {
                            for (int x = 0; x < blockSize; x++)
                            {
                                int mapY = (newY * blockSize) + y;
                                int mapX = (newX * blockSize) + x;
                                map[mapY, mapX] = ' ';
                            }
                        }

                        map[(blockY * blockSize) + dy / 2, (blockX * blockSize) + dx / 2] = ' ';

                        CarvePath(newY, newX, depth + 1);
                    }
                }
            }
        }

        private static void RenderPlayer()
        {
            string[] playerLines = Token.player.Split('\n');

            // Render player in new position.
            for (int i = 0; i < playerLines.Length; i++)
            {
                int playerY = Game.playerY + i;
                if (playerY < map.GetLength(0))
                {
                    for (int j = 0; j < playerLines[i].Length; j++)
                    {
                        int playerX = Game.playerX + j;
                        if (playerX < map.GetLength(1))
                        {
                            map[playerY, playerX] = playerLines[i][j];
                        }
                    }
                }
            }
        }

        public static void SpecificDraw(int y, int x)
        {
            string[] playerLines = Token.player.Split('\n');

            RenderPlayer();

            if (Check(Game.oldPlayerY, Game.oldPlayerX, Token.player))
            {
                // Remove old player on old posistion
                for (int i = 0; i < playerLines.Length; i++)
                {
                    int oldPlayerY = Game.oldPlayerY + i;
                    if (oldPlayerY < map.GetLength(0))
                    {
                        for (int j = 0; j < playerLines[i].Length; j++)
                        {
                            int oldPlayerX = Game.oldPlayerX + j;
                            if (oldPlayerX < map.GetLength(1))
                            {
                                map[oldPlayerY, oldPlayerX] = ' ';
                                Console.SetCursorPosition(oldPlayerX, oldPlayerY);
                                Console.Write(map[oldPlayerY, oldPlayerX]);
                            }
                        }
                    }
                }
            }

            // Place bombs in the map array
            foreach (var bomb in Bomb.bombs)
            {
                var bombLines = Token.bomb.Split('\n');
                for (int i = 0; i < bombLines.Length; i++)
                {
                    int bombY = bomb.Y + i;
                    if (bombY < map.GetLength(0))
                    {
                        for (int j = 0; j < bombLines[i].Length; j++)
                        {
                            int bombX = bomb.X + j;
                            if (bombX < map.GetLength(1))
                            {
                                map[bombY, bombX] = bombLines[i][j];
                            }
                        }
                    }
                }
            }

            // Draw tokens.
            int adjustedY = y / blockSize * blockSize;
            int adjustedX = x / blockSize * blockSize;
            if (Check(adjustedY, adjustedX, Token.boundary)) DrawToken(y, x, ConsoleColor.Gray, Token.boundary);
            else if (Check(adjustedY, adjustedX, Token.player) && Check(adjustedY, adjustedX, Token.bomb)) DrawToken(y, x, ConsoleColor.Red, Token.player);
            else if (Check(adjustedY, adjustedX, Token.player)) DrawToken(y, x, ConsoleColor.Green, Token.player);
            else if (Check(adjustedY, adjustedX, Token.bomb)) DrawToken(y, x, ConsoleColor.Red, Token.bomb);
            else DrawToken(y, x, ConsoleColor.White, null);
        }

        // Token drawer (ASCII Art Supported)
        private static void DrawToken(int y, int x, ConsoleColor color, string? chosenToken)
        {
            Console.ForegroundColor = color;

            if (chosenToken == null)
            {
                for (int i = 0; i < blockSize; i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    Console.Write(' ');
                }
                return;
            }

            string[] token = chosenToken.Split('\n');
            for (int i = 0; i < token.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(token[i]);
            }
        }

        // Generate initial Map on console.
        public static void DrawMap()
        {
            RenderPlayer();
            Console.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.SetCursorPosition(x, y);

                    // Adjust coordinates to match the 7x7 grid
                    int adjustedY = y / blockSize * blockSize;
                    int adjustedX = x / blockSize * blockSize;

                    if (Check(adjustedY, adjustedX, Token.boundary))
                    {
                        Console.ForegroundColor = ConsoleColor.Gray; // Boundary color
                        Console.Write(map[y, x]);
                    }
                    else if (Check(adjustedY, adjustedX, Token.player))
                    {
                        Console.ForegroundColor = ConsoleColor.Green; // Player color
                        Console.Write(map[y, x]);
                    }
                    else if (Check(adjustedY, adjustedX, Token.bomb))
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Bomb color
                        Console.Write(map[y, x]);
                    }
                    else
                    {
                        Console.ResetColor(); // Default color for other spaces
                        Console.Write(map[y, x]);
                    }
                }
            }
        }

        // Display current player status.
        public static void DrawScore()
        {
            const string livesToken = "  _      _                    ____                          _         _                   \r\n | |    (_)__   __ ___  ___  |  _ \\  ___  _ __ ___    __ _ (_) _ __  (_) _ __    __ _  _  \r\n | |    | |\\ \\ / // _ \\/ __| | |_) |/ _ \\| '_ ` _ \\  / _` || || '_ \\ | || '_ \\  / _` |(_) \r\n | |___ | | \\ V /|  __/\\__ \\ |  _ <|  __/| | | | | || (_| || || | | || || | | || (_| | _  \r\n |_____||_|  \\_/  \\___||___/ |_| \\_\\\\___||_| |_| |_| \\__,_||_||_| |_||_||_| |_| \\__, |(_) \r\n                                                                                |___/     ";
            const string bombsToken = "  ____                     _            ____                          _         _                   \r\n | __ )   ___   _ __ ___  | |__   ___  |  _ \\  ___  _ __ ___    __ _ (_) _ __  (_) _ __    __ _  _  \r\n |  _ \\  / _ \\ | '_ ` _ \\ | '_ \\ / __| | |_) |/ _ \\| '_ ` _ \\  / _` || || '_ \\ | || '_ \\  / _` |(_) \r\n | |_) || (_) || | | | | || |_) |\\__ \\ |  _ <|  __/| | | | | || (_| || || | | || || | | || (_| | _  \r\n |____/  \\___/ |_| |_| |_||_.__/ |___/ |_| \\_\\\\___||_| |_| |_| \\__,_||_||_| |_||_||_| |_| \\__, |(_) \r\n                                                                                          |___/     ";
            const string guideToken = "   ____        __ _  _  _                                                    _                  __  _                                   _              _              __  \r\n  / /\\ \\      / /(_)| || |  _ __  ___   __ _   ___  _ __    ___  _ __  __ _ | |_  ___    __ _  / _|| |_  ___  _ __    ___ __  __ _ __  | |  ___   ___ (_)  ___   _ __ \\ \\ \r\n | |  \\ \\ /\\ / / | || || | | '__|/ _ \\ / _` | / _ \\| '_ \\  / _ \\| '__|/ _` || __|/ _ \\  / _` || |_ | __|/ _ \\| '__|  / _ \\\\ \\/ /| '_ \\ | | / _ \\ / __|| | / _ \\ | '_ \\ | |\r\n | |   \\ V  V /  | || || | | |  |  __/| (_| ||  __/| | | ||  __/| |  | (_| || |_|  __/ | (_| ||  _|| |_|  __/| |    |  __/ >  < | |_) || || (_) |\\__ \\| || (_) || | | || |\r\n | |    \\_/\\_/   |_||_||_| |_|   \\___| \\__, | \\___||_| |_| \\___||_|   \\__,_| \\__|\\___|  \\__,_||_|   \\__|\\___||_|     \\___|/_/\\_\\| .__/ |_| \\___/ |___/|_| \\___/ |_| |_|| |\r\n  \\_\\                                  |___/                                                                                    |_|                                   /_/ ";
            DrawToken(height + 1, 0, ConsoleColor.Yellow, livesToken);
            DrawToken(height + 1, 90, Bomb.playerLives > 1 ? ConsoleColor.Yellow : ConsoleColor.Red, NumberToken(Bomb.playerLives));
            DrawToken(height + 7, 0, ConsoleColor.Yellow, bombsToken);
            DrawToken(height + 7, 100,(Bomb.maxBombs - Bomb.currentBombs) < 1 ? ConsoleColor.Red : ConsoleColor.Yellow, NumberToken(Bomb.maxBombs - Bomb.currentBombs));
            DrawToken(height + 7, 109,ConsoleColor.Yellow, guideToken);
        }

        // Number to ASCII Art Number.
        private static string NumberToken(int number)
        {
            return number switch
            {
                0 => "   ___  \r\n  / _ \\ \r\n | | | |\r\n | |_| |\r\n  \\___/ \r\n        ",
                1 => "  _     \r\n / |    \r\n | |    \r\n | |    \r\n |_|    \r\n        ",
                2 => "  ____  \r\n |___ \\ \r\n   __) |\r\n  / __/ \r\n |_____|\r\n        ",
                3 => "  _____ \r\n |___ / \r\n   |_ \\ \r\n  ___) |\r\n |____/ \r\n        ",
                _ => "",
            };
        }
    }
}