using Game.Core;
using Game.Enemies;
using Game.Extensions.Unity;
using Game.Turrets.Weapons;
using UnityEngine;

namespace Game.Turrets
{
    public class ShootingTurret : SingleTargetTurret
    {
        [SerializeField]
        private HomingBullet _bullet;
        [SerializeField]
        private UnityTimer _shootingTimer = new(1f, true);

        private bool _canShoot = false;

        protected virtual void Awake()
        {
            _shootingTimer.Fired += OnShootingTimerFired;
        }

        protected override void Update()
        {
            base.Update();
            if(_canShoot == false)
                _shootingTimer.Tick();
        }

        protected virtual void OnDestroy()
        {
            _shootingTimer.Fired -= OnShootingTimerFired;
        }

        protected virtual void ShootAt(Enemy enemy)
        {
            var bullet = GameObject.Instantiate(_bullet, transform.position, transform.rotation);
            bullet.SetTarget(CurrentTarget.transform);
        }

        protected override void OnTargetUpdated(Enemy oldTarget, Enemy newTarget)
        {
            if(newTarget.IsNotNull() && _canShoot)
                ShootAtInner(newTarget);
        }

        private void ShootAtInner(Enemy enemy)
        {
            ShootAt(enemy);
            _canShoot = false;
        }

        private void OnShootingTimerFired()
        {
            _canShoot = true;

            bool hasTarget = CurrentTarget.IsNotNull();
            if(hasTarget)
                ShootAtInner(CurrentTarget);
        }
    }
}
