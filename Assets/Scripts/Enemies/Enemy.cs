using Game.Core;
using System;
using UnityEngine;

namespace Game.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public event Action<Enemy> Died;

        public bool IsAlive => _isAlive;
        public int DamageToTower => _damageToTower;

        [SerializeField, Min(0)]
        private int _damageToTower = 1;
        [SerializeField, Min(0)]
        private float _maxHealth = 100;
        [SerializeField, Min(0)]
        private float _health = 100;

        private bool _isAlive = true;

        public void Damage(float damage)
        {
            if(damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            _health -= damage;
            if(_health <= 0)
                OnDied();
        }

        [ContextMenu("Kill")]
        public void Kill()
        {
            _health = 0;
            OnDied();
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