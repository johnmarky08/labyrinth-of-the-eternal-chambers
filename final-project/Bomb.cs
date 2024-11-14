namespace final_project
{
    internal class Bomb(int x, int y)
    {
        public static readonly int maxBombs = (int)Configurations.MAX_BOMBS_ALLOWED;
        public static int playerLives = (int)Configurations.PLAYER_LIVES;
        public static readonly List<Bomb> bombs = [];
        public static int currentBombs = 0;
        public int X { get; } = x;
        public int Y { get; } = y;
        private readonly DateTime placedAt = DateTime.Now;

        // Define offsets for the explosion range (1 block above, below, left, right, and diagonally)
        public static readonly List<(int, int)> explosionOffsets =
            [
                (-7, 0),   // 1 block above
                (7, 0),    // 1 block below
                (0, -7),   // 1 block to the left
                (0, 7),    // 1 block to the right
                (-7, -7),  // Top-left diagonal
                (-7, 7),   // Top-right diagonal
                (7, -7),   // Bottom-left diagonal
                (7, 7)     // Bottom-right diagonal
            ];

        public bool IsTimerUp() => (DateTime.Now - placedAt).TotalSeconds >= (int)Configurations.BOMB_TICKS;

        // Plant a new bomb.
        public static void Plant()
        {
            if (currentBombs < maxBombs)
            {
                bombs.Add(new Bomb(Game.playerX, Game.playerY));
                currentBombs++;
            }
        }

        // Check bomb if ready to explode.
        public static void Update()
        {
            List<Bomb> explodedBombs = [];

            foreach (var bomb in bombs)
            {
                if (bomb.IsTimerUp())
                {
                    explodedBombs.Add(bomb);
                    Explode(bomb);
                }
            }

            foreach (var bomb in explodedBombs)
            {
                bombs.Remove(bomb);
                currentBombs--;
            }

            if (explodedBombs.Count > 0)
            {
                Map.SpecificDraw(Game.playerY, Game.playerX);
            }
        }

        // Bomb explossion effect.
        public static void Explode(Bomb bomb)
        {
            int centerX = bomb.X;
            int centerY = bomb.Y;
            bool playerCaughtInExplosion = false;

            string[] explosionPattern = Token.bombExplosion.Split('\n');

            DrawToken(centerX, centerY, explosionPattern, ref playerCaughtInExplosion);

            // Draw the 7x7 explosion tokens around the center based on defined offsets
            foreach (var (offsetX, offsetY) in explosionOffsets)
            {
                int startX = centerX + offsetX;
                int startY = centerY + offsetY;
                if (!(Map.Check(startY, startX, Token.leftRightWall) || Map.Check(startY, startX, Token.topBottomWall) || Map.Check(startY, startX, Token.bomb)))
                    DrawToken(startX, startY, explosionPattern, ref playerCaughtInExplosion);
            }

            // Allow time for the explosion to appear on screen
            Thread.Sleep(200);

            ClearExplosionArea(centerX, centerY, explosionOffsets);

            // If player was in the explosion area, reduce life
            if (playerCaughtInExplosion)
                playerLives--;
        }

        // Draw explossion token.
        private static void DrawToken(int startX, int startY, string[] token, ref bool playerCaughtInExplosion)
        {
            for (int offsetY = 0; offsetY < 7; offsetY++)
            {
                for (int offsetX = 0; offsetX < 7; offsetX++)
                {
                    int x = startX + offsetX;
                    int y = startY + offsetY;

                    if (x >= 0 && y >= 0 && x < Map.width && y < Map.height)
                    {
                        char explosionChar = token[offsetY][offsetX];

                        Map.map[y, x] = explosionChar;
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Map.map[y, x]);

                        // Check if the player is caught in the explosion
                        // Check if the current explosion position overlaps with any part of the 7x7 player area
                        for (int playerOffsetY = 0; playerOffsetY < 7; playerOffsetY++)
                        {
                            for (int playerOffsetX = 0; playerOffsetX < 7; playerOffsetX++)
                            {
                                int playerPosX = Game.playerX + playerOffsetX;
                                int playerPosY = Game.playerY + playerOffsetY;

                                // Check if the current explosion coordinates match any player coordinates
                                if (x == playerPosX && y == playerPosY)
                                {
                                    playerCaughtInExplosion = true;
                                    break;
                                }
                            }
                            if (playerCaughtInExplosion)
                                break;
                        }
                    }
                }
            }
        }

        // Remove explossion tokens to all sides.
        public static void ClearExplosionArea(int centerX, int centerY, List<(int, int)> offsets)
        {
            // Clear the central 7x7 block
            if (!Map.Check(centerY, centerX, Token.player))
                ClearToken(centerX, centerY);

            // Clear the surrounding 7x7 blocks
            foreach (var (offsetX, offsetY) in offsets)
            {
                int startX = centerX + offsetX;
                int startY = centerY + offsetY;
                if (!(Map.Check(startY, startX, Token.leftRightWall) || Map.Check(startY, startX, Token.topBottomWall) || Map.Check(startY, startX, Token.bomb)))
                    ClearToken(startX, startY);
            }
        }

        // Remove explossion token.
        private static void ClearToken(int startX, int startY)
        {
            for (int offsetY = 0; offsetY < 7; offsetY++)
            {
                for (int offsetX = 0; offsetX < 7; offsetX++)
                {
                    int x = startX + offsetX;
                    int y = startY + offsetY;

                    if (x >= 0 && y >= 0 && x < Map.width && y < Map.height)
                    {
                        Map.map[y, x] = ' ';
                        Console.SetCursorPosition(x, y);
                        Console.Write(Map.map[y, x]);
                    }
                }
            }
        }
    }
}
