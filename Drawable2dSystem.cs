using NLog;
namespace Tracks
{
    internal class Drawable2dSystem
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private List<Drawable2dComponent> DrawableComponents { get; } = new List<Drawable2dComponent>();

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            DrawableComponents.AddRange(newGameObjects
                .SelectMany(x => x.GetComponents<Drawable2dComponent>())
                .Where(x => x != null));
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(DrawableComponents, x => !x.Owner.IsAlive);
        }

        public void Draw()
        {
            foreach (Drawable2dComponent drawableComponent in DrawableComponents.OrderBy(x => x.SortingOrder))
            {
                if (drawableComponent.Owner.IsEnabled)
                {
                    drawableComponent.Draw();
                }
            }
        }
    }
}
