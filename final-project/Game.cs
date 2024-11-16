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

        static void StartGameLoop()
        {
            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                        gameOver = true;

                    if (MovePlayer(key)) break;
                    Map.SpecificDraw(playerY, playerX); // Update the map only if the player has moved to a valid position
                }

                Map.DrawScore();
            }

            // Game Over
            Console.Clear();
            Thread.Sleep(50);

            // Enlarge buffersize.
            var simulator = new InputSimulator();
            for (int i = 0; i < 5; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_PLUS);

            // Display game over text.
            Thread.Sleep(100);
            Console.SetCursorPosition(0, 5);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("        _ _    ___                                    _            _          _     _                     _    __   __                 _                            \n       ( | )  / __|  ___   _ _    __ _   _ _   __ _  | |_   _  _  | |  __ _  | |_  (_)  ___   _ _    ___ | |   \\ \\ / /  ___   _  _    | |_    __ _  __ __  ___      \n        V V  | (__  / _ \\ | ' \\  / _` | | '_| / _` | |  _| | || | | | / _` | |  _| | | / _ \\ | ' \\  (_-< |_|    \\ V /  / _ \\ | || |   | ' \\  / _` | \\ V / / -_)     \n              \\___| \\___/ |_||_| \\__, | |_|   \\__,_|  \\__|  \\_,_| |_| \\__,_|  \\__| |_| \\___/ |_||_| /__/ (_)     |_|   \\___/  \\_,_|   |_||_| \\__,_|  \\_/  \\___|     \n                                 |___/                                                                                                                              \n                                  _                 _              _     _     _                                  _                 _                               \n              _ _    __ _  __ __ (_)  __ _   __ _  | |_   ___   __| |   | |_  | |_    ___     _ __    _  _   ___ | |_   ___   _ _  (_)  ___   _  _   ___            \n             | ' \\  / _` | \\ V / | | / _` | / _` | |  _| / -_) / _` |   |  _| | ' \\  / -_)   | '  \\  | || | (_-< |  _| / -_) | '_| | | / _ \\ | || | (_-<            \n             |_||_| \\__,_|  \\_/  |_| \\__, | \\__,_|  \\__| \\___| \\__,_|    \\__| |_||_| \\___|   |_|_|_|  \\_, | /__/  \\__| \\___| |_|   |_| \\___/  \\_,_| /__/            \n                                     |___/                                                            |__/                                                          \n      _              _        _                             _     _                                                 _                                            _  \n     | |_  __ __ __ (_)  ___ | |_   ___    __ _   _ _    __| |   | |_   _  _   _ _   _ _    ___    __ _   _ _    __| |    ___   ___  __   __ _   _ __   ___   __| | \n     |  _| \\ V  V / | | (_-< |  _| (_-<   / _` | | ' \\  / _` |   |  _| | || | | '_| | ' \\  (_-<   / _` | | ' \\  / _` |   / -_) (_-< / _| / _` | | '_ \\ / -_) / _` | \n      \\__|  \\_/\\_/  |_| /__/  \\__| /__/   \\__,_| |_||_| \\__,_|    \\__|  \\_,_| |_|   |_||_| /__/   \\__,_| |_||_| \\__,_|   \\___| /__/ \\__| \\__,_| | .__/ \\___| \\__,_| \n                                                                                                                                                |_|                 \n                _     _              _             _                   _          _     _               __     ___   _                               _              \n               | |_  | |_    ___    | |     __ _  | |__   _  _   _ _  (_)  _ _   | |_  | |_      ___   / _|   | __| | |_   ___   _ _   _ _    __ _  | |             \n               |  _| | ' \\  / -_)   | |__  / _` | | '_ \\ | || | | '_| | | | ' \\  |  _| | ' \\    / _ \\ |  _|   | _|  |  _| / -_) | '_| | ' \\  / _` | | |             \n                \\__| |_||_| \\___|   |____| \\__,_| |_.__/  \\_, | |_|   |_| |_||_|  \\__| |_||_|   \\___/ |_|     |___|  \\__| \\___| |_|   |_||_| \\__,_| |_|             \n                                                          |__/                                                                                                      \n       ___   _                     _                            __   __                         _                                                            _      \n      / __| | |_    __ _   _ __   | |__   ___   _ _   ___       \\ \\ / /  ___   _  _   _ _      (_)  ___   _  _   _ _   _ _    ___   _  _     ___   _ _    __| |  ___\n     | (__  | ' \\  / _` | | '  \\  | '_ \\ / -_) | '_| (_-<  _     \\ V /  / _ \\ | || | | '_|     | | / _ \\ | || | | '_| | ' \\  / -_) | || |   / -_) | ' \\  / _` | (_-<\n      \\___| |_||_| \\__,_| |_|_|_| |_.__/ \\___| |_|   /__/ (_)     |_|   \\___/  \\_,_| |_|      _/ | \\___/  \\_,_| |_|   |_||_| \\___|  \\_, |   \\___| |_||_| \\__,_| /__/\n                                                                                             |__/                                   |__/                            \n        _                              _             _       _     _                                                _                       __     _     _          \n       | |_    ___   _ _   ___        | |__   _  _  | |_    | |_  | |_    ___     _ __    ___   _ __    ___   _ _  (_)  ___   ___    ___   / _|   | |_  | |_    ___ \n       | ' \\  / -_) | '_| / -_)  _    | '_ \\ | || | |  _|   |  _| | ' \\  / -_)   | '  \\  / -_) | '  \\  / _ \\ | '_| | | / -_) (_-<   / _ \\ |  _|   |  _| | ' \\  / -_)\n       |_||_| \\___| |_|   \\___| ( )   |_.__/  \\_,_|  \\__|    \\__| |_||_| \\___|   |_|_|_| \\___| |_|_|_| \\___/ |_|   |_| \\___| /__/   \\___/ |_|      \\__| |_||_| \\___|\n                                |/                                                                                                                                  \n                            _   _                     _             _   _                   _   _   _          _                              _   _     _           \n            ___   _ _    __| | | |  ___   ___  ___   | |_    __ _  | | | |  ___   __ __ __ (_) | | | |    ___ | |_   __ _   _  _    __ __ __ (_) | |_  | |_         \n           / -_) | ' \\  / _` | | | / -_) (_-< (_-<   | ' \\  / _` | | | | | (_-<   \\ V  V / | | | | | |   (_-< |  _| / _` | | || |   \\ V  V / | | |  _| | ' \\        \n           \\___| |_||_| \\__,_| |_| \\___| /__/ /__/   |_||_| \\__,_| |_| |_| /__/    \\_/\\_/  |_| |_| |_|   /__/  \\__| \\__,_|  \\_, |    \\_/\\_/  |_|  \\__| |_||_|       \n                                                                                                                            |__/                                    \n                                                                       __                                           _ _                                             \n                                                _  _   ___   _  _     / _|  ___   _ _   ___  __ __  ___   _ _      ( | )                                            \n                                               | || | / _ \\ | || |   |  _| / _ \\ | '_| / -_) \\ V / / -_) | '_|  _   V V                                             \n                                                \\_, | \\___/  \\_,_|   |_|   \\___/ |_|   \\___|  \\_/  \\___| |_|   (_)                                                  \n                                                |__/                                                                                                                ");
            Console.ReadKey();
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
                roomsEntered++;

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
