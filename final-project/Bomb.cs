namespace final_project
{
    internal class Bomb(int x, int y)
    {
        public static readonly int maxBombs = (int)Configurations.MAX_BOMBS_ALLOWED;
        public static readonly List<Bomb> bombs = [];
        public static int playerLives = 3;
        public static int currentBombs = 0;
        public int X { get; } = x;
        public int Y { get; } = y;
        private readonly DateTime placedAt = DateTime.Now;

        public bool IsTimerUp() => (DateTime.Now - placedAt).TotalSeconds >= 3;

        public static void Plant()
        {
            if (currentBombs < maxBombs)
            {
                bombs.Add(new Bomb(Game.playerX, Game.playerY));
                currentBombs++;
            }
        }

        public static void Update()
        {
            List<Bomb> explodedBombs = [];

            foreach (var bomb in bombs)
            {
                if (bomb.IsTimerUp())
                {
                    Explode(bomb);
                    explodedBombs.Add(bomb);
                }
            }

            // Remove exploded bombs
            foreach (var bomb in explodedBombs)
            {
                bombs.Remove(bomb);
                currentBombs--;
            }

            if (explodedBombs.Count > 0)
            {
                Map.DrawMap();
            }
        }

        public static void Explode(Bomb bomb)
        {
            int centerX = bomb.X;
            int centerY = bomb.Y;
            bool playerCaughtInExplosion = false;

            // Define explosion range as a cross pattern
            var explosionRange = new (int, int)[] {
                (centerX, centerY),  // Center
                (centerX - 1, centerY), // Left
                (centerX + 1, centerY), // Right
                (centerX, centerY - 1), // Up
                (centerX, centerY + 1)  // Down
            };

            // Draw explosion
            foreach (var (x, y) in explosionRange)
            {
                if (x >= 0 && y >= 0 && x < Map.width && y < Map.height)
                {
                    // Make sure it's not a wall or boundary, else leave it intact
                    if (!new char[] { (char)Configurations.WALL_LEFT_RIGHT, (char)Configurations.WALL_TOP_BOTTOM }.Contains(Map.map[y, x]))
                    {
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write((char)Configurations.EXPLOSION_TOKEN);

                        // Check if the player is caught in the explosion
                        if (x == Game.playerX && y == Game.playerY)
                        {
                            playerCaughtInExplosion = true;
                        }
                    }
                }
            }

            // Allow time for explosion to appear on screen
            Thread.Sleep(200);

            // Clear explosion area (avoid clearing walls)
            foreach (var (x, y) in explosionRange)
            {
                if (x >= 0 && y >= 0 && x < Map.width && y < Map.height)
                {
                    // Only clear if it's not a wall or boundary
                    if (Map.map[y, x] != (char)Configurations.WALL_LEFT_RIGHT && Map.map[y, x] != (char)Configurations.WALL_TOP_BOTTOM)
                    {
                        Map.map[y, x] = ' ';
                    }
                }
            }

            // If player was in explosion area, reduce life
            if (playerCaughtInExplosion)
                playerLives--;
        }
    }
}
