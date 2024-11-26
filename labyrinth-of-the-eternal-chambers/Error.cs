namespace labyrinth_of_the_eternal_chambers
{
    internal class Error
    {
        public static void Handler(Action action)
        {
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
            }

        }
    }
}
