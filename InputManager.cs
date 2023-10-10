using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Tracks
{
    // TODO: Would be useful to have a way to check if multiple callsites
    // are checking for the same mouse / key input

    // TODO: Would be useful to get the list of keys, rather than calling for each
    internal class InputManager
    {
        private HashSet<Keys> KeyPressed { get; set; } = new HashSet<Keys>();
        private HashSet<Keys> KeyDown { get; set; } = new HashSet<Keys>();
        private HashSet<Keys> KeyUp { get; set; } = new HashSet<Keys>();

        private HashSet<MouseButton> MousePressed { get; set; } = new HashSet<MouseButton>();
        private HashSet<MouseButton> MouseDown { get; set; } = new HashSet<MouseButton>();
        private HashSet<MouseButton> MouseUp { get; set; } = new HashSet<MouseButton>();

        public Vector2 MousePosition { get; private set; }
        public Vector2 MouseMoveDelta { get; private set; }
        public Vector2 MouseWheelDelta { get; private set; }

        public void OnFrameStarted()
        {
            // These start fresh on every frame
            KeyDown.Clear();
            KeyUp.Clear();
            MouseDown.Clear();
            MouseUp.Clear();

            MouseMoveDelta = Vector2.Zero;
            MouseWheelDelta = Vector2.Zero;
        }

        public void OnWindowFocusChanged(object sender, FocusedChangedEventArgs e)
        {
            if (!e.IsFocused)
            {
                // Clear the held state when the user clicks away
                KeyPressed.Clear();
                MousePressed.Clear();
            }
        }

        public bool IsKeyPressed(Keys key)
        {
            return KeyPressed.Contains(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return KeyDown.Contains(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return KeyUp.Contains(key);
        }

        public bool IsMousePressed(MouseButton button)
        {
            return MousePressed.Contains(button);
        }

        public bool IsMouseDown(MouseButton button)
        {
            return MouseDown.Contains(button);
        }

        public bool IsMouseUp(MouseButton button)
        {
            return MouseUp.Contains(button);
        }

        internal void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!KeyPressed.Contains(e.Key))
            {
                KeyPressed.Add(e.Key);
                KeyDown.Add(e.Key);
            }
        }

        internal void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (KeyPressed.Contains(e.Key))
            {
                KeyPressed.Remove(e.Key);
                KeyUp.Add(e.Key);
            }
        }

        internal void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!MousePressed.Contains(e.Button))
            {
                MousePressed.Add(e.Button);
                MouseDown.Add(e.Button);
            }
        }

        internal void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MousePressed.Contains(e.Button))
            {
                MousePressed.Remove(e.Button);
                MouseUp.Add(e.Button);
            }
        }

        internal void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            MouseMoveDelta = e.Delta;
            MousePosition = e.Position;
        }

        internal void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseWheelDelta = e.Offset;
        }
    }
}
