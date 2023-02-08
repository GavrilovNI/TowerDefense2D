using Game.Enemies;
using UnityEngine;

namespace Game.EnemyEffects
{
    public abstract class EnemyEffect : ScriptableObject
    {
        public abstract void Apply(Enemy enemy);
    }
}
