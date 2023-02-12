﻿using Game.CustomAttributes;
using Game.Enemies;
using Game.EnemyEffects;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Turrets.Weapons
{
    public abstract class WeaponWithEffects<T> : MonoBehaviour, IEnemyEffectsTransmitter<T> where T : EnemyEffect
    {
        [SerializeField]
        [ExtendScriptableObject]
        protected List<T> Effects = new();

        public void AddEffect(T effect) => Effects.Add(effect);
        public void AddEffects(IEnumerable<T> effects) => Effects.AddRange(effects);

        protected virtual void ApplyEffects(Enemy target)
        {
            foreach(var effect in Effects)
                effect.Apply(target);
        }
    }
}
