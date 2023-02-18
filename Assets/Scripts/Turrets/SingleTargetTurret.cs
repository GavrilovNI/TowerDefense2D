using Game.CustomAttributes;
using Game.Enemies;
using Game.Extensions.Unity;
using Game.Turrets.EnemySelectors;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Turrets
{
    public abstract class SingleTargetTurret : Turret
    {
        [SerializeField, ExtendScriptableObject]
        protected EnemySelector _enemySelector;

        protected Enemy? CurrentTarget { get; private set; }

        protected Enemy? ChooseTarget(HashSet<Enemy> _enemies)
        {
            return _enemySelector.Select(_enemies);
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
            bool shouldUpdateTarget = CurrentTarget.IsNull() || CurrentTarget.IsAlive == false;
            if(shouldUpdateTarget)
                UpdateTarget(_enemies);
        }
    }
}
