using System.Runtime.InteropServices;

namespace PersedawnyGames.TBE;

public class GameInitializer
{
    // P/Invoke declarations
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    // Constants
    private const int MF_BYCOMMAND = 0x00000000;
    private const int SC_MINIMIZE = 0xF020;
    private const int SC_MAXIMIZE = 0xF030;
    private const int SC_SIZE = 0xF000; // Resize
    private const int STD_INPUT_HANDLE = -10;
    private const uint ENABLE_QUICK_EDIT = 0x0040; // Quick Edit Mode
    private const uint ENABLE_EXTENDED_FLAGS = 0x0080;

    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOZORDER = 0x0004;

    // Structs
    private struct Rect
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    private struct Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    // Methods

    private static Size GetScreenSize() => new Size(GetSystemMetrics(0), GetSystemMetrics(1));

    private static Size GetWindowSize(IntPtr window)
    {
        if (!GetWindowRect(new HandleRef(null, window), out Rect rect))
            throw new Exception("Unable to get window rect!");

        return new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
    }

    private static void MoveWindowToCenter()
    {
        IntPtr window = GetConsoleWindow();
        if (window == IntPtr.Zero)
            throw new Exception("Couldn't find a window to center!");

        Size screenSize = GetScreenSize();
        Size windowSize = GetWindowSize(window);

        int x = (screenSize.Width - windowSize.Width) / 2;
        int y = (screenSize.Height - windowSize.Height) / 2;

        SetWindowPos(window, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
    }

    private static void DisableQuickEditMode()
    {
        IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
        if (consoleHandle == IntPtr.Zero)
            throw new InvalidOperationException("Unable to get console handle.");

        if (!GetConsoleMode(consoleHandle, out uint consoleMode))
            throw new InvalidOperationException("Unable to get console mode.");

        consoleMode &= ~ENABLE_QUICK_EDIT; // Remove Quick Edit Mode
        consoleMode |= ENABLE_EXTENDED_FLAGS; // Ensure Extended Flags are set

        if (!SetConsoleMode(consoleHandle, consoleMode))
            throw new InvalidOperationException("Unable to set console mode.");
    }

    public static void Initialize()
    {
        IntPtr handle = GetConsoleWindow();
        IntPtr sysMenu = GetSystemMenu(handle, false);

        if (handle != IntPtr.Zero)
        {
            // Remove minimize, maximize, and resize options
            DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
        }

        MoveWindowToCenter();
        DisableQuickEditMode();
    }
}
