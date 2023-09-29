using NLog;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class DrawableSystem
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private List<DrawableComponent> DrawableComponents { get; } = new List<DrawableComponent>();

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            DrawableComponents.AddRange(newGameObjects
                .SelectMany(x => x.GetComponents<DrawableComponent>())
                .Where(x => x != null));
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(DrawableComponents, x => !x.Owner.IsAlive);
        }

        public void Draw(GraphicsManager graphicsManager)
        {
            foreach (DrawableComponent drawableComponent in DrawableComponents.OrderBy(x => x.SortingOrder))
            {
                if (drawableComponent.Owner.IsEnabled)
                {
                    drawableComponent.Draw(graphicsManager);
                }
            }
        }
    }
}
