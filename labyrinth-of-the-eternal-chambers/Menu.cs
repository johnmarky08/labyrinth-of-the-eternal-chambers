﻿using WindowsInput;
using WindowsInput.Native;

namespace labyrinth_of_the_eternal_chambers
{
    internal class Menu
    {
        private static bool exitFlag = false;
        private static bool directExit = false;

        public static void Start()
        {
            Console.Clear();
            Console.CursorVisible = false;

            // Start game if presses space.
            Thread.Sleep(100);
            Thread keyListenerThread = new(ExitMenuKey);
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

        private static void ExitMenuKey()
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
                        if (!directExit) Exit(); // Exit program
                        else End();
                        return;
                    }
                }
                Thread.Sleep(50);
            }

            Program.ToggleFontSize(3);

            Console.Clear();
            Error.Handler(Game.Execute);
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
        public static void Exit(bool isInGame = false)
        {
            string exitMenu = Token.exitMenu;
            int startY = (Console.BufferHeight - 17) / 2;
            int startX = (Console.BufferWidth - 109) / 2;

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Map.DrawToken(startY, startX, ConsoleColor.White, exitMenu);

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
                                            Console.ResetColor(); // Boundary color
                                            Console.Write(Map.currentMap[y, x]);
                                        }
                                        else if (Map.Check(Map.currentMap, adjustedY, adjustedX, Token.player))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green; // Player color
                                            Console.Write(Map.currentMap[y, x]);
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkGray; // Default color for other spaces
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
            InputSimulator simulator = new();
            simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4);
        }

        public static void GameOver()
        {
            Console.Clear();
            Thread.Sleep(50);

            // Enlarge fontsize.
            Program.ToggleFontSize(5);

            // Display game over texts.
            Thread.Sleep(100);

            // First end message.
            if (!Game.gameOver && Game.won)
            {
                Console.SetCursorPosition(0, 5);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(Token.endMessage1);
                Thread.Sleep(10_000); // Wait for 10 seconds before proceeding to next message
            }

            // Second end message.
            directExit = true;
            Thread keyListenerThread = new(ExitMenuKey);
            keyListenerThread.Start();
            bool nextText = true;
            exitFlag = false;

            Program.ToggleFontSize(-1);
            Thread.Sleep(100);
            Console.Clear();

            while (!exitFlag)
            {
                string endMessage = Game.gameOver ? Token.gameOver : Token.endMessage2;

                if (!Game.gameOver && Game.won)
                {
                    string score = Token.wrongDoors2;
                    string[] scoreLines = score.Split('\n');
                    string number = Token.ConvertNumber(Game.wrongDoors.ToString());
                    List<string> mergedLines = MergedNumberToken(scoreLines, number);

                    endMessage += string.Join('\n', mergedLines);
                }

                string playMessage = Convert.ToDouble(Game.wrongDoors) switch
                {
                    < ((int)Configurations.MAX_GUESS * 0.25) => Token.genius,
                    < ((int)Configurations.MAX_GUESS * 0.5) => Token.smart,
                    < ((int)Configurations.MAX_GUESS * 0.75) => Token.good,
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

        private static List<string> MergedNumberToken(string[] messageLines, string token, bool additionalSpace = false)
        {
            List<string> numberLines = Enumerable.Range(0, (int)Math.Ceiling(token.Length / 8.0))
                                    .Select(i => token.Substring(i * 8, Math.Min(8, token.Length - i * 8)))
                                    .ToList();

            int totalNumberLines = numberLines.Count;
            int totalScoreLines = messageLines.Length;

            List<string> mergedLines = [];

            for (int i = 0; i < totalScoreLines; i++)
            {
                string mergedLine = messageLines[i];

                for (int j = 0; j < totalNumberLines; j++)
                {
                    if (i + j * 6 < totalNumberLines)
                    {
                        mergedLine += numberLines[i + j * 6];
                    }
                }

                if (additionalSpace)
                    mergedLine += "   ";

                mergedLines.Add(mergedLine);
            }

            return mergedLines;
        }

        private static void Restart()
        {
            Program.ToggleFontSize(2);

            // Set all variables to default
            Game.playerX = Game.defaultPlayerX;
            Game.playerY = Game.defaultPlayerY;
            Game.oldPlayerX = Game.playerX;
            Game.oldPlayerY = Game.playerY;
            Game.wrongDoors = 0;
            Game.roomNumber = 1;
            Game.won = false;
            Game.gameOver = false;
            Map.maps.Clear();
            Logic.currentPattern = "";
            Logic.pattern = "";

            Console.Clear();
            Game.Execute();
        }

        public static void Guide()
        {
            int patternLength = (int)Configurations.PATTERN_LENGTH;
            string guideMenu = Token.guideMessage1;
            guideMenu += string.Join('\n', MergedNumberToken(Token.guideMessage2.Split('\n'),
                                                             Token.ConvertNumber((patternLength < 10)
                                                                ? (patternLength.ToString() + '%')
                                                                : patternLength.ToString()),
                                                             true));
            guideMenu += Token.guideMessage3;
            guideMenu += Token.guideControls;
            guideMenu += Token.closeMenu;

            int menuHeight = 67, menuWidth = 147;
            int startY = (Console.BufferHeight - menuHeight) / 2;
            int startX = (Console.BufferWidth - menuWidth) / 2;

            bool isGuideOpen = true;

            Console.BackgroundColor = ConsoleColor.Gray;
            Map.DrawToken(startY, startX, ConsoleColor.Black, guideMenu);

            while (isGuideOpen)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            for (int y = startY; y < (startY + menuHeight); y++)
                            {
                                for (int x = startX; x < (startX + menuWidth); x++)
                                {
                                    Console.SetCursorPosition(x, y);

                                    // Adjust coordinates to match the 7x7 grid
                                    int adjustedY = y / Map.blockSize * Map.blockSize;
                                    int adjustedX = x / Map.blockSize * Map.blockSize;

                                    if (Map.Check(Map.currentMap, adjustedY, adjustedX, Token.boundary))
                                    {
                                        Console.ResetColor(); // Boundary color
                                        Console.Write(Map.currentMap[y, x]);
                                    }
                                    else if (Map.Check(Map.currentMap, adjustedY, adjustedX, Token.player))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green; // Player color
                                        Console.Write(Map.currentMap[y, x]);
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray; // Default color for other spaces
                                        Console.Write(Map.currentMap[y, x]);
                                    }
                                }
                            }

                            Map.DrawGuides();
                            isGuideOpen = false;
                            break;
                        }
                }
            }
        }
    }
}
