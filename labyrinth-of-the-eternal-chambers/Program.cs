using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

namespace labyrinth_of_the_eternal_chambers
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        private const int STD_OUTPUT_HANDLE = -11;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CONSOLE_FONT_INFO_EX
        {
            public int cbSize;
            public int nFont;
            public short dwFontSizeX;
            public short dwFontSizeY;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        /// <summary>
        /// To maximize the console screen and to start the game.
        /// </summary>
        static void Main()
        {
            IntPtr consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            if (consoleHandle == IntPtr.Zero)
            {
                Console.WriteLine("Unable to get console handle.");
                return;
            }

            CONSOLE_FONT_INFO_EX fontInfo = new()
            {
                cbSize = Marshal.SizeOf<CONSOLE_FONT_INFO_EX>(),
                FaceName = "Consolas",
                dwFontSizeX = 0,
                dwFontSizeY = 12
            };

            SetCurrentConsoleFontEx(consoleHandle, false, ref fontInfo);

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
