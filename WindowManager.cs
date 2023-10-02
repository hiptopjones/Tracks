﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.ComponentModel;

namespace Tracks
{
    internal class WindowManager
    {
        public event EventHandler<KeyboardKeyEventArgs> KeyDown;
        public event EventHandler<KeyboardKeyEventArgs> KeyUp;
        public event EventHandler<FocusedChangedEventArgs> FocusChanged;

        public bool IsOpen => !GameWindow.IsExiting;

        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private GameWindow GameWindow { get; }

        public WindowManager(string windowName, int windowWidth, int windowHeight)
        {
            Width = windowWidth;
            Height = windowHeight;

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(Width, Height),
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

            GL.Enable(EnableCap.DepthTest);
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
            GameWindow.SwapBuffers();
        }

        public void Close()
        {
            GameWindow.Close();
        }

        private void OnResized(ResizeEventArgs e)
        {
            Width = e.Width;
            Height = e.Height;

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        private void OnFocusChanged(FocusedChangedEventArgs e)
        {
            FocusChanged?.Invoke(this, e);
        }

        private void OnKeyUp(KeyboardKeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        private void OnKeyDown(KeyboardKeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }

    }
}
