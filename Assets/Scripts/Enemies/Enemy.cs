using Game.Core;
using Game.EnemyEffects;
using System;
using UnityEngine;

namespace Game.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable, IHaveHealth
    {
        public event Action<Enemy> Died;
        public event Action HealthChanged;

        public bool IsAlive => _isAlive;
        public float DamageToTower => _damageToTower;

        public float CurrentHealth => _health;
        public float MaxHealth => _maxHealth;

        public EffectsApplier EffectsApplier;

        [SerializeField, Min(0)]
        private float _damageToTower = 1;
        [SerializeField, Min(0)]
        private float _maxHealth = 100;
        [SerializeField, Min(0)]
        private float _health = 100;

        private bool _isAlive = true;

        public Enemy()
        {
            EffectsApplier = new(this);
        }

        public void Damage(float damage)
        {
            if(damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            _health -= damage;
            HealthChanged?.Invoke();

            if(_health <= 0)
                OnDied();
        }

        [ContextMenu("Kill")]
        public void Kill()
        {
            Damage(_health);
        }
        
        private void Start()
        {
            if(_health <= 0)
                OnDied();
        }

        private void OnDestroy()
        {
            if(_isAlive)
                OnDied();

            EffectsApplier = null; // free memory
        }

        private void OnDied()
        {
            if(_isAlive == false)
                return;

            _isAlive = false;
            Died?.Invoke(this);

#if UNITY_EDITOR
            if(Application.isPlaying)
                GameObject.Destroy(gameObject);
            else
                gameObject.SetActive(false);
#else
            GameObject.Destroy(gameObject);
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_health > _maxHealth)
                _health = _maxHealth;

            if(Application.isPlaying)
            {
                if(_health <= 0)
                    OnDied();
            }
        }
#endif
    }

}