using WindowsInput;
using WindowsInput.Native;

namespace labyrinth_of_the_eternal_chambers
{
    internal class Program
    {
        // Make the console full screen.
        static void Main()
        {
            InputSimulator simulator = new();

            simulator.Keyboard.KeyPress(VirtualKeyCode.F11);
            Thread.Sleep(50);
            ToggleFontSize(-3);

            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            Error.Handler(Menu.Start);
        }

        public static void ToggleFontSize(int strokes)
        {
            InputSimulator simulator = new();
            simulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            simulator.Mouse.VerticalScroll(strokes);
            simulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
        }
    }
}
