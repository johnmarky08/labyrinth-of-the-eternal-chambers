namespace labyrinth_of_the_eternal_chambers
{
    internal class Handler
    {
        public static int toggleFont = 0;

        /// <summary>
        /// To handle unexpected errors while playing.
        /// </summary>
        /// <param name="action">The method that you want to try.</param>
        /// <param name="isToggleFont">If you wish to change the font size.</param>
        public static void Error(Action action, bool isToggleFont = false)
        {
            if (isToggleFont)
            {
                if (toggleFont == 3) Program.ToggleFontSize(3);
                else if (toggleFont == 2) Program.ToggleFontSize(2);
            }

            try
            {
                action();
            }
            catch (Exception error)
            {
                Program.ToggleFontSize(6);

                Thread.Sleep(100);
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occured: {error.Message}");

                Console.WriteLine("\n\nDeveloper Message: If you have tried to zoom in/out or resize the console, the game will not work properly. Please restart the program and try again. Thank you.\n\nExiting Program...");
                Environment.Exit(0);
            }

        }
    }
}
