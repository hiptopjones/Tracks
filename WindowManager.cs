using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.ComponentModel;

namespace Tracks
{
    internal class WindowManager
    {
        public event EventHandler<FocusedChangedEventArgs> FocusChanged;
        public event EventHandler<ResizeEventArgs> Resized;
        public event EventHandler<KeyboardKeyEventArgs> KeyDown;
        public event EventHandler<KeyboardKeyEventArgs> KeyUp;
        public event EventHandler<MouseMoveEventArgs> MouseMove;
        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseWheelEventArgs> MouseWheel;

        public bool IsOpen => !GameWindow.IsExiting;

        public string Name { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        private GameWindow GameWindow { get; }

        public WindowManager(string windowName, int windowWidth, int windowHeight)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(WindowWidth, WindowHeight),
                Title = windowName,
                Profile = ContextProfile.Any,
                Flags = ContextFlags.ForwardCompatible,
                DepthBits = 24
            };

            GameWindow = new GameWindow(gameWindowSettings, nativeWindowSettings);

            GameWindow.FocusedChanged += OnFocusChanged;
            GameWindow.Resize += OnResized;
            GameWindow.KeyDown += OnKeyDown;
            GameWindow.KeyUp += OnKeyUp;
            GameWindow.MouseMove += OnMouseMove;
            GameWindow.MouseDown += OnMouseDown;
            GameWindow.MouseUp += OnMouseUp;
            GameWindow.MouseWheel += OnMouseWheel;

            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        public void ProcessEvents()
        {
            GameWindow.ProcessEvents(0);
        }

        public void BeginDraw()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void EndDraw()
        {
            // Draw any debug graphics on top of everything before displaying the scene
            Debug.Draw();

            GameWindow.SwapBuffers();
        }

        public void Close()
        {
            GameWindow.Close();
        }

        private void OnResized(ResizeEventArgs e)
        {
            WindowWidth = e.Width;
            WindowHeight = e.Height;

            GL.Viewport(0, 0, e.Width, e.Height);

            Resized?.Invoke(this, e);
        }

        private void OnFocusChanged(FocusedChangedEventArgs e)
        {
            FocusChanged?.Invoke(this, e);
        }

        private void OnKeyDown(KeyboardKeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }

        private void OnKeyUp(KeyboardKeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        private void OnMouseMove(MouseMoveEventArgs e)
        {
            MouseMove?.Invoke(this, e);
        }

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        private void OnMouseUp(MouseButtonEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            MouseWheel?.Invoke(this, e);
        }
    }
}
