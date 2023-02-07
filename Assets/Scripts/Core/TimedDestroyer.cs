using UnityEngine;

namespace Game.Core
{
    public class TimedDestroyer : MonoBehaviour
    {
        [SerializeField]
        private UnityTimer _timer = new(1, false);

        private void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        private void Awake()
        {
            _timer.Fired += Destroy;
        }

        private void Update()
        {
            _timer.Tick();
        }

        private void OnDestroy()
        {
            _timer.Fired -= Destroy;
        }
    }
}
