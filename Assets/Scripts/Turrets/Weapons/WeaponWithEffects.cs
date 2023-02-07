using Game.Enemies;
using Game.EnemyEffects;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Turrets.Weapons
{
    public abstract class WeaponWithEffects<T> : MonoBehaviour, IEnemyEffectsTransmitter<T> where T : IEnemyEffect
    {
        [SerializeReference]
        protected List<T> Effects = new();

        public void AddEffect(T effect) => Effects.Add(effect);

        protected virtual void ApplyEffects(Enemy target)
        {
            foreach(var effect in Effects)
                effect.Apply(target);
        }
    }
}
