using UnityEngine;
using Zenject;

namespace Service
{
    public interface ITimerService
    {
        void StartTimer();
        void StopTimer();
        float GetTime();
        void ResetTimer();
    }
    public class TimerService : ITimerService, ITickable
    {
        private float _time;
        private bool _isRunning;

        public float TimeValue => _time;

        public void Tick()
        {
            if (_isRunning)
            {
                _time += UnityEngine.Time.deltaTime;
            }
        }

        public void StartTimer() => _isRunning = true;
        public void StopTimer() => _isRunning = false;
        public float GetTime() => _time;
        public void ResetTimer() => _time = 0f;
    }
}
