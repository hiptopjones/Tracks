using SFML.System;
using SFML.Window;

namespace Tracks
{
    internal class InputManager
    {
        private HashSet<Keyboard.Key> KeyPressed { get; set; } = new HashSet<Keyboard.Key>();
        private HashSet<Keyboard.Key> KeyDown { get; set; } = new HashSet<Keyboard.Key>();
        private HashSet<Keyboard.Key> KeyUp { get; set; } = new HashSet<Keyboard.Key>();

        private HashSet<Mouse.Button> MouseButtonPressed { get; set; } = new HashSet<Mouse.Button>();
        private HashSet<Mouse.Button> MouseButtonDown { get; set; } = new HashSet<Mouse.Button>();
        private HashSet<Mouse.Button> MouseButtonUp { get; set; } = new HashSet<Mouse.Button>();

        public Vector2f MousePosition { get; private set; }
 
        public void OnFrameStarted()
        {
            // These start fresh on every frame
            KeyUp.Clear();
            KeyDown.Clear();

            MouseButtonUp.Clear();
            MouseButtonDown.Clear();
        }

        public void OnWindowLostFocus(object sender, EventArgs e)
        {
            KeyPressed.Clear();
            MouseButtonPressed.Clear();
        }

        public bool IsKeyPressed(Keyboard.Key keycode)
        {
            return KeyPressed.Contains(keycode);
        }

        public bool IsKeyDown(Keyboard.Key keycode)
        {
            return KeyDown.Contains(keycode);
        }

        public bool IsKeyUp(Keyboard.Key keycode)
        {
            return KeyUp.Contains(keycode);
        }

        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (!KeyPressed.Contains(e.Code))
            {
                KeyPressed.Add(e.Code);
                KeyDown.Add(e.Code);
            }
        }

        public void OnKeyReleased(object sender, KeyEventArgs e)
        {
            if (KeyPressed.Contains(e.Code))
            {
                KeyPressed.Remove(e.Code);
                KeyUp.Add(e.Code);
            }
        }

        public void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            MousePosition = new Vector2f(e.X, e.Y);
        }

        public bool IsMouseButtonPressed(Mouse.Button button)
        {
            return MouseButtonPressed.Contains(button);
        }

        public bool IsMouseButtonDown(Mouse.Button button)
        {
            return MouseButtonDown.Contains(button);
        }

        public bool IsMouseButtonUp(Mouse.Button button)
        {
            return MouseButtonUp.Contains(button);
        }

        public void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!MouseButtonPressed.Contains(e.Button))
            {
                MouseButtonPressed.Add(e.Button);
                MouseButtonDown.Add(e.Button);
            }
        }

        public void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (MouseButtonPressed.Contains(e.Button))
            {
                MouseButtonPressed.Remove(e.Button);
                MouseButtonUp.Add(e.Button);
            }
        }
    }
}
