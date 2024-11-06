namespace final_project
{
    internal class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n");
                Console.WriteLine("██████╗░░█████╗░███╗░░░███╗██████╗░███████╗██████╗░███╗░░░███╗░█████╗░███╗░░██╗");
                Console.WriteLine("██╔══██╗██╔══██╗████╗░████║██╔══██╗██╔════╝██╔══██╗████╗░████║██╔══██╗████╗░██║");
                Console.WriteLine("██████╦╝██║░░██║██╔████╔██║██████╦╝█████╗░░██████╔╝██╔████╔██║███████║██╔██╗██║");
                Console.WriteLine("██╔══██╗██║░░██║██║╚██╔╝██║██╔══██╗██╔══╝░░██╔══██╗██║╚██╔╝██║██╔══██║██║╚████║");
                Console.WriteLine("██████╦╝╚█████╔╝██║░╚═╝░██║██████╦╝███████╗██║░░██║██║░╚═╝░██║██║░░██║██║░╚███║");
                Console.WriteLine("╚═════╝░░╚════╝░╚═╝░░░░░╚═╝╚═════╝░╚══════╝╚═╝░░╚═╝╚═╝░░░░░╚═╝╚═╝░░╚═╝╚═╝░░╚══╝");
                Console.WriteLine("\n\n > Press \"SPACE\" to start.");
                Console.WriteLine(" > Press \"ESCAPE\" to exit.");
                
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Spacebar)
                {
                    break;
                }
                else if (key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n\nExiting the game...");
                    return;
                }
            }
            Game game = new();
            Game.Execute();
        }
    }
}
