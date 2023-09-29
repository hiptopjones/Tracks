using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    // https://blog.oliverbooth.dev/2021/04/27/how-do-unitys-coroutines-actually-work/
    internal class WaitForTime : IEnumerator
    {
        private static TimeManager TimeManager { get; set; }

        private readonly float _endTime;

        static WaitForTime()
        {
            // NOTE: Assumes that WaitForTime will not be referenced until after the game is initialized
            TimeManager = ServiceLocator.Instance.GetService<TimeManager>();
        }

        public WaitForTime(TimeSpan waitTime)
        {
            _endTime = TimeManager.TotalTime + (float)waitTime.TotalSeconds;
        }

        public bool MoveNext() => TimeManager.TotalTime < _endTime;

        public void Reset() { }

        public object Current => null;
    }
}
