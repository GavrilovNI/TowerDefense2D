using Game.CustomAttributes;
using Game.EnemyEffects.Effects.Abstract;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Regions
{
    public class RegionSpawner : MonoBehaviour
    {
        [SerializeField]
        private RegionWithEffects _regionPrefab;

        [SerializeField]
        [ExtendScriptableObject]
        protected List<EnemyEffect> OnEnterEffects = new();

        [SerializeField]
        [ExtendScriptableObject]
        protected List<EnemyEffect> InsideContinuousEffects = new();


        public void Spawn()
        {
            RegionWithEffects region = GameObject.Instantiate(_regionPrefab, transform.position, transform.rotation);

            region.AddOneTimeEffects(OnEnterEffects);
            region.AddContinuousEffects(InsideContinuousEffects);
        }
    }
}
