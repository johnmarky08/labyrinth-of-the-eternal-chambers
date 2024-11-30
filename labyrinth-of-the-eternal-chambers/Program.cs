using NAudio.Wave;
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

        public static string currentBackgroundMusic = "bg1";
        private static readonly object lockObject = new();
        private static AudioFileReader backgroundMusic = new(@$"Sounds\{currentBackgroundMusic}.mp3");
        private static WaveOutEvent backgroundMusicOutput = new();
        private static bool musicPlaying = true;
        private static Thread bgMusicThread = new(PlayBackgroundMusic);
        /// <summary>
        /// To maximize the console screen, start the background music, and start the game.
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

            bgMusicThread.Start();

            Error.Handler(Menu.Start);
        }

        /// <summary>
        /// To play background music while running the program.
        /// </summary>
        private static void PlayBackgroundMusic()
        {
            try
            {
                lock (lockObject)
                {
                    backgroundMusicOutput.Init(backgroundMusic);
                }
                backgroundMusicOutput.Play();

                while (musicPlaying)
                {

                    if (backgroundMusicOutput.PlaybackState == PlaybackState.Stopped)
                    {
                        lock (lockObject)
                        {
                            backgroundMusic.Position = 0;
                            backgroundMusicOutput.Play();
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Background music error: {exception.Message}");
            }
            finally
            {
                backgroundMusic.Dispose();
                backgroundMusicOutput.Dispose();
            }
        }

        /// <summary>
        /// To play, pause or stop the background music.
        /// </summary>
        /// <param name="playPauseStop"> 0 - Stop, 1 - Play, 2 - Pause</param>
        public static void ToggleBackgroundMusic(int playPauseStop)
        {
            lock (lockObject)
            {
                switch (playPauseStop)
                {
                    // Stop
                    case 0:
                        {
                            musicPlaying = false;
                            backgroundMusicOutput.Stop();
                            bgMusicThread.Join(); // Ensure the thread has stopped
                            break;
                        }
                    // Play
                    case 1:
                        {
                            if (backgroundMusicOutput.PlaybackState != PlaybackState.Playing)
                            {
                                musicPlaying = true;
                                if (!bgMusicThread.IsAlive)
                                {
                                    bgMusicThread = new(PlayBackgroundMusic);
                                    bgMusicThread.Start();
                                }
                                else
                                {
                                    backgroundMusicOutput.Play();
                                }
                            }
                            break;
                        }
                    // Pause
                    case 2:
                        {
                            if (backgroundMusicOutput.PlaybackState == PlaybackState.Playing)
                            {
                                backgroundMusicOutput.Pause();
                            }
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// To change the current background music.
        /// </summary>
        /// <param name="fileName">File name of the new background music</param>
        public static void ChangeBackgroundMusic(string fileName)
        {
            lock (lockObject)
            {
                musicPlaying = false;
                if (bgMusicThread.IsAlive)
                {
                    backgroundMusicOutput.Stop();
                    bgMusicThread.Join();
                }

                backgroundMusic.Dispose();
                backgroundMusicOutput.Dispose();

                backgroundMusic = new(@$"Sounds\{fileName}.mp3");
                backgroundMusicOutput = new();

                musicPlaying = true;
                bgMusicThread = new(PlayBackgroundMusic);
                bgMusicThread.Start();
                currentBackgroundMusic = fileName;
            }
        }

        /// <summary>
        /// Paused the background music, then played a specific sound effect, then played the background music again.
        /// </summary>
        /// <param name="fileName">The file name of the sound effect you want to play.</param>
        public static void PlaySoundEffect(string fileName)
        {
            new Thread(() =>
            {
                try
                {
                    ToggleBackgroundMusic(2);

                    using AudioFileReader audioFile = new(@$"Sounds\{fileName}.mp3");
                    using WaveOutEvent outputDevice = new();
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100);
                    }

                    ToggleBackgroundMusic(1);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Sound Effect error: {exception.Message}");
                }
            }).Start();
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
