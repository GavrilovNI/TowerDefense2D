using System;
using UnityEngine;

namespace Game.Core
{
    [Serializable]
    public class UnityTimer
    {
        public event Action Fired;

        [SerializeField, Min(0.01f)]
        private float _totalTime;
        [SerializeField]
        private bool _repeating;

        private float _timePassed = 0f;

        public UnityTimer() : this(1f, false)
        {

        }

        public UnityTimer(float time, bool repeating)
        {
            if(time <= 0)
                throw new ArgumentOutOfRangeException(nameof(time));

            _totalTime = time;
            _repeating = repeating;
        }

        public void TickFixed()
        {
            Tick(Time.fixedDeltaTime);
        }

        public void Tick()
        {
            Tick(Time.deltaTime);
        }

        private void Tick(float time)
        {
            _timePassed += time;

            int timesToFire = GetTimesToFire();

            if(timesToFire > 0)
            {
                if(Fired != null)
                {
                    for(int i = 0; i < timesToFire; ++i)
                        Fired.Invoke();
                }

                _timePassed %= _totalTime;
            }
        }

        private int GetTimesToFire()
        {
            if(_repeating)
                return Mathf.FloorToInt(_timePassed / _totalTime);

            return _timePassed > _totalTime ? 1 : 0;
        }
    }
}
