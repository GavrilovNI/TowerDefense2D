using UnityEngine;

namespace Game.Core
{
    public class OnCollisionDestroyer : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
