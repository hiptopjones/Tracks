using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Tracks
{
    internal class InputManager
    {
        private HashSet<Keys> KeyPressed { get; set; } = new HashSet<Keys>();
        private HashSet<Keys> KeyDown { get; set; } = new HashSet<Keys>();
        private HashSet<Keys> KeyUp { get; set; } = new HashSet<Keys>();

        public void OnFrameStarted()
        {
            // These start fresh on every frame
            KeyUp.Clear();
            KeyDown.Clear();
        }

        public void OnWindowFocusChanged(object sender, FocusedChangedEventArgs e)
        {
            if (!e.IsFocused)
            {
                KeyPressed.Clear();
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

        public void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!KeyPressed.Contains(e.Key))
            {
                KeyPressed.Add(e.Key);
                KeyDown.Add(e.Key);
            }
        }

        public void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (KeyPressed.Contains(e.Key))
            {
                KeyPressed.Remove(e.Key);
                KeyUp.Add(e.Key);
            }
        }
    }
}
