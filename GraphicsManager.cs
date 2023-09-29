using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SFML.Graphics;
using SFML.Window;
using MouseButtonEventArgs = SFML.Window.MouseButtonEventArgs;
using MouseMoveEventArgs = SFML.Window.MouseMoveEventArgs;

namespace Tracks
{
    internal class GraphicsManager
    {
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

        private VideoMode VideoMode { get; }

        private RenderWindow RenderWindow { get; }
        private GameWindow GameWindow { get; }

        public GraphicsManager(string windowName, uint windowWidth, uint windowHeight)
        {
            Width = windowWidth;
            Height = windowHeight;

            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                APIVersion = Version.Parse("3.3"),
                Profile = ContextProfile.Core
            };

            GameWindow = new GameWindow(gameWindowSettings, nativeWindowSettings);
            GameWindow.IsVisible = false;

            VideoMode videoMode = new VideoMode(Width, Height);
            RenderWindow = new RenderWindow(videoMode, windowName, Styles.Titlebar | Styles.Close);
            RenderWindow.SetVerticalSyncEnabled(true);

            RenderWindow.Closed += OnClosed;
            RenderWindow.Resized += OnResized;
            RenderWindow.LostFocus += OnLostFocus;
            RenderWindow.KeyPressed += OnKeyPressed;
            RenderWindow.KeyReleased += OnKeyReleased;
            RenderWindow.MouseMoved += OnMouseMoved;
            RenderWindow.MouseButtonPressed += OnMouseButtonPressed;
            RenderWindow.MouseButtonReleased += OnMouseButtonReleased;

            RenderWindow.SetActive(true);
        }

        public void ProcessEvents()
        {
            RenderWindow.DispatchEvents();
        }

        public void BeginDraw()
        {
            RenderWindow.Clear(GameSettings.WindowClearColor);
        }

        public void Draw(Drawable drawable)
        {
            RenderWindow.Draw(drawable);
        }

        public void EndDraw()
        {
            // Draw any debug graphics on top of everything before displaying the scene
            Debug.Draw(this);

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
            GL.Viewport(0, 0, (int)e.Width, (int)e.Height);
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

    }
}
