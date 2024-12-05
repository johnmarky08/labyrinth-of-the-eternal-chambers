using WindowsInput;
using WindowsInput.Native;

namespace labyrinth_of_the_eternal_chambers
{
    internal class Menu
    {
        private static bool exitFlag = false;
        private static bool directExit = false;
        public static bool isGuideOpen = false;
        public static bool isExitMenuOpen = false;

        /// <summary>
        /// To read when the user presses a key to know if they want to start the game or to exit the program.
        /// </summary>
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

                int startLength = Token.startKey.Split('\n')[0].Length;
                int exitLength = Token.exitKey1.Split('\n')[0].Length;
                Map.DrawToken(Console.CursorTop + 6, (Console.BufferWidth - startLength) / 2, ConsoleColor.Green, Token.startKey);
                Map.DrawToken(Console.CursorTop + 2, (Console.BufferWidth - (exitLength + (startLength - exitLength))) / 2, ConsoleColor.White, Token.exitKey1);

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// To know if the user wants to start, restart, or quit the game.
        /// </summary>
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

            Handler.toggleFont = 3;
            Handler.Error(Game.Execute);
        }

        /// <summary>
        /// Animation for the moving text. Uses the parameter bool next to identify whether to bring the text up or down the line.
        /// </summary>
        /// <param name="currentTitle">The text you wish to animate.</param>
        /// <param name="next">The boolean which represents whether to move up or down.</param>
        private static void MoveText(string currentTitle, bool next)
        {
            int length = currentTitle.Split('\n').Length;
            if (next) length++;

            Map.DrawToken((Console.BufferHeight - (length + 16)) / 2, (Console.BufferWidth - currentTitle.Split('\n')[0].Length) / 2, ConsoleColor.Red, currentTitle);
        }

        /// <summary>
        /// Exit Menu. To quit the program or return to where the user left off.
        /// </summary>
        /// <param name="isInGame">The boolean that represents wheter your in-game or not, default to false.</param>
        public static void Exit(bool isInGame = false)
        {
            string exitMenu = Token.exitMenu;
            int startY = (Console.BufferHeight - 17) / 2;
            int startX = (Console.BufferWidth - 109) / 2;

            Program.ToggleBackgroundMusic(2);
            isExitMenuOpen = true;
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
                            Program.ToggleBackgroundMusic(1);
                            isExitMenuOpen = false;

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

                                        if (adjustedY >= Map.currentMap.GetLength(0) || adjustedX >= Map.currentMap.GetLength(1))
                                        {
                                            Console.ResetColor();
                                            Console.Write(' ');
                                        }
                                        else if (Map.Check(Map.currentMap, adjustedY, adjustedX, Token.boundary))
                                        {
                                            Console.ResetColor(); // Boundary color
                                            if (Map.currentMapNumber == Map.maps.Count)
                                            {
                                                int currentY = Game.playerY - adjustedY;
                                                int currentX = Game.playerX - adjustedX;
                                                if (Map.shadowMaze.Contains((currentY, currentX))) Console.Write(Map.currentMap[y, x]);
                                                else Console.Write(' ');
                                            }
                                            else Console.Write(Map.currentMap[y, x]);
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

        /// <summary>
        /// To exit the program. Dynamically pressing ALT + F4.
        /// </summary>
        private static void End()
        {
            InputSimulator simulator = new();
            simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4);
        }

        /// <summary>
        /// To display the game over message, whether the user won or not.
        /// </summary>
        public static void GameOver()
        {
            Logic.StopTime();
            Database.UpdateHighScore(Game.playerName, Game.wrongDoors);
            Database.UpdateTime(Game.playerName, Logic.timeInSeconds);
            Program.ToggleBackgroundMusic(0);
            Console.Clear();
            Thread.Sleep(50);

            // Enlarge fontsize.
            Program.ToggleFontSize(5);

            // Display game over texts.
            Thread.Sleep(100);

            // First end message.
            if (!Game.gameOver && Game.won)
            {
                Map.DrawToken((Console.BufferHeight - Token.endMessage1.Split('\n').Length) / 2,
                    (Console.BufferWidth - Token.endMessage1.Split('\n')[0].Length) / 2,
                    ConsoleColor.DarkGray, Token.endMessage1);
                Thread.Sleep(4_000);
                Program.ChangeBackgroundMusic("bg1");
                Thread.Sleep(6_000); // Wait for 10 seconds (4000ms + 6000ms) before proceeding to next message
            }

            // Second end message.
            if (Program.currentBackgroundMusic != "bg1")
            {
                Thread.Sleep(1_500);
                Program.ChangeBackgroundMusic("bg1");
            }
            DisplayScores();

            // Third end message.
            DisplayLeaderboard();

            // Fourth end message.
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

                int playAgainLength = 121;
                int exitLength = Token.exitKey1.Split('\n')[0].Length;
                Map.DrawToken(Console.CursorTop + 6, (Console.BufferWidth - playAgainLength) / 2, ConsoleColor.Green, Token.startKey);
                Map.DrawToken(Console.CursorTop + 2, (Console.BufferWidth - (exitLength + (playAgainLength - exitLength))) / 2, ConsoleColor.White, Token.exitKey1);

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Display your scores attained in the game.
        /// </summary>
        private static void DisplayScores()
        {
            Console.Clear();
            int highScore = Game.wrongDoors;

            Map.DrawToken((Console.BufferHeight - 37) / 2, (Console.BufferWidth - Token.yourScore.Split('\n')[0].Length) / 2, ConsoleColor.Yellow, Token.yourScore);

            string[] highScoreLines = Token.highScore.Split('\n');

            string highScoreNumber = highScore < 10 ? string.Join('\n', MergeTokens(Token.whitespace.Split('\n'), Token.ConvertText(highScore.ToString()))) : Token.ConvertText(highScore.ToString());

            string scores = "\n\n" + Token.leastDoor;
            scores += '\n' + string.Join('\n', MergeTokens([.. MergeTokens(highScoreLines, highScoreNumber)], Token.doors));

            string timeScore = Token.time1;
            string mergedTime = "";

            foreach (char character in Logic.GetTime(Logic.timeInSeconds))
            {
                if (string.IsNullOrEmpty(mergedTime))
                {
                    mergedTime = Token.ConvertText(character.ToString());
                }
                else
                {
                    mergedTime = string.Join('\n',
                                                   MergeTokens(mergedTime.Split('\n'),
                                                                    Token.ConvertText(character.ToString())));
                }
            }

            timeScore += '\n' + string.Join('\n', MergeTokens(Token.time2.Split('\n'), mergedTime));
            scores += "\n\n" + timeScore;

            Map.DrawToken(Console.CursorTop + 3, (Console.BufferWidth - 112) / 2, ConsoleColor.White, scores);
            Thread.Sleep(10_000);
        }

        /// <summary>
        /// Displays the local leaderboard of the game.
        /// </summary>
        private static void DisplayLeaderboard()
        {
            Program.ToggleFontSize(-2);
            Console.Clear();
            Map.DrawToken((Console.BufferHeight - 59) / 2,
                    (Console.BufferWidth - Token.leaderboards.Split('\n')[0].Length) / 2,
                    ConsoleColor.Blue, Token.leaderboards);
            Map.DrawToken(Console.CursorTop + 5,
                    (Console.BufferWidth - Token.tableHeader.Split('\n')[0].Length) / 2,
                    ConsoleColor.White, Token.tableHeader + '\n');

            Dictionary<string, (int? HighScore, int? Time)> players = Database.GetAllPlayers();
            int rank = 1;
            foreach (KeyValuePair<string, (int? HighScore, int? Time)> player in players)
            {
                string playerLeaderboard = $"| {rank} | {player.Key} | {player.Value.HighScore} Doors | {Logic.GetTime(player.Value.Time ?? 0)} |";
                string leaderboardToken = "";
                foreach (char character in playerLeaderboard)
                {
                    if (string.IsNullOrEmpty(leaderboardToken))
                    {
                        leaderboardToken = Token.ConvertText(character.ToString());
                    }
                    else
                    {
                        leaderboardToken = string.Join('\n',
                                                       MergeTokens(leaderboardToken.Split('\n'),
                                                                        Token.ConvertText(character.ToString())));
                    }
                }

                Map.DrawToken(Console.CursorTop + 1,
                    (Console.BufferWidth - leaderboardToken.Split('\n')[0].Length) / 2,
                    ConsoleColor.White, leaderboardToken + '\n');
                if (++rank > 5) break;
            }

            Thread.Sleep(10_000);
            Program.ToggleFontSize(2);
        }

        /// <summary>
        /// To merge the ASCII message and the ASCII number since the ASCII number is dynamically provided.
        /// </summary>
        /// <param name="messageLines">The ASCII Art texts the you wish to merged from.</param>
        /// <param name="token">The ASCII Art you wish to merged with the messageLines.</param>
        /// <param name="additionalSpace">Represents whether you want an additional space to merged to, or not, default to false.</param>
        /// <returns>The merged ASCII Arts.</returns>
        public static List<string> MergeTokens(string[] messageLines, string token, bool additionalSpace = false)
        {
            // Determine the dimensions of the token
            string[] tokenLines = token.Split('\n');
            int tokenHeight = tokenLines.Length;

            // Prepare the result list
            List<string> mergedLines = [];

            // Merge each line of the message and token
            for (int i = 0; i < messageLines.Length; i++)
            {
                // Start with the message line
                string mergedLine = messageLines[i];

                // Add the corresponding token line if available
                if (i < tokenHeight)
                {
                    mergedLine += tokenLines[i];
                }

                if (additionalSpace)
                    mergedLine += "   ";

                mergedLines.Add(mergedLine);
            }

            return mergedLines;
        }

        /// <summary>
        /// To restart the game and restore all initial variables to their default values.
        /// </summary>
        private static void Restart()
        {
            // Set all variables to default
            Game.playerX = Game.defaultPlayerX;
            Game.playerY = Game.defaultPlayerY;
            Game.oldPlayerX = Game.playerX;
            Game.oldPlayerY = Game.playerY;
            Game.wrongDoors = 0;
            Game.roomNumber = 1;
            Game.won = false;
            Game.gameOver = false;
            Game.playerName = "";
            Token.player = Token.originalPlayer;
            Map.maps.Clear();
            Logic.currentPattern = "";
            Logic.pattern = "";
            Logic.timeInSeconds = 0;

            Handler.toggleFont = 2;
            Handler.Error(Game.Execute);
        }

        /// <summary>
        /// Guide Menu. To display the player's goal and the controllers of the game.
        /// </summary>
        public static void Guide()
        {
            int patternLength = (int)Configurations.PATTERN_LENGTH;
            string guideMenu = Token.guideMessage1;
            string patternToken = patternLength.ToString();
            guideMenu += string.Join('\n', MergeTokens(Token.guideMessage2.Split('\n'),
                                                             (patternLength < 10)
                                                             ? string.Join('\n', MergeTokens(
                                                                 Token.ConvertText(
                                                                    patternToken).Split('\n'),
                                                                    Token.ConvertText("%"),
                                                                    true))
                                                             : string.Join('\n', MergeTokens(
                                                                 Token.ConvertText(
                                                                    patternToken[0].ToString()).Split('\n'),
                                                                    Token.ConvertText(
                                                                    patternToken[1].ToString()),
                                                                    true))));
            guideMenu += Token.guideMessage3;
            guideMenu += Token.guideControls;
            guideMenu += Token.closeMenu;

            int menuHeight = 67, menuWidth = 147;
            int startY = (Console.BufferHeight - menuHeight) / 2;
            int startX = (Console.BufferWidth - menuWidth) / 2;

            isGuideOpen = true;

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


                                    if (adjustedY >= Map.currentMap.GetLength(0) || adjustedX >= Map.currentMap.GetLength(1))
                                    {
                                        Console.ResetColor();
                                        Console.Write(' ');
                                    }
                                    else if (Map.Check(Map.currentMap, adjustedY, adjustedX, Token.boundary))
                                    {
                                        Console.ResetColor(); // Boundary color
                                        if (Map.currentMapNumber == Map.maps.Count)
                                        {
                                            int currentY = Game.playerY - adjustedY;
                                            int currentX = Game.playerX - adjustedX;
                                            if (Map.shadowMaze.Contains((currentY, currentX))) Console.Write(Map.currentMap[y, x]);
                                            else Console.Write(' ');
                                        }
                                        else Console.Write(Map.currentMap[y, x]);
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
