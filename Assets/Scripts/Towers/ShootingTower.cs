using UnityEngine;
using UnityEngine.Extensions;
using System.Linq;

namespace Game.Towers
{
    public sealed class ShootingTower : Tower
    {
        [SerializeField]
        private HomingBullet _bullet;
        [SerializeField]
        private UnityTimer _shootingTimer = new(1f, true);

        private Enemy _currentTarget = null;


        private void Shoot()
        {
            bool hasTarget = _currentTarget.IsNotNull();
            if (hasTarget)
            {
                var bullet = GameObject.Instantiate(_bullet, transform.position, transform.rotation);
                bullet.SetTarget(_currentTarget.transform);
            }
        }

        private void ChooseTarget()
        {
            _currentTarget = _enemies.Count > 0 ? _enemies.Single() : null;
        }

        private void Awake()
        {
            _shootingTimer.Fired += Shoot;
        }

        private void FixedUpdate()
        {
            ChooseTarget();
            _shootingTimer.TickFixed();
        }

        private void OnDestroy()
        {
            _shootingTimer.Fired -= Shoot;
        }
    }
}
