using WindowsInput;

namespace final_project
{
    internal class Program
    {
        private static bool exitFlag = false;
        static void Main()
        {
            // Make the console full screen.
            Console.CursorVisible = false;
            var simulator = new InputSimulator();
            simulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.MENU, WindowsInput.Native.VirtualKeyCode.RETURN);
            Thread.Sleep(100);
            if (OperatingSystem.IsWindows())
                Console.SetBufferSize(Console.WindowWidth, Console.LargestWindowHeight);

            // Start game if presses space.
            Thread.Sleep(100);
            Thread keyListenerThread = new(KeyListener);
            keyListenerThread.Start();
            bool nextText = true;

            while (!exitFlag)
            {
                const string gameTitle = "\n\n\n\n\n                                                                                                                                                       \r\n          _____          _____         ______  _______         _____        ______        _____        ______  _______         _____  _____   ______   \r\n     ___|\\     \\    ____|\\    \\       |      \\/       \\   ___|\\     \\   ___|\\     \\   ___|\\    \\      |      \\/       \\    ___|\\    \\|\\    \\ |\\     \\  \r\n    |    |\\     \\  /     /\\    \\     /          /\\     \\ |    |\\     \\ |     \\     \\ |    |\\    \\    /          /\\     \\  /    /\\    \\\\\\    \\| \\     \\ \r\n    |    | |     |/     /  \\    \\   /     /\\   / /\\     ||    | |     ||     ,_____/||    | |    |  /     /\\   / /\\     ||    |  |    |\\|    \\  \\     |\r\n    |    | /_ _ /|     |    |    | /     /\\ \\_/ / /    /||    | /_ _ / |     \\--'\\_|/|    |/____/  /     /\\ \\_/ / /    /||    |__|    | |     \\  |    |\r\n    |    |\\    \\ |     |    |    ||     |  \\|_|/ /    / ||    |\\    \\  |     /___/|  |    |\\    \\ |     |  \\|_|/ /    / ||    .--.    | |      \\ |    |\r\n    |    | |    ||\\     \\  /    /||     |       |    |  ||    | |    | |     \\____|\\ |    | |    ||     |       |    |  ||    |  |    | |    |\\ \\|    |\r\n    |____|/____/|| \\_____\\/____/ ||\\____\\       |____|  /|____|/____/| |____ '     /||____| |____||\\____\\       |____|  /|____|  |____| |____||\\_____/|\r\n    |    /     || \\ |    ||    | /| |    |      |    | / |    /     || |    /_____/ ||    | |    || |    |      |    | / |    |  |    | |    |/ \\|   ||\r\n    |____|_____|/  \\|____||____|/  \\|____|      |____|/  |____|_____|/ |____|     | /|____| |____| \\|____|      |____|/  |____|  |____| |____|   |___|/\r\n      \\(    )/        \\(    )/        \\(          )/       \\(    )/      \\( |_____|/   \\(     )/      \\(          )/       \\(      )/     \\(       )/  \r\n       '    '          '    '          '          '         '    '        '    )/       '     '        '          '         '      '       '       '   \r\n                                                                               '                                                                       \r\n    ";
                string[] gameStartingButtonsTexts = [
                    "\n\n\n\n\n\n\t\t\t__       ____                             _____  ____   ___    ______ ______   __                  __                __  \r\n\t\t\t\\ \\     / __ \\ _____ ___   _____ _____   / ___/ / __ \\ /   |  / ____// ____/  / /_ ____     _____ / /_ ____ _ _____ / /_ \r\n\t\t\t \\ \\   / /_/ // ___// _ \\ / ___// ___/   \\__ \\ / /_/ // /| | / /    / __/    / __// __ \\   / ___// __// __ `// ___// __/ \r\n\t\t\t / /  / ____// /   /  __/(__  )(__  )   ___/ // ____// ___ |/ /___ / /___   / /_ / /_/ /  (__  )/ /_ / /_/ // /   / /_ _ \r\n\t\t\t/_/  /_/    /_/    \\___//____//____/   /____//_/    /_/  |_|\\____//_____/   \\__/ \\____/  /____/ \\__/ \\__,_//_/    \\__/(_)",
                    "\r\n\t\t\t__       ____                             ______ _____  ______   __                          _  __  \r\n\t\t\t\\ \\     / __ \\ _____ ___   _____ _____   / ____// ___/ / ____/  / /_ ____     ____ _ __  __ (_)/ /_ \r\n\t\t\t \\ \\   / /_/ // ___// _ \\ / ___// ___/  / __/   \\__ \\ / /      / __// __ \\   / __ `// / / // // __/ \r\n\t\t\t / /  / ____// /   /  __/(__  )(__  )  / /___  ___/ // /___   / /_ / /_/ /  / /_/ // /_/ // // /_ _ \r\n\t\t\t/_/  /_/    /_/    \\___//____//____/  /_____/ /____/ \\____/   \\__/ \\____/   \\__, / \\__,_//_/ \\__/(_)\r\n\t\t\t                                                                              /_/                   "
                ];
                Console.Clear();
                nextText = !nextText;
                MoveText(gameTitle, gameStartingButtonsTexts, nextText);

                Thread.Sleep(500);
            }
        }

        static void KeyListener()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Spacebar)
                    {
                        exitFlag = true;
                        break;
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        Environment.Exit(0); // Exit program
                    }
                }
                Thread.Sleep(50);
            }
            Game.Execute();
        }

        // Moving text menu animation.
        static void MoveText(string title, string[] texts, bool next)
        {
            string currentTitle = title;
            string[] currentTexts = texts;
            
            if (next)
            {
                currentTitle = currentTitle.Insert(0, "\n");
                currentTexts[0] = currentTexts[0].Insert(0, "\n");
                currentTexts[1] = currentTexts[1].Insert(0, "\n");
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(currentTitle);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(currentTexts[0]);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(currentTexts[1]);
        }
    }
}
