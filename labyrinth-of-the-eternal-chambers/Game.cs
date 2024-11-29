using WindowsInput;

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

        /// <summary>
        /// Initialize and render all needed requirements to run the game and start the loop of the game.
        /// </summary>
        internal static void Execute()
        {
            // Minimize fontsize for ASCII art to fit on screen.
            Program.ToggleFontSize(-6);

            // Game settings loader.
            Thread.Sleep(100);
            Console.CursorVisible = false;
            Map.CreateMaps();
            Map.ChangeMap(roomNumber, playerY, playerX);
            Logic.GeneratePattern();

            // Flash the guide on start up
            InputSimulator simulator = new();
            simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.F1);

            // Start the game.
            Error.Handler(StartGameLoop);
        }

        /// <summary>
        /// This is the main loop of the game that detects when the user moves, wants to quit, loses, or wins the game.
        /// </summary>
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

        /// <summary>
        /// Move the player to its new position and return a boolean that corresponds to whether, when the user moves to the new position, they won or lost.
        /// </summary>
        /// <param name="key">The key that the user presses in the keyboard.</param>
        /// <returns>The booelan that represents whether the user won, lost, or still playing.</returns>
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
