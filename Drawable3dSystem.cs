namespace Tracks
{
    internal class Drawable3dSystem
    {
        private List<Drawable3dComponent> DrawableComponents { get; } = new List<Drawable3dComponent>();

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            DrawableComponents.AddRange(newGameObjects
                .SelectMany(x => x.GetComponents<Drawable3dComponent>())
                .Where(x => x != null));
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(DrawableComponents, x => !x.Owner.IsAlive);
        }

        public void Draw()
        {
            foreach (Drawable3dComponent drawableComponent in DrawableComponents)
            {
                if (drawableComponent.Owner.IsEnabled)
                {
                    drawableComponent.Draw();
                }
            }
        }
    }
}
