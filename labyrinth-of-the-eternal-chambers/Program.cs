using WindowsInput;
using WindowsInput.Native;

namespace labyrinth_of_the_eternal_chambers
{
    internal class Program
    {
        static void Main()
        {
            // Make the console full screen.
            Console.CursorVisible = false;
            InputSimulator simulator = new();
            Thread.Sleep(500);
            simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);
            Thread.Sleep(500);
            if (OperatingSystem.IsWindows())
                Console.SetBufferSize(Console.WindowWidth, Console.LargestWindowHeight);

            for (int i = 0; i < 3; i++)
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_MINUS);

            Error.Handler(Menu.Start);
        }
    }
}
