
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
        private static readonly (int dx, int dy)[] directions =
        {
            (0, -7), // Up
            (7, 0),  // Right
            (0, 7),  // Down
            (-7, 0)  // Left
        };

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

        public static void RenderBoundaries()
        {
            string[] boundaryLines = Token.boundary.Split('\n');

            for (int blockY = 1; blockY < blockHeight - 1; blockY++)
            {
                for (int blockX = 1; blockX < blockWidth - 1; blockX++)
                {
                    for (int y = 0; y < blockSize; y++)
                    {
                        for (int x = 0; x < blockSize; x++)
                        {
                            int mapY = blockY * blockSize + y;
                            int mapX = blockX * blockSize + x;

                            if (mapY < map.GetLength(0) && mapX < map.GetLength(1))
                            {
                                map[mapY, mapX] = boundaryLines[y][x];
                            }
                        }
                    }
                }
            }
        }

        public static void GeneratePath(int currentX, int currentY, HashSet<(int, int)>? visited = null)
        {
            // Initialize the visited set if null
            visited ??= [];

            // Adjust coordinates to match the 7x7 grid
            int startY = currentY / blockSize * blockSize;
            int startX = currentX / blockSize * blockSize;

            // Ensure the current position is within bounds and not already visited
            if (visited.Contains((startX, startY)))
            {
                return;
            }

            // Mark current position as visited
            visited.Add((startX, startY));

            RenderToken(startY, startX, Token.whitespace);

            var random = new Random();
            var directionsList = new List<(int, int)>(directions);
            directionsList.Sort((a, b) => random.Next().CompareTo(random.Next()));

            foreach (var (dx, dy) in directionsList)
            {
                int newX = startX + dx * 2; // Step size of 2 to create pathways
                int newY = startY + dy * 2;
                int midX = startX + dx; // Midpoint between current and new position
                int midY = startY + dy;

                if (newX >= 0 && newY >= 0 && newX < width && newY < height && !visited.Contains((newX, newY)) &&
                    !(Check(newY, newX, Token.topBottomWall) || Check(newY, newX, Token.leftRightWall) || Check(newY, newX, Token.door)))
                {
                    // Render the path between the current and new cell
                    RenderToken(midY, midX, Token.whitespace);
                    RenderToken(newY, newX, Token.whitespace);

                    GeneratePath(newX, newY, visited); // Recursively generate the maze
                }
            }
        }

        // Ensure exits are accessible and place them at the four corners
        public static void PlaceExits()
        {
            // Place exits at the four corners
            RenderToken(0, 7, Token.door);
            RenderToken(0, width - 14, Token.door);
            RenderToken(height - 7, 7, Token.door);
            RenderToken(height - 7, width - 14, Token.door);
        }

        private static void RenderToken(int renderY, int renderX, string token)
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

            RenderToken(Game.playerY, Game.playerX, Token.player);

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
            RenderToken(Game.playerY, Game.playerX, Token.player);
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
                    else if (Check(adjustedY, adjustedX, Token.door))
                    {
                        Console.ForegroundColor = ConsoleColor.Green; // Teleportation color
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
            const string roomEntered = "  ____                             _____       _                    _      \r\n |  _ \\ ___   ___  _ __ ___  ___  | ____|_ __ | |_ ___ _ __ ___  __| |  _  \r\n | |_) / _ \\ / _ \\| '_ ` _ \\/ __| |  _| | '_ \\| __/ _ | '__/ _ \\/ _` | (_) \r\n |  _ | (_) | (_) | | | | | \\__ \\ | |___| | | | ||  __| | |  __| (_| |  _  \r\n |_| \\_\\___/ \\___/|_| |_| |_|___/ |_____|_| |_|\\__\\___|_|  \\___|\\__,_| (_) \r\n                                                                           ";
            const string roomNumber = "  ____                          \r\n |  _ \\ ___   ___  _ __ ___  _  \r\n | |_) / _ \\ / _ \\| '_ ` _ \\(_) \r\n |  _ | (_) | (_) | | | | | |_  \r\n |_| \\_\\___/ \\___/|_| |_| |_(_) \r\n                                ";

            DrawToken(height + 1, 0, ConsoleColor.Yellow, roomEntered, null);
            DrawToken(height + 1, 75, ConsoleColor.Yellow, NumberToken(Game.roomsEntered), true);
            DrawToken(height + 1, width - 45, ConsoleColor.Red, roomNumber, null);
            DrawToken(height + 1, width - 13, ConsoleColor.Red, NumberToken(Game.roomNumber), true);
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