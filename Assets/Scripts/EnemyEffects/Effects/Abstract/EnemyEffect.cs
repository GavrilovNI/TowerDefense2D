using Game.Enemies;
using System.Collections;
using UnityEngine;

namespace Game.EnemyEffects.Effects.Abstract
{
    public abstract class EnemyEffect : ScriptableObject
    {
        public const string DefaultMenuPath = "ScriptableObjects/EnemyEffects/";

        public abstract IEnumerator Apply(Enemy enemy);

        public virtual void OnCancelled(Enemy enemy)
        {

        }
    }
}
