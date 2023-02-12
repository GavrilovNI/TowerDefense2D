using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class OnDestroyEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _destroying;

        private void OnDestroy()
        {
            _destroying.Invoke();
        }
    }
}
