using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class CoreSystem
    {
        private List<GameObject> GameObjects { get; } = new List<GameObject>();

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            GameObjects.AddRange(newGameObjects);
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(GameObjects, x => !x.IsAlive);
        }

        public void Update(float deltaTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                if (gameObject.IsEnabled)
                {
                    gameObject.Update(deltaTime);
                }
            }
        }

        public void LateUpdate(float deltaTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                if (gameObject.IsEnabled)
                {
                    gameObject.LateUpdate(deltaTime);
                }
            }
        }
    }
}
