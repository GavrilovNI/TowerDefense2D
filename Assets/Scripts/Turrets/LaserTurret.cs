using Game.Enemies;
using Game.Turrets.Weapons;
using UnityEngine;

namespace Game.Turrets
{
    public class LaserTurret : SingleTargetTurret
    {
        [SerializeField]
        private Laser _laser;

        protected override void OnTargetUpdated(Enemy oldTarget, Enemy newTarget)
        {
            _laser.SetTarget(newTarget?.transform);
        }
    }
}
