using NLog;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SFML.Graphics;
using SFML.Window;
using System.Runtime.InteropServices;
using MouseButtonEventArgs = SFML.Window.MouseButtonEventArgs;
using MouseMoveEventArgs = SFML.Window.MouseMoveEventArgs;

namespace Tracks
{
    internal class WindowManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<KeyEventArgs> KeyReleased;
        public event EventHandler<MouseMoveEventArgs> MouseMoved;
        public event EventHandler<MouseButtonEventArgs> MouseButtonPressed;
        public event EventHandler<MouseButtonEventArgs> MouseButtonReleased;

        public event EventHandler WindowLostFocus;

        public bool IsOpen => RenderWindow.IsOpen;

        public string Name { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        private RenderWindow RenderWindow { get; }
        private GameWindow GameWindow { get; }

        private static DebugProc DebugMessageDelegate = OnDebugMessage;

        public WindowManager(string windowName, uint windowWidth, uint windowHeight)
        {
            Width = windowWidth;
            Height = windowHeight;

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                APIVersion = Version.Parse("3.3"),
                Profile = ContextProfile.Any,
                Flags = ContextFlags.Debug
            };

            GameWindow = new GameWindow(gameWindowSettings, nativeWindowSettings);
            GameWindow.IsVisible = false;

            VideoMode videoMode = new VideoMode(Width, Height);
            ContextSettings contextSettings = new ContextSettings
            {
                DepthBits = 24,   
            };

            RenderWindow = new RenderWindow(videoMode, windowName, Styles.Default, contextSettings);

            RenderWindow.Closed += OnClosed;
            RenderWindow.Resized += OnResized;
            RenderWindow.LostFocus += OnLostFocus;
            RenderWindow.KeyPressed += OnKeyPressed;
            RenderWindow.KeyReleased += OnKeyReleased;
            RenderWindow.MouseMoved += OnMouseMoved;
            RenderWindow.MouseButtonPressed += OnMouseButtonPressed;
            RenderWindow.MouseButtonReleased += OnMouseButtonReleased;

            RenderWindow.SetActive(true);

            GL.DebugMessageCallback(DebugMessageDelegate, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);

            GL.Enable(EnableCap.DepthTest);
        }

        public void ProcessEvents()
        {
            RenderWindow.DispatchEvents();
        }

        public void BeginDraw()
        {
            RenderWindow.Clear(GameSettings.WindowClearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Draw(Drawable drawable)
        {
            RenderWindow.PushGLStates();
            RenderWindow.ResetGLStates();

            RenderWindow.Draw(drawable);
            
            RenderWindow.PopGLStates();
        }

        public void EndDraw()
        {
            // Draw any debug graphics on top of everything before displaying the scene
            Debug.Draw();

            RenderWindow.Display();
        }

        public void Close()
        {
            RenderWindow.Close();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            RenderWindow.Close();
        }

        private void OnResized(object sender, SizeEventArgs e)
        {
            RenderWindow.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        private void OnLostFocus(object sender, EventArgs e)
        {
            WindowLostFocus?.Invoke(sender, e);
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            KeyPressed?.Invoke(sender, e);
        }

        private void OnKeyReleased(object sender, KeyEventArgs e)
        {
            KeyReleased?.Invoke(sender, e);
        }

        private void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            MouseMoved?.Invoke(sender, e);
        }

        private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(sender, e);
        }

        private void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            MouseButtonReleased?.Invoke(sender, e);
        }

        private static void OnDebugMessage(
            DebugSource source,     // Source of the debugging message.
            DebugType type,         // Type of the debugging message.
            int id,                 // ID associated with the message.
            DebugSeverity severity, // Severity of the message.
            int length,             // Length of the string in pMessage.
            IntPtr pMessage,        // Pointer to message string.
            IntPtr pUserParam)      // The pointer you gave to OpenGL, explained later.
        {
            // In order to access the string pointed to by pMessage, you can use Marshal
            // class to copy its contents to a C# string without unsafe code. You can
            // also use the new function Marshal.PtrToStringUTF8 since .NET Core 1.1.
            string message = Marshal.PtrToStringAnsi(pMessage, length);

            Logger.Error("[{0} source={1} type={2} id={3}] {4}", severity, source, type, id, message);
        }
    }
}
