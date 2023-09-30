using NLog;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class GraphicsSystem
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private List<GraphicsComponent> GraphicsComponents { get; } = new List<GraphicsComponent>();

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            GraphicsComponents.AddRange(newGameObjects
                .SelectMany(x => x.GetComponents<GraphicsComponent>())
                .Where(x => x != null));
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(GraphicsComponents, x => !x.Owner.IsAlive);
        }

        public void Render()
        {
            foreach (GraphicsComponent graphicsComponent in GraphicsComponents)
            {
                if (graphicsComponent.Owner.IsEnabled)
                {
                    graphicsComponent.Render();
                }
            }
        }
    }
}
