using System.Collections;

namespace Tracks
{
    // https://forums.tigsource.com/index.php?topic=32178.0
    internal class CoroutineManager
    {
        private List<IEnumerator> Coroutines { get; } = new List<IEnumerator>();

        public void StartCoroutine(IEnumerator coroutine)
        {
            Coroutines.Add(coroutine);
        }

        public void Update(float deltaTime)
        {
            // Avoiding "foreach" loop as we may mutate the list
            // Avoiding "for" loop as we're using swap and pop
            int i = 0;
            while (i < Coroutines.Count)
            {
                // A coroutine method can yield return another coroutine / enumerator, like WaitForTime
                // This descends into that, and doesn't tick the parent forward until the child is done
                IEnumerator coroutine = Coroutines[i].Current as IEnumerator;
                if (coroutine != null)
                {
                    if (coroutine.MoveNext())
                    {
                        i++;
                        continue;
                    }
                }

                // If the parent coroutine / enumerator is done, then remove it from our consideration
                if (!Coroutines[i].MoveNext())
                {
                    // Swap and pop to avoid shifting elements in an array list
                    int lastIndex = Coroutines.Count - 1;
                    Coroutines[i] = Coroutines[lastIndex];
                    Coroutines.RemoveAt(lastIndex);

                    continue;
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
