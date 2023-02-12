using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class OnDestroyEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _destroying;

        private bool _applicationQuitted = false;

        private void OnApplicationQuit()
        {
            _applicationQuitted = true;
        }

        private void OnDestroy()
        {
            if(_applicationQuitted == false)
                _destroying.Invoke();
        }
    }
}
