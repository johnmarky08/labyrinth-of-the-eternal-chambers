namespace labyrinth_of_the_eternal_chambers
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
        public static char[,] currentMap = { };
        public static int currentMapNumber = 0;
        private static readonly (int dx, int dy)[] directions =
        {
            (0, -7), // Up
            (7, 0),  // Right
            (0, 7),  // Down
            (-7, 0)  // Left
        };
        public static readonly (int, int)[] shadowMaze =
        {
            (-7, 0),   // 1 block above
            (7, 0),    // 1 block below
            (0, -7),   // 1 block to the left
            (0, 7),    // 1 block to the right
            (-7, -7),  // Top-left diagonal
            (-7, 7),   // Top-right diagonal
            (7, -7),   // Bottom-left diagonal
            (7, 7)     // Bottom-right diagonal
        };

        /// <summary>
        /// Check whether the given startX and startY match the representationToken in the current map.
        /// </summary>
        /// <param name="map">The current map.</param>
        /// <param name="startY">The starting Y of the token.</param>
        /// <param name="startX">The starting X of the token.</param>
        /// <param name="representationToken">The ASCII Art/Token you wish to know if it matches or not with the token associated within the given <paramref name="startY"/> and <paramref name="startX"/>.</param>
        /// <returns>The boolean that represents whether it matches or not.</returns>
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

        /// <summary>
        /// Create and initialize all maps. Generate its borders, boundaries, doors, and maze path.
        /// </summary>
        public static void CreateMaps()
        {
            for (int i = 0; i < (int)Configurations.PATTERN_LENGTH; i++)
                maps.Add(new char[height, width]);

            foreach (char[,] map in maps)
            {
                RenderWalls(map);
                RenderBoundaries(map);
                PlaceDoors(map);
                GeneratePath(map, Game.defaultPlayerX, Game.defaultPlayerY);
            }
        }

        /// <summary>
        /// Changes the current rendered map to a new map and sets it as the current map, then renders it on the console. And uses the parameters playerX and playerY to remove them from the previous map.
        /// </summary>
        /// <param name="mapNumber">The map mumber you want to change to.</param>
        /// <param name="playerY">The old Y position of the player in the previous map.</param>
        /// <param name="playerX">The old X position of the player in the previous map.</param>
        public static void ChangeMap(int mapNumber, int playerY, int playerX)
        {
            char[,] oldMap = currentMap;
            currentMap = maps[mapNumber - 1];
            currentMapNumber = mapNumber;

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

        /// <summary>
        /// To render the whole map and render the walls of it.
        /// </summary>
        /// <param name="map">Current map.</param>
        public static void RenderWalls(char[,] map)
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

        /// <summary>
        /// To render the boundaries to the inside of the map.
        /// </summary>
        /// <param name="map">Current map.</param>
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

        /// <summary>
        /// A recursive method that randomly generates a maze-like pattern of white spaces and renders it on the map.
        /// </summary>
        /// <param name="map">Current map.</param>
        /// <param name="currentX">Current position X.</param>
        /// <param name="currentY">Current position Y.</param>
        /// <param name="visited">Uses HashSet data type to ensure that it only visits that particular block once.</param>
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

            Random random = new();
            List<(int, int)> directionsList = new(directions);
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

        /// <summary>
        /// Render all 4 doors to each corner of the map.
        /// </summary>
        /// <param name="map">Current map.</param>
        public static void PlaceDoors(char[,] map)
        {
            // Place exits at the four corners
            RenderToken(map, 0, 7, Token.door1);
            RenderToken(map, 0, width - 14, Token.door2);
            RenderToken(map, height - 7, 7, Token.door3);
            RenderToken(map, height - 7, width - 14, Token.door4);
        }

        /// <summary>
        /// A supporting method that returns the number of which door the player entered.
        /// </summary>
        /// <param name="y">Door position Y.</param>
        /// <param name="x">Door position X.</param>
        /// <returns>The door number.</returns>
        public static int WhatDoor(int y, int x)
        {
            if (Check(currentMap, y, x, Token.door1)) return 1;
            else if (Check(currentMap, y, x, Token.door2)) return 2;
            else if (Check(currentMap, y, x, Token.door3)) return 3;
            else if (Check(currentMap, y, x, Token.door4)) return 4;
            else return 0;
        }

        /// <summary>
        /// To render the given token to the exact x and y of it in the map.
        /// </summary>
        /// <param name="map">Current map.</param>
        /// <param name="renderY">Y position where you want to render it.</param>
        /// <param name="renderX">X position where you want to render it.</param>
        /// <param name="token">The ASCII Art/Token you want to render.</param>
        public static void RenderToken(char[,] map, int renderY, int renderX, string token)
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

        /// <summary>
        /// This method writes the given x and y of the map to the console.
        /// </summary>
        /// <param name="y">Position Y in the map and console.</param>
        /// <param name="x">Position X in the map and console.</param>
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
            if (currentMapNumber == maps.Count)
            {
                int currentY = Game.playerY;
                int currentX = Game.playerX;
                int oldY = Game.oldPlayerY;
                int oldX = Game.oldPlayerX;
                List<(int, int)> shadows = [];
                foreach (var (offsetY, offsetX) in shadowMaze)
                {
                    // Write boundaries that are near the player.
                    int startX = currentX + offsetX;
                    int startY = currentY + offsetY;
                    shadows.Add((startY, startX));

                    if (Check(currentMap, startY, startX, Token.boundary))
                        DrawToken(startY, startX, ConsoleColor.White, Token.boundary);
                }

                foreach (var (offsetY, offsetX) in shadowMaze)
                {
                    // Remove boundaries that are far away from the player.
                    int oldStartX = oldX + offsetX;
                    int oldStartY = oldY + offsetY;

                    if ((!shadows.Contains((oldStartY, oldStartX)) && Check(currentMap, oldStartY, oldStartX, Token.boundary)))
                        DrawToken(oldStartY, oldStartX, ConsoleColor.White, Token.whitespace);
                }
            }

            if (Check(currentMap, adjustedY, adjustedX, Token.boundary)) DrawToken(y, x, ConsoleColor.White, Token.boundary);
            else if (Check(currentMap, adjustedY, adjustedX, Token.player)) DrawToken(y, x, ConsoleColor.Green, Token.player);
            else DrawToken(y, x, ConsoleColor.DarkGray);
        }

        /// <summary>
        /// Writes the given token or ASCII Art to the console with the given color, x, and y.
        /// </summary>
        /// <param name="y">Starting Y position.</param>
        /// <param name="x">Starting X position.</param>
        /// <param name="color">Font color.</param>
        /// <param name="chosenToken">ASCII Art Token</param>
        /// <param name="isScore">Boolean that represents if the given <paramref name="chosenToken"/> is a token or a score.</param>
        public static void DrawToken(int y, int x, ConsoleColor color, string chosenToken = "", bool isScore = false)
        {
            Console.ForegroundColor = color;

            if (chosenToken == "")
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

        /// <summary>
        /// Prints the whole map to the console.
        /// </summary>
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

                        if (currentMapNumber == maps.Count)
                        {
                            int currentY = Game.playerY - adjustedY;
                            int currentX = Game.playerX - adjustedX;
                            if (shadowMaze.Contains((currentY, currentX))) Console.Write(currentMap[y, x]);
                        }
                        else
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

        /// <summary>
        /// Checks if the given x and y match the coordinates of a door.
        /// </summary>
        /// <param name="map">Current map.</param>
        /// <param name="y">Starting position Y.</param>
        /// <param name="x">Starting position X.</param>
        /// <returns>Boolean that represents whether it is a door or not.</returns>
        public static bool IsDoor(char[,] map, int y, int x)
        {
            return (Check(map, y, x, Token.door1) || Check(map, y, x, Token.door2) || Check(map, y, x, Token.door3) || Check(map, y, x, Token.door4));
        }

        /// <summary>
        /// Prints the guides, current wrong doors, and current room number in the console.
        /// </summary>
        public static void DrawGuides()
        {
            ConsoleColor roomNumberColor = Game.roomNumber switch
            {
                (int)Configurations.PATTERN_LENGTH => ConsoleColor.Green,
                > ((int)Configurations.PATTERN_LENGTH / 2) => ConsoleColor.Yellow,
                <= ((int)Configurations.PATTERN_LENGTH / 2) => ConsoleColor.Red,
            };

            ConsoleColor wrongDoorsColor = Game.wrongDoors switch
            {
                <= ((int)Configurations.MAX_GUESS / 2) => ConsoleColor.Green,
                < ((int)Configurations.MAX_GUESS - 1) => ConsoleColor.Yellow,
                _ => ConsoleColor.Red,
            };

            DrawToken(height + 1, 0, wrongDoorsColor, Token.wrongDoors1);
            DrawToken(height + 1, 66, wrongDoorsColor, Token.ConvertNumber(Game.wrongDoors.ToString()), true);
            DrawToken(height + 1, width - 45, roomNumberColor, Token.roomNumber);
            DrawToken(height + 1, width - 13, roomNumberColor, Token.ConvertNumber(Game.roomNumber.ToString()), true);

            // Place guide token
            DrawToken(5, width + 10, ConsoleColor.DarkGreen, Token.guide);
        }
    }
}