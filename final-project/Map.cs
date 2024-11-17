
namespace final_project
{
    internal class Map
    {
        public static readonly int blockSize = 7;
        public static readonly int width = (int)Configurations.WIDTH;
        public static readonly int height = (int)Configurations.HEIGHT;
        public static readonly int blockWidth = width / blockSize;
        public static readonly int blockHeight = height / blockSize;
        public static readonly List<char[,]> maps = [];
        public static readonly Random random = new();
        private static readonly (int dx, int dy)[] directions =
        {
            (0, -7), // Up
            (7, 0),  // Right
            (0, 7),  // Down
            (-7, 0)  // Left
        };
        public static char[,] currentMap = { };

        // Check if map[y, x] is the same with the given representationToken.
        public static bool Check(char[,] map, int startY, int startX, string representationToken)
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

        // Create and initialize all maps
        public static void CreateMaps()
        {
            for (int i = 0; i < (int)Configurations.PATTERN_LENGTH; i++)
                maps.Add(new char[height, width]);

            foreach (char[,] map in maps)
            {
                InitializeMap(map);
                RenderBoundaries(map);
                PlaceDoors(map);
                GeneratePath(map, Game.defaultPlayerX, Game.defaultPlayerY);
            }
        }

        public static void ChangeMap(int mapNumber, int playerY, int playerX)
        {
            char[,] oldMap = currentMap;
            currentMap = maps[mapNumber - 1];

            if (!oldMap.Equals(currentMap))
            {
                string[] playerLines = Token.player.Split('\n');
                // Remove old player on old map
                for (int i = 0; i < playerLines.Length; i++)
                {
                    int oldPlayerY = playerY + i;
                    if (oldPlayerY < oldMap.GetLength(0))
                    {
                        for (int j = 0; j < playerLines[i].Length; j++)
                        {
                            int oldPlayerX = playerX + j;
                            if (oldPlayerX < oldMap.GetLength(1))
                            {
                                oldMap[oldPlayerY, oldPlayerX] = ' ';
                            }
                        }
                    }
                }
                DrawMap();
            }
        }

        // Generate Walls.
        public static void InitializeMap(char[,] map)
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

        public static void RenderBoundaries(char[,] map)
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

        public static void GeneratePath(char[,] map, int currentX, int currentY, HashSet<(int, int)>? visited = null)
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

            RenderToken(map, startY, startX, Token.whitespace);

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
                    !(Check(map, newY, newX, Token.topBottomWall) || Check(map, newY, newX, Token.leftRightWall) || IsDoor(map, newY, newX)))
                {
                    // Render the path between the current and new cell
                    RenderToken(map, midY, midX, Token.whitespace);
                    RenderToken(map, newY, newX, Token.whitespace);

                    GeneratePath(map, newX, newY, visited); // Recursively generate the maze
                }
            }
        }

        // Ensure exits are accessible and place them at the four corners
        public static void PlaceDoors(char[,] map)
        {
            // Place exits at the four corners
            RenderToken(map, 0, 7, Token.door1);
            RenderToken(map, 0, width - 14, Token.door2);
            RenderToken(map, height - 7, 7, Token.door3);
            RenderToken(map, height - 7, width - 14, Token.door4);
        }

        // Check what door is it
        public static int WhatDoor(int y, int x)
        {
            if (Check(currentMap, y, x, Token.door1)) return 1;
            else if (Check(currentMap, y, x, Token.door2)) return 2;
            else if (Check(currentMap, y, x, Token.door3)) return 3;
            else if (Check(currentMap, y, x, Token.door4)) return 4;
            else return 0;
        }

        private static void RenderToken(char[,] map, int renderY, int renderX, string token)
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

            RenderToken(currentMap, Game.playerY, Game.playerX, Token.player);

            if (Check(currentMap, Game.oldPlayerY, Game.oldPlayerX, Token.player) && (!(Game.oldPlayerX == Game.playerX && Game.oldPlayerY == Game.playerY)))
            {
                // Remove old player on old posistion
                for (int i = 0; i < playerLines.Length; i++)
                {
                    int oldPlayerY = Game.oldPlayerY + i;
                    if (oldPlayerY < currentMap.GetLength(0))
                    {
                        for (int j = 0; j < playerLines[i].Length; j++)
                        {
                            int oldPlayerX = Game.oldPlayerX + j;
                            if (oldPlayerX < currentMap.GetLength(1))
                            {
                                currentMap[oldPlayerY, oldPlayerX] = ' ';
                                Console.SetCursorPosition(oldPlayerX, oldPlayerY);
                                Console.Write(currentMap[oldPlayerY, oldPlayerX]);
                            }
                        }
                    }
                }
            }

            // Draw tokens.
            int adjustedY = y / blockSize * blockSize;
            int adjustedX = x / blockSize * blockSize;
            if (Check(currentMap, adjustedY, adjustedX, Token.boundary)) DrawToken(y, x, ConsoleColor.Gray, Token.boundary, null);
            else if (Check(currentMap, adjustedY, adjustedX, Token.player)) DrawToken(y, x, ConsoleColor.Green, Token.player, null);
            else DrawToken(y, x, ConsoleColor.White, null, null);
        }

        // Token drawer (ASCII Art Supported)
        public static void DrawToken(int y, int x, ConsoleColor color, string? chosenToken, bool? isScore)
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
            RenderToken(currentMap, Game.playerY, Game.playerX, Token.player);
            Console.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.SetCursorPosition(x, y);

                    // Adjust coordinates to match the 7x7 grid
                    int adjustedY = y / blockSize * blockSize;
                    int adjustedX = x / blockSize * blockSize;

                    if (Check(currentMap, adjustedY, adjustedX, Token.boundary))
                    {
                        Console.ResetColor(); // Boundary color
                        Console.Write(currentMap[y, x]);
                    }
                    else if (Check(currentMap, adjustedY, adjustedX, Token.player))
                    {
                        Console.ForegroundColor = ConsoleColor.Green; // Player color
                        Console.Write(currentMap[y, x]);
                    }
                    else if (IsDoor(currentMap, adjustedY, adjustedX))
                    {
                        Console.ForegroundColor = ConsoleColor.Green; // Teleportation color
                        Console.Write(currentMap[y, x]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray; // Default color for other spaces
                        Console.Write(currentMap[y, x]);
                    }
                }
            }
        }

        public static bool IsDoor(char[,] map, int y, int x)
        {
            return (Check(map, y, x, Token.door1) || Check(map, y, x, Token.door2) || Check(map, y, x, Token.door3) || Check(map, y, x, Token.door4));
        }

        // Display current player status.
        public static void DrawScore()
        {
            ConsoleColor roomNumberColor = Game.roomNumber switch
            {
                ((int)Configurations.PATTERN_LENGTH / 1) => ConsoleColor.Green,
                > ((int)Configurations.PATTERN_LENGTH / 2) => ConsoleColor.Yellow,
                <= ((int)Configurations.PATTERN_LENGTH / 2) => ConsoleColor.Red,
            };

            ConsoleColor wrongDoorsColor = Game.wrongDoors switch
            {
                < 5 => ConsoleColor.Green,
                < 8 => ConsoleColor.Yellow,
                _ => ConsoleColor.Red,
            };

            DrawToken(height + 1, 0, wrongDoorsColor, Token.wrongDoors1, null);
            DrawToken(height + 1, 66, wrongDoorsColor, Token.ConvertNumber(Game.wrongDoors), true);
            DrawToken(height + 1, width - 45, roomNumberColor, Token.roomNumber, null);
            DrawToken(height + 1, width - 13, roomNumberColor, Token.ConvertNumber(Game.roomNumber), true);
        }
    }
}