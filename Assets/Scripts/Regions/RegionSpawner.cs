using Game.Core;
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

        [SerializeField]
        private Vector3 _spawnRegionOffset = Vector3.forward;

        private LevelTimeObject _levelTimeObject;

        private bool CanSpawn()
        {
            return _levelTimeObject == null || _levelTimeObject.IsLevelStopped == false;
        }

        public void Spawn()
        {
            if(CanSpawn() == false)
                return;

            RegionWithEffects region = GameObject.Instantiate(_regionPrefab, transform.position + _spawnRegionOffset, transform.rotation);

            region.AddOneTimeEffects(OnEnterEffects);
            region.AddContinuousEffects(InsideContinuousEffects);
        }

        private void Awake()
        {
            _levelTimeObject = GetComponent<LevelTimeObject>();
        }
    }
}
