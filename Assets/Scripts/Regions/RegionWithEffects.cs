using Game.CustomAttributes;
using Game.Enemies;
using Game.EnemyEffects.Effects.Abstract;
using System.Collections.Generic;
using UnityEngine;
using static Game.EnemyEffects.EffectsApplier;

namespace Game.Regions
{
    public class RegionWithEffects : MonoBehaviour
    {
        [SerializeField]
        [ExtendScriptableObject]
        protected List<EnemyEffect> OnEnterEffects = new();

        [SerializeField]
        [ExtendScriptableObject]
        protected List<EnemyEffect> InsideContinuousEffects = new();


        protected Dictionary<Enemy, List<Canceler>> _effectCancelers = new();

        public void AddOneTimeEffect(EnemyEffect effect) => OnEnterEffects.Add(effect);
        public void AddOneTimeEffects(IEnumerable<EnemyEffect> effects) => OnEnterEffects.AddRange(effects);
        public void AddContinuousEffect(EnemyEffect effect) => InsideContinuousEffects.Add(effect);
        public void AddContinuousEffects(IEnumerable<EnemyEffect> effects) => InsideContinuousEffects.AddRange(effects);

        protected virtual void ApplyEffectsOnEnter(Enemy enemy)
        {
            var effectsApplier = enemy.EffectsApplier;

            foreach(var effect in OnEnterEffects)
                effectsApplier.AddEffect(effect);

            if(InsideContinuousEffects.Count > 0)
            {
                var enemyCancelers = GetOrCreateEnemyCancelers(enemy);

                foreach(var effect in InsideContinuousEffects)
                {
                    var canceler = effectsApplier.AddEffectPermanently(effect);
                    enemyCancelers.Add(canceler);
                }
            }
        }

        protected List<Canceler> GetOrCreateEnemyCancelers(Enemy enemy)
        {
            if(_effectCancelers.TryGetValue(enemy, out List<Canceler> enemyCancelers) == false)
            {
                enemyCancelers = new();
                _effectCancelers.Add(enemy, enemyCancelers);
            }

            return enemyCancelers;
        }

        protected virtual void CancelAllEffects(Enemy enemy)
        {
            var enemyCancelers = GetOrCreateEnemyCancelers(enemy);

            foreach(var canceler in enemyCancelers)
                canceler.Cancel();
        }

        private void RemoveEffectsOnExit(Enemy enemy)
        {
            CancelAllEffects(enemy);
            _effectCancelers.Remove(enemy);
        }

        protected virtual void OnDestroy()
        {
            foreach(var enemy in _effectCancelers.Keys)
                CancelAllEffects(enemy);

            _effectCancelers.Clear();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            var enemy = collision.GetComponent<Enemy>();
            if(enemy != null)
                ApplyEffectsOnEnter(enemy);
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            var enemy = collision.GetComponent<Enemy>();
            if(enemy != null)
                RemoveEffectsOnExit(enemy);
        }
    }
}
