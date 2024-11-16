using WindowsInput;
using WindowsInput.Native;

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
            simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);
            Thread.Sleep(1000);
            if (OperatingSystem.IsWindows())
                Console.SetBufferSize(Console.WindowWidth, Console.LargestWindowHeight);

            for (int i = 0; i < 3; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_MINUS);

            // Start game if presses space.
            Thread.Sleep(100);
            Thread keyListenerThread = new(KeyListener);
            keyListenerThread.Start();
            bool nextText = true;

            while (!exitFlag)
            {
                const string gameTitle = "\n\n\n\n\n\n\n\n\n\n\n\n\t                           ___             __       _______    ___  ___    _______     __      _____  ___    ___________    __    __           ______      _______                             \n\t                          |\"  |           /\"\"\\     |   _  \"\\  |\"  \\/\"  |  /\"      \\   |\" \\    (\\\"   \\|\"  \\  (\"     _   \")  /\" |  | \"\\         /    \" \\    /\"     \"|                            \n\t                          ||  |          /    \\    (. |_)  :)  \\   \\  /  |:        |  ||  |   |.\\\\   \\    |  )__/  \\\\__/  (:  (__)  :)       // ____  \\  (: ______)                            \n\t                          |:  |         /' /\\  \\   |:     \\/    \\\\  \\/   |_____/   )  |:  |   |: \\.   \\\\  |     \\\\_ /      \\/      \\/       /  /    ) :)  \\/    |                              \n\t                           \\  |___     //  __'  \\  (|  _  \\\\    /   /     //      /   |.  |   |.  \\    \\. |     |.  |      //  __  \\\\      (: (____/ //   // ___)                              \n\t                          ( \\_|:  \\   /   /  \\\\  \\ |: |_)  :)  /   /     |:  __   \\   /\\  |\\  |    \\    \\ |     \\:  |     (:  (  )  :)      \\        /   (:  (                                 \n\t                           \\_______) (___/    \\___)(_______/  |___/      |__|  \\___) (__\\_|_)  \\___|\\____\\)      \\__|      \\__|  |__/        \\\"_____/     \\__/                                 \n\t                                                                                                                                                                                               \n\t    _______   ___________    _______    _______    _____  ___         __       ___             ______     __    __         __       ___      ___  _______     _______    _______     ________  \n\t   /\"     \"| (\"     _   \")  /\"     \"|  /\"      \\  (\\\"   \\|\"  \\       /\"\"\\     |\"  |           /\" _  \"\\   /\" |  | \"\\       /\"\"\\     |\"  \\    /\"  ||   _  \"\\   /\"     \"|  /\"      \\   /\"       ) \n\t  (: ______)  )__/  \\\\__/  (: ______) |:        | |.\\\\   \\    |     /    \\    ||  |          (: ( \\___) (:  (__)  :)     /    \\     \\   \\  //   |(. |_)  :) (: ______) |:        | (:   \\___/  \n\t   \\/    |       \\\\_ /      \\/    |   |_____/   ) |: \\.   \\\\  |    /' /\\  \\   |:  |           \\/ \\       \\/      \\/     /' /\\  \\    /\\\\  \\/.    ||:     \\/   \\/    |   |_____/   )  \\___  \\    \n\t   // ___)_      |.  |      // ___)_   //      /  |.  \\    \\. |   //  __'  \\   \\  |___        //  \\ _    //  __  \\\\    //  __'  \\  |: \\.        |(|  _  \\\\   // ___)_   //      /    __/  \\\\   \n\t  (:      \"|     \\:  |     (:      \"| |:  __   \\  |    \\    \\ |  /   /  \\\\  \\ ( \\_|:  \\      (:   _) \\  (:  (  )  :)  /   /  \\\\  \\ |.  \\    /:  ||: |_)  :) (:      \"| |:  __   \\   /\" \\   :)  \n\t   \\_______)      \\__|      \\_______) |__|  \\___)  \\___|\\____\\) (___/    \\___) \\_______)      \\_______)  \\__|  |__/  (___/    \\___)|___|\\__/|___|(_______/   \\_______) |__|  \\___) (_______/   \n\t  \t                                                                                                                                                                                             ";
                string[] gameStartingButtonsTexts = [
                    "\n\n\n\n\n\n\t\t\t\t\t__       ____                             _____  ____   ___    ______ ______   __                  __                __  \r\n\t\t\t\t\t\\ \\     / __ \\ _____ ___   _____ _____   / ___/ / __ \\ /   |  / ____// ____/  / /_ ____     _____ / /_ ____ _ _____ / /_ \r\n\t\t\t\t\t \\ \\   / /_/ // ___// _ \\ / ___// ___/   \\__ \\ / /_/ // /| | / /    / __/    / __// __ \\   / ___// __// __ `// ___// __/ \r\n\t\t\t\t\t / /  / ____// /   /  __/(__  )(__  )   ___/ // ____// ___ |/ /___ / /___   / /_ / /_/ /  (__  )/ /_ / /_/ // /   / /_ _ \r\n\t\t\t\t\t/_/  /_/    /_/    \\___//____//____/   /____//_/    /_/  |_|\\____//_____/   \\__/ \\____/  /____/ \\__/ \\__,_//_/    \\__/(_)",
                    "\r\n\t\t\t\t\t__       ____                             ______ _____  ______   __                          _  __  \r\n\t\t\t\t\t\\ \\     / __ \\ _____ ___   _____ _____   / ____// ___/ / ____/  / /_ ____     ____ _ __  __ (_)/ /_ \r\n\t\t\t\t\t \\ \\   / /_/ // ___// _ \\ / ___// ___/  / __/   \\__ \\ / /      / __// __ \\   / __ `// / / // // __/ \r\n\t\t\t\t\t / /  / ____// /   /  __/(__  )(__  )  / /___  ___/ // /___   / /_ / /_/ /  / /_/ // /_/ // // /_ _ \r\n\t\t\t\t\t/_/  /_/    /_/    \\___//____//____/  /_____/ /____/ \\____/   \\__/ \\____/   \\__, / \\__,_//_/ \\__/(_)\r\n\t\t\t\t\t                                                                              /_/                   "
                ];
                Console.Clear();
                nextText = !nextText;

                MoveText(gameTitle, nextText);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(gameStartingButtonsTexts[0]);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(gameStartingButtonsTexts[1]);


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

            var simulator = new InputSimulator();

            for (int i = 0; i < 3; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_PLUS);

            Console.Clear();
            Game.Execute();
        }

        // Moving text menu animation.
        static void MoveText(string title, bool next)
        {
            string currentTitle = title;
            
            if (next)
            {
                currentTitle = currentTitle.Insert(0, "\n");
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(currentTitle);
        }
    }
}
