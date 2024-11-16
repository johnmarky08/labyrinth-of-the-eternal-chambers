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
        public static int roomsEntered = 0;
        public static int roomNumber = 0;
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
            Map.InitializeMap();
            Map.RenderBoundaries();
            Map.PlaceExits();
            Map.GeneratePath(playerX, playerY);
            Map.DrawMap();
            Logic.GeneratePattern();

            // Start the game.
            StartGameLoop();
        }

        static void StartGameLoop()
        {
            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        gameOver = true;
                    }

                    MovePlayer(key);
                    Map.SpecificDraw(playerY, playerX); // Update the map only if the player has moved to a valid position
                }
                else if (Logic.currentPattern.Equals(Logic.pattern))
                {
                    gameOver = true;
                }

                Map.DrawScore();
            }

            // Game Over
            Console.Clear();
            Thread.Sleep(50);

            // Enlarge buffersize.
            var simulator = new InputSimulator();
            for (int i = 0; i < 6; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_PLUS);

            // Display game over text.
            Thread.Sleep(100);
            Console.SetCursorPosition(0, 10);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t\t      ___           ___           ___           ___                    ___           ___           ___           ___     \r\n\t\t     /\\  \\         /\\  \\         /\\__\\         /\\  \\                  /\\  \\         /\\__\\         /\\  \\         /\\  \\    \r\n\t\t    /::\\  \\       /::\\  \\       /::|  |       /::\\  \\                /::\\  \\       /:/  /        /::\\  \\       /::\\  \\   \r\n\t\t   /:/\\:\\  \\     /:/\\:\\  \\     /:|:|  |      /:/\\:\\  \\              /:/\\:\\  \\     /:/  /        /:/\\:\\  \\     /:/\\:\\  \\  \r\n\t\t  /:/  \\:\\  \\   /::\\~\\:\\  \\   /:/|:|__|__   /::\\~\\:\\  \\            /:/  \\:\\  \\   /:/__/  ___   /::\\~\\:\\  \\   /::\\~\\:\\  \\ \r\n\t\t /:/__/_\\:\\__\\ /:/\\:\\ \\:\\__\\ /:/ |::::\\__\\ /:/\\:\\ \\:\\__\\          /:/__/ \\:\\__\\  |:|  | /\\__\\ /:/\\:\\ \\:\\__\\ /:/\\:\\ \\:\\__\\\r\n\t\t \\:\\  /\\ \\/__/ \\/__\\:\\/:/  / \\/__/~~/:/  / \\:\\~\\:\\ \\/__/          \\:\\  \\ /:/  /  |:|  |/:/  / \\:\\~\\:\\ \\/__/ \\/_|::\\/:/  /\r\n\t\t  \\:\\ \\:\\__\\        \\::/  /        /:/  /   \\:\\ \\:\\__\\             \\:\\  /:/  /   |:|__/:/  /   \\:\\ \\:\\__\\      |:|::/  / \r\n\t\t   \\:\\/:/  /        /:/  /        /:/  /     \\:\\ \\/__/              \\:\\/:/  /     \\::::/__/     \\:\\ \\/__/      |:|\\/__/  \r\n\t\t    \\::/  /        /:/  /        /:/  /       \\:\\__\\                 \\::/  /       ~~~~          \\:\\__\\        |:|  |    \r\n\t\t     \\/__/         \\/__/         \\/__/         \\/__/                  \\/__/                       \\/__/         \\|__|    \r\n\t\t");
            Console.WriteLine("\n\n\n\n\n\n\n\n\t\t    ____                                                    __                   __                        _  __        \r\n\t\t   / __ \\ _____ ___   _____ _____   ____ _ ____   __  __   / /__ ___   __  __   / /_ ____     ___   _  __ (_)/ /_       \r\n\t\t  / /_/ // ___// _ \\ / ___// ___/  / __ `// __ \\ / / / /  / //_// _ \\ / / / /  / __// __ \\   / _ \\ | |/_// // __/       \r\n\t\t / ____// /   /  __/(__  )(__  )  / /_/ // / / // /_/ /  / ,<  /  __// /_/ /  / /_ / /_/ /  /  __/_>  < / // /_ _  _  _ \r\n\t\t/_/    /_/    \\___//____//____/   \\__,_//_/ /_/ \\__, /  /_/|_| \\___/ \\__, /   \\__/ \\____/   \\___//_/|_|/_/ \\__/(_)(_)(_)\r\n\t\t                                               /____/               /____/                                              \r\n\t\t");
            Console.ReadKey();
        }

        // Main controls method
        static void MovePlayer(ConsoleKey key)
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
                default: return;
            }

            bool isWall = Map.Check(newY, newX, Token.boundary) ||
                  Map.Check(newY, newX, Token.leftRightWall) ||
                  Map.Check(newY, newX, Token.topBottomWall);

            if (Map.IsDoor(newY, newX))
            {
                roomsEntered++;

                int nextDoor = Map.WhatDoor(newY, newX);
                if (Logic.CheckPattern(nextDoor))
                {
                    Logic.currentPattern += nextDoor;
                    roomNumber++;
                }
                else
                {
                    Logic.currentPattern = "";
                    roomNumber = 0;
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
        }
    }
}
