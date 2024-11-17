using WindowsInput;
using WindowsInput.Native;
namespace final_project
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
        public static bool gameOver = false;

        // Main Game Methods
        internal static void Execute()
        {
            // Minimize buffersize for ASCII art to fit on screen.
            var simulator = new InputSimulator();
            for (int i = 0; i < 6; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_MINUS);

            // Game settings loader.
            Thread.Sleep(100);
            Console.CursorVisible = false;
            Map.CreateMaps();
            Map.ChangeMap(roomNumber, playerY, playerX);
            Logic.GeneratePattern();

            // Start the game.
            StartGameLoop();
        }

        private static void StartGameLoop()
        {
            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                        Menu.Exit(Console.BufferHeight, Console.BufferWidth, true);

                    if (MovePlayer(key)) break;
                    Map.SpecificDraw(playerY, playerX); // Update the map only if the player has moved to a valid position
                }

                Map.DrawScore();
            }

            Menu.Won();
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
                        gameOver = true;
                        return true;
                    }

                    Map.ChangeMap(++roomNumber, playerY, playerX);
                }
                else
                {
                    wrongDoors++;
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
