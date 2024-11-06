using System;

namespace final_project
{
    internal class Map
    {
        public static readonly int width = (int)Configurations.WIDTH;
        public static readonly int height = (int)Configurations.HEIGHT;
        public static readonly char playerToken = (char)Configurations.PLAYER_TOKEN;
        public static readonly char bombToken = (char)Configurations.BOMB_TOKEN;
        public static readonly char boundaryToken = (char)Configurations.BOUNDARY_TOKEN;
        public static readonly char[,] map = new char[height, width];
        public static readonly Random random = new();

        public static void InitializeMap()
        {
            // Initialize the map with wall boundaries
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == 0 || y == height - 1)
                        map[y, x] = (char)Configurations.WALL_TOP_BOTTOM;
                    else if (x == 0 || x == width - 1)
                        map[y, x] = (char)Configurations.WALL_LEFT_RIGHT;
                    else
                        map[y, x] = ' ';
                }
            }
        }

        public static void GenerateBoundaries()
        {
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    map[y, x] = (random.Next(0, 2) == 0) ? boundaryToken : ' ';
                }
            }

            map[Game.playerY, Game.playerX] = ' ';

            // Create a random boundary using a depth-first search method
            // Starting point for boundary generation
            CarvePath(random.Next(1, height - 1), random.Next(1, width - 1));
        }

        // Recursive method to carve the boundary
        public static void CarvePath(int y, int x)
        {
            // Directions to carve: up, down, left, right
            var directions = new (int, int)[] { (-2, 0), (2, 0), (0, -2), (0, 2) };
            // Shuffle directions to ensure randomness
            directions = directions.OrderBy(d => random.Next()).ToArray();

            foreach (var (dy, dx) in directions)
            {
                int newY = y + dy, newX = x + dx;

                // Check if the new position is within bounds and is a wall
                if (newY > 0 && newY < height - 1 && newX > 0 && newX < width - 1 && map[newY, newX] == '%')
                {
                    // Carve a path to the new position
                    map[newY, newX] = ' ';
                    // Carve a path in between (to ensure the boundary is interconnected)
                    map[y + dy / 2, x + dx / 2] = ' ';

                    // Recursively carve further
                    CarvePath(newY, newX);
                }
            }
        }

        public static void DrawMap()
        {
            Console.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.SetCursorPosition(x, y);
                    if (map[y, x] == boundaryToken)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(map[y, x]);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(map[y, x]);
                    }
                }
            }

            Console.SetCursorPosition(Game.playerX, Game.playerY);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(playerToken);

            foreach (var bomb in Bomb.bombs)
            {
                Console.SetCursorPosition(bomb.X, bomb.Y);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(bombToken);
            }

            Console.ResetColor();
            // Display remaining lives and bombs
            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine($"Lives Remaining: {Bomb.playerLives}\nBombs Remaining: {Bomb.maxBombs - Bomb.currentBombs} (Will regenerate after explosion)");
        }
    }
}
