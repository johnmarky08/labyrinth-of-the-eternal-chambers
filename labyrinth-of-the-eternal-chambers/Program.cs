using WindowsInput;
using WindowsInput.Native;

namespace labyrinth_of_the_eternal_chambers
{
    internal class Program
    {
        /// <summary>
        /// To maximize the console screen and to start the game.
        /// </summary>
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

        /// <summary>
        /// Toggles font size depending on the strokes given.
        /// </summary>
        /// <param name="strokes">The number of clicks for scrolling, possitive means forward, negative means backwards.</param>
        public static void ToggleFontSize(int strokes)
        {
            InputSimulator simulator = new();
            for (int i = 0; i < Math.Abs(strokes); i++)
            {
                simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, ((strokes > 0) ? VirtualKeyCode.OEM_PLUS : VirtualKeyCode.OEM_MINUS));
            }
        }
    }
}
