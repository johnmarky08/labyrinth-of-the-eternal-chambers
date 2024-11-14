using WindowsInput;
using WindowsInput.Native;
namespace final_project
{
    internal class Game
    {
        public static int playerX = 7;
        public static int playerY = 7;
        public static int oldPlayerX = 0;
        public static int oldPlayerY = 0;
        static bool gameOver = false;
        // Ensure no boundaries at the starting point
        public static readonly List<(int, int)> clearBoundary =
            [
                (7, 0),    // 1 blocks below
                (0, 7),    // 1 blocks to the right
                (7, 7),    // 1 blocks to the bottom-right
                (14, 0),    // 2 blocks below
                (0, 14),    // 2 blocks to the right
                (14, 14),    // 2 blocks to the bottom-right
                (14, 7),    // 2 blocks below, 1 block right
                (7, 14),    // 1 block below, 2 blocks right
            ];

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
            Map.GenerateBoundaries();
            Map.DrawMap();

            // Recycle ClearExplosionArea Method to ensure no boundaries at the player.
            Bomb.ClearExplosionArea(playerX, playerY, clearBoundary);

            // Start the game.
            StartGameLoop();
        }

        static void StartGameLoop()
        {
            while (Bomb.playerLives > 0 && !gameOver)
            {
                Map.DrawScore();
                Bomb.Update();

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        gameOver = true;
                    }

                    Thread.Sleep(100);
                    MovePlayer(key);
                    Map.SpecificDraw(playerY, playerX); // Update the map only if the player has moved to a valid position
                }

                Thread.Sleep(50);
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
                    newY = playerY - 7;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    newY = playerY + 7;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    newX = playerX - 7;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    newX = playerX + 7;
                    break;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    Bomb.Plant();
                    return;
                default: return;
            }

            bool isWall = Map.Check(newY, newX, Token.boundary) ||
                  Map.Check(newY, newX, Token.leftRightWall) ||
                  Map.Check(newY, newX, Token.topBottomWall);

            if (!isWall && newX > 0 && newX < Map.width && newY > 0 && newY < Map.height)
            {
                oldPlayerX = playerX;
                oldPlayerY = playerY;
                playerX = newX;
                playerY = newY;
            }
        }
    }
}
