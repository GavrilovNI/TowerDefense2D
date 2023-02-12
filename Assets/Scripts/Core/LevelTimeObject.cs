using UnityEngine;

namespace Game.Core
{
    public class LevelTimeObject : MonoBehaviour
    {
        public bool IsLevelStopped { get; private set; } = false;

        public void OnLevelStopped()
        {
            IsLevelStopped = true;
            GameObject.Destroy(gameObject);
        }

        private void OnDestroy()
        {
            IsLevelStopped = true;
        }
    }
}
