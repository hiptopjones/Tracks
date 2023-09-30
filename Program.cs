namespace Tracks
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            while (game.IsRunning)
            {
                game.StartFrame();
                game.ProcessEvents();
                game.Update();
                game.LateUpdate();
                game.Render();
            }
        }
    }
}