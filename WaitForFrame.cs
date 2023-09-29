using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class WaitForFrame : IEnumerator
    {
        public bool MoveNext() => false;

        public void Reset() { }

        public object Current => null;
    }
}
