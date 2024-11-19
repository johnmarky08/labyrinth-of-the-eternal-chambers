using WindowsInput;
using WindowsInput.Native;
namespace labyrinth_of_the_eternal_chambers
{
    internal class Game
    {
        public static int defaultPlayerX = ((int)Configurations.WIDTH / 2) / Map.blockSize * Map.blockSize;
        public static int defaultPlayerY = ((int)Configurations.HEIGHT / 2) / Map.blockSize * Map.blockSize;
        public static int playerX = defaultPlayerX;
        public static int playerY = defaultPlayerY;
        public static int oldPlayerX = playerX;
        public static int oldPlayerY = playerY;
        public static int wrongDoors = 0;
        public static int roomNumber = 1;
        public static bool won = false;
        public static bool gameOver = false;

        // Main Game Methods
        internal static void Execute()
        {
            // Minimize buffersize for ASCII art to fit on screen.
            InputSimulator simulator = new();
            for (int i = 0; i < 6; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_MINUS);

            // Game settings loader.
            Thread.Sleep(100);
            Console.CursorVisible = false;
            Map.CreateMaps();
            Map.ChangeMap(roomNumber, playerY, playerX);
            Logic.GeneratePattern();

            // Start the game.
            Error.Handler(StartGameLoop);
        }

        private static void StartGameLoop()
        {
            while (!won || !gameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                        Menu.Exit(true);
                    else if (key == ConsoleKey.F1)
                        Menu.Guide();

                    if (MovePlayer(key)) break;
                    Map.SpecificDraw(playerY, playerX); // Update the map only if the player has moved to a valid position
                }

                Map.DrawGuides();
            }

            Menu.GameOver();
        }

        // Main controls method
        private static bool MovePlayer(ConsoleKey key)
        {
            int newX = playerX, newY = playerY;

            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    newY -= 7;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    newY += 7;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    newX -= 7;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    newX += 7;
                    break;
                default: return false;
            }

            bool isWall = Map.Check(Map.currentMap, newY, newX, Token.boundary) ||
                  Map.Check(Map.currentMap, newY, newX, Token.leftRightWall) ||
                  Map.Check(Map.currentMap, newY, newX, Token.topBottomWall);

            if (Map.IsDoor(Map.currentMap, newY, newX))
            {
                int nextDoor = Map.WhatDoor(newY, newX);
                if (Logic.CheckPattern(nextDoor))
                {
                    Logic.currentPattern += nextDoor;

                    if (Logic.currentPattern.Equals(Logic.pattern))
                    {
                        won = true;
                        return true;
                    }

                    Map.ChangeMap(++roomNumber, playerY, playerX);
                }
                else
                {
                    if (++wrongDoors >= (int)Configurations.MAX_GUESS)
                    {
                        gameOver = true;
                        return true;
                    }
                    Logic.currentPattern = "";
                    Map.ChangeMap(roomNumber = 1, playerY, playerX);
                }

                oldPlayerX = playerX;
                oldPlayerY = playerY;
                playerX = defaultPlayerX;
                playerY = defaultPlayerY;
            }
            else if (!isWall && newX > 0 && newX < Map.width && newY > 0 && newY < Map.height)
            {
                oldPlayerX = playerX;
                oldPlayerY = playerY;
                playerX = newX;
                playerY = newY;
            }

            return false;
        }
    }
}
