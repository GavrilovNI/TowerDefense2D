using Game.Extensions.Unity;
using UnityEngine;

namespace Game.Core
{
    public class TargetFollower : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float _speed = 1f;
        [SerializeField]
        private Transform? _target;

        public void SetTarget(Transform? target)
        {
            _target = target;
        }

        protected virtual void FixedUpdate()
        {
            bool hasTarget = _target.IsNotNull();

            if(hasTarget)
            {
                Vector3 directionToTarget = _target.position - transform.position;
                directionToTarget.z = 0;
                directionToTarget.Normalize();
                transform.up = directionToTarget;
            }

            transform.position += _speed * Time.fixedDeltaTime * transform.up;
        }
    }
}
