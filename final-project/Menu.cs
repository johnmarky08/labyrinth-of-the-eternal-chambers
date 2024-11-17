using WindowsInput;
using WindowsInput.Native;

namespace final_project
{
    internal class Menu
    {
        private static readonly InputSimulator simulator = new();
        private static bool exitFlag = false;
        private static bool directExit = false;

        public static void Start()
        {
            Console.Clear();

            // Start game if presses space.
            Thread.Sleep(100);
            Thread keyListenerThread = new(KeyListener);
            keyListenerThread.Start();
            bool nextText = true;

            while (!exitFlag)
            {
                Console.Clear();
                nextText = !nextText;

                MoveText(Token.gameTitle, nextText);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Token.startKey);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Token.exitKey1);


                Thread.Sleep(500);
            }
        }

        private static void KeyListener()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Spacebar)
                    {
                        exitFlag = true;
                        if (directExit)
                        {
                            Restart();
                            return;
                        }
                        break;
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        exitFlag = true;
                        if (!directExit) Exit(Console.BufferHeight, Console.BufferWidth); // Exit program
                        else End();
                        return;
                    }
                }
                Thread.Sleep(50);
            }

            for (int i = 0; i < 3; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_PLUS);

            Console.Clear();
            Game.Execute();
        }

        // Moving text menu animation.
        private static void MoveText(string title, bool next)
        {
            string currentTitle = title;

            if (next)
            {
                currentTitle = currentTitle.Insert(0, "\n");
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(currentTitle);
        }
        public static void Exit(int currentY, int currentX, bool isInGame = false)
        {
            string exitMenu = Token.exitMenu;
            int startY = (currentY - 17) / 2;
            int startX = (currentX - 109) / 2;

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Map.DrawToken(startY, startX, ConsoleColor.White, exitMenu, null);

            bool exit = false;
            while (!exit)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        {
                            End();
                            exit = true;
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            Console.BackgroundColor = ConsoleColor.Black;

                            if (!isInGame)
                            {
                                exitFlag = false;
                                Start();
                            }
                            else
                            {
                                for (int y = startY; y < (startY + 17); y++)
                                {
                                    for (int x = startX; x < (startX + 109); x++)
                                    {
                                        Console.SetCursorPosition(x, y);

                                        // Adjust coordinates to match the 7x7 grid
                                        int adjustedY = y / Map.blockSize * Map.blockSize;
                                        int adjustedX = x / Map.blockSize * Map.blockSize;

                                        if (Map.Check(Map.currentMap, adjustedY, adjustedX, Token.boundary))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Gray; // Boundary color
                                            Console.Write(Map.currentMap[y, x]);
                                        }
                                        else if (Map.Check(Map.currentMap, adjustedY, adjustedX, Token.player))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green; // Player color
                                            Console.Write(Map.currentMap[y, x]);
                                        }
                                        else
                                        {
                                            Console.ResetColor(); // Default color for other spaces
                                            Console.Write(Map.currentMap[y, x]);
                                        }
                                    }
                                }
                            }

                            exit = true;
                            break;
                        }
                }
            }
        }

        private static void End()
        {
            simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4);
        }

        public static void Won()
        {
            // Game Over
            Console.Clear();
            Thread.Sleep(50);

            // Enlarge buffersize.
            var simulator = new InputSimulator();
            for (int i = 0; i < 5; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_PLUS);

            // Display game over texts.
            Thread.Sleep(100);

            // First end message.
            Console.SetCursorPosition(0, 5);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(Token.endMessage1);
            Console.ReadKey(true);

            // Second end message.
            directExit = true;
            Thread keyListenerThread = new(KeyListener);
            keyListenerThread.Start();
            bool nextText = true;
            exitFlag = false;

            simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_MINUS);
            Thread.Sleep(100);
            Console.Clear();

            while (!exitFlag)
            {
                string endMessage = Token.endMessage2;

                string score = Token.wrongDoors2;
                string[] scoreLines = score.Split('\n');
                string number = Token.ConvertNumber(Game.wrongDoors);

                List<string> numberLines = Enumerable.Range(0, (int)Math.Ceiling(number.Length / 8.0))
                    .Select(i => number.Substring(i * 8, Math.Min(8, number.Length - i * 8)))
                    .ToList();

                int totalNumberLines = numberLines.Count;
                int totalScoreLines = scoreLines.Length;

                List<string> mergedLines = [];

                for (int i = 0; i < totalScoreLines; i++)
                {
                    string mergedLine = scoreLines[i];

                    for (int j = 0; j < totalNumberLines; j++)
                    {
                        if (i + j * 6 < totalNumberLines)
                        {
                            mergedLine += numberLines[i + j * 6];
                        }
                    }
                    mergedLines.Add(mergedLine);
                }

                endMessage += string.Join('\n', mergedLines);

                string playMessage = Game.wrongDoors switch
                {
                    <= 5 => Token.genius,
                    <= 8 => Token.smart,
                    <= 10 => Token.good,
                    _ => Token.betterLuckNextTime,
                };

                endMessage += '\n' + playMessage;
                Console.Clear();
                nextText = !nextText;

                MoveText(endMessage, nextText);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Token.playAgain);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Token.exitKey2);

                Thread.Sleep(500);
            }
        }

        private static void Restart()
        {
            for (int i = 0; i < 2; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_PLUS);

            // Set all variables to default
            Game.playerX = Game.defaultPlayerX;
            Game.playerY = Game.defaultPlayerY;
            Game.oldPlayerX = Game.playerX;
            Game.oldPlayerY = Game.playerY;
            Game.wrongDoors = 0;
            Game.roomNumber = 1;
            Game.gameOver = false;
            Map.maps.Clear();
            Logic.currentPattern = "";
            Logic.pattern = "";

            Console.Clear();
            Game.Execute();
        }
    }
}
