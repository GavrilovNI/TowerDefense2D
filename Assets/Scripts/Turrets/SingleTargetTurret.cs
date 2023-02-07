using Game.Enemies;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Turrets
{
    public abstract class SingleTargetTurret : Turret
    {
        protected Enemy? CurrentTarget { get; private set; }

        protected virtual Enemy? ChooseTarget(HashSet<Enemy> _enemies)
        {
            if(_enemies.Count == 0)
                return null;
            return _enemies.First();
        }

        protected virtual void OnTargetUpdated(Enemy? oldTarget, Enemy? newTarget)
        {

        }

        protected void UpdateTarget(HashSet<Enemy> enemies)
        {
            var oldTarget = CurrentTarget;
            CurrentTarget = ChooseTarget(enemies);

            if(Object.ReferenceEquals(oldTarget, CurrentTarget) == false)
                OnTargetUpdated(oldTarget, CurrentTarget);
        }

        protected virtual void Update()
        {
            UpdateTarget(_enemies);
        }
    }
}
