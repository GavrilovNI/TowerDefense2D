using Game.Core;
using Game.Extensions.Unity;
using UnityEngine;

namespace Game.Turrets.Weapons
{
    public class Laser : MonoBehaviour
    {
        [SerializeField]
        private Transform _beam;
        [SerializeField, Min(0)]
        private float _damagePerSecond = 1f;
        [SerializeField]
        private Transform? _target;

        private IDamageable _targetDamageable;

        public void SetTarget(Transform? target)
        {
            _target = target;

            if(_target.IsNotNull())
                _targetDamageable = _target.GetComponent<IDamageable>();

            OnTargetUpdated();
        }


        private void OnTargetUpdated()
        {
            if(_target == null)
            {
                DisableBeam();
            }
            else
            {
                UpdateTransformTowardsTarget();
                EnableBeam();
            }
        }

        private void Awake()
        {
            SetTarget(_target);
        }

        private void DisableBeam()
        {
            _beam.gameObject.SetActive(false);
        }

        private void EnableBeam()
        {
            _beam.gameObject.SetActive(true);
        }

        private void UpdateTransformTowardsTarget()
        {
            float length = Vector2.Distance(transform.position, _target.position);

            var scale = transform.lossyScale;
            scale.y = length;
            transform.SetGlobalScale(scale);

            Vector3 direction = (_target.position - transform.position).normalized;
            direction.z = 0;

            transform.up = direction;
        }

        private void DamageTarget()
        {
            float damage = _damagePerSecond * Time.deltaTime;
            _targetDamageable.Damage(damage);
        }

        private void Update()
        {
            if(_target.IsNotNull())
                UpdateTransformTowardsTarget();

            if(_targetDamageable.IsNotNull())
                DamageTarget();
        }
    }
}
