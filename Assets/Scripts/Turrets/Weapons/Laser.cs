using Game.Core;
using Game.Enemies;
using Game.EnemyEffects;
using Game.Extensions.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Turrets.Weapons
{
    public class Laser : WeaponWithEffects<ContinuousEnemyEffect>
    {
        [SerializeField]
        private Transform _beam;
        [SerializeField, Min(0)]
        private float _damagePerSecond = 1f;
        [SerializeField]
        private Transform? _target;

        private IDamageable _targetDamageable;
        private Enemy _targetEnemy;

        private readonly List<Coroutine> _appliedEffectCoroutines = new();

        public void SetTarget(Transform? target)
        {
            if(System.Object.ReferenceEquals(_target, target))
                return;

            OnUpdatingTarget();

            _target = target;

            if(_target.IsNull())
            {
                _target = null;
            }
            else
            {
                _targetDamageable = _target.GetComponent<IDamageable>();
                _targetEnemy = _target.GetComponent<Enemy>();
            }

            OnTargetUpdated();
        }

        protected override void ApplyEffects(Enemy target)
        {
            foreach(var effect in Effects)
            {
                effect.ApplyPermantly(target, out Coroutine effectCoroutine);
                _appliedEffectCoroutines.Add(effectCoroutine);
            }
        }

        protected virtual void StopAllEffects(Enemy enemy)
        {
            foreach(var effectCoroutine in _appliedEffectCoroutines)
                enemy.StopCoroutine(effectCoroutine);

            _appliedEffectCoroutines.Clear();
        }

        private void OnUpdatingTarget()
        {
            if(_targetEnemy.IsNotNull())
                StopAllEffects(_targetEnemy);
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

                if(_targetEnemy.IsNotNull())
                    ApplyEffects(_targetEnemy);
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
