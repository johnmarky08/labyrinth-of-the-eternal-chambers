
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
        public static readonly List<(int, int)> offsets =
            [
                (-7, 0),   // 1 block above
                (7, 0),    // 1 block below
                (0, -7),   // 1 block to the left
                (0, 7),    // 1 block to the right
            ];

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
                    // Randomly decide to place a boundary or leave space (chance of 50% for boundary)
                    bool placeBoundary = random.Next(0, 100) < 50;

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

        private static void RenderPlayer(int renderY, int renderX, string token)
        {
            string[] playerLines = token.Split('\n');

            // Render player in new position.
            for (int i = 0; i < playerLines.Length; i++)
            {
                int playerY = renderY + i;
                if (playerY < map.GetLength(0))
                {
                    for (int j = 0; j < playerLines[i].Length; j++)
                    {
                        int playerX = renderX + j;
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

            RenderPlayer(Game.playerY, Game.playerX, Token.player);

            if (Check(Game.oldPlayerY, Game.oldPlayerX, Token.player) && (!(Game.oldPlayerX == Game.playerX && Game.oldPlayerY == Game.playerY)))
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

            // Draw tokens.
            int adjustedY = y / blockSize * blockSize;
            int adjustedX = x / blockSize * blockSize;
            if (Check(adjustedY, adjustedX, Token.boundary)) DrawToken(y, x, ConsoleColor.Gray, Token.boundary, null);
            else if (Check(adjustedY, adjustedX, Token.player)) DrawToken(y, x, ConsoleColor.Green, Token.player, null);
            else DrawToken(y, x, ConsoleColor.White, null, null);
        }

        // Token drawer (ASCII Art Supported)
        private static void DrawToken(int y, int x, ConsoleColor color, string? chosenToken, bool? isScore)
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
            else if (isScore == true) // Define score as true, since its a nullable one
            {
                // Split the string into chunks of 8 characters
                List<string> chunks = Enumerable.Range(0, (int)Math.Ceiling(chosenToken.Length / 8.0))
                               .Select(i => chosenToken.Substring(i * 8, Math.Min(8, chosenToken.Length - i * 8)))
                               .ToList();

                // Group chunks into sub-arrays of up to 6 chunks each
                List<string[]> groupedChunks = chunks
                                    .Select((chunk, index) => new { chunk, index })
                                    .GroupBy(x => x.index / 6)
                                    .Select(g => g.Select(x => x.chunk).ToArray())
                                    .ToList();
                int temporaryY = y;
                foreach (string[] groupChunk in groupedChunks)
                {
                    foreach (string chunk in groupChunk)
                    {
                        Console.SetCursorPosition(x, temporaryY++);
                        Console.Write(chunk);
                    }
                    x += 8;
                    temporaryY = y;
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
            RenderPlayer(Game.playerY, Game.playerX, Token.player);
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
            const string livesToken = "  ____                             _____       _                    _      \r\n |  _ \\ ___   ___  _ __ ___  ___  | ____|_ __ | |_ ___ _ __ ___  __| |  _  \r\n | |_) / _ \\ / _ \\| '_ ` _ \\/ __| |  _| | '_ \\| __/ _ | '__/ _ \\/ _` | (_) \r\n |  _ | (_) | (_) | | | | | \\__ \\ | |___| | | | ||  __| | |  __| (_| |  _  \r\n |_| \\_\\___/ \\___/|_| |_| |_|___/ |_____|_| |_|\\__\\___|_|  \\___|\\__,_| (_) \r\n                                                                           ";
            DrawToken(height + 1, 0, ConsoleColor.Yellow, livesToken, null);
            DrawToken(height + 1, 75, ConsoleColor.Yellow, NumberToken(Game.roomEntered), true);
        }

        // Number to ASCII Art Number.
        private static string NumberToken(int number)
        {
            string numberToken = Convert.ToString(number);

            numberToken = numberToken.Replace("0", "   ___    / _ \\  | | | | | |_| |  \\___/         ");
            numberToken = numberToken.Replace("1", "   __     /  |    |  |    |  |    |__|          ");
            numberToken = numberToken.Replace("2", "  ____   |___ \\    __) |  / __/  |_____|        ");
            numberToken = numberToken.Replace("3", "  _____  |___ /    |_ \\   ___) | |____/         ");
            numberToken = numberToken.Replace("4", " _  _   | || |  | || |_ |__   _|   |_|          ");
            numberToken = numberToken.Replace("5", "  ____   | ___|  |___ \\   ___) | |____/         ");
            numberToken = numberToken.Replace("6", "   __     / /_   | '_ \\  | (_) |  \\___/         ");
            numberToken = numberToken.Replace("7", "  _____  |___  |    / /    / /    /_/           ");
            numberToken = numberToken.Replace("8", "   ___    ( _ )   / _ \\  | (_) |  \\___/         ");
            numberToken = numberToken.Replace("9", "   ___    / _ \\  | (_) |  \\__, |    /_/         ");

            return numberToken;
        }
    }
}