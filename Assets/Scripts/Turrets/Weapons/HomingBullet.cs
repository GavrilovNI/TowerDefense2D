using Game.Core;
using Game.Extensions.Unity;
using UnityEngine;

namespace Game.Turrets.Weapons
{
    public class HomingBullet : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float _speed = 1f;
        [SerializeField, Min(0)]
        private float _damage = 30f;
        [SerializeField]
        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void FixedUpdate()
        {
            bool hasTarget = _target.IsNotNull();

            if (hasTarget)
            {
                Vector3 directionToTarget = _target.position - transform.position;
                directionToTarget.z = 0;
                directionToTarget.Normalize();
                transform.up = directionToTarget;
            }

            transform.position += _speed * Time.fixedDeltaTime * transform.up;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnHit(collision.transform);
        }

        private void OnHit(Transform target)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if(damageable != null)
                damageable.Damage(_damage);

            GameObject.Destroy(gameObject);
        }
    }
}
