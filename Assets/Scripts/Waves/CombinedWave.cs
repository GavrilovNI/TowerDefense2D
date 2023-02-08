using Game.CustomAttributes;
using Game.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Waves
{
    [CreateAssetMenu(fileName = nameof(CombinedWave), menuName = "ScriptableObjects/Wave/" + nameof(CombinedWave))]
    public class CombinedWave : Wave
    {
        [SerializeField]
        [ExtendScriptableObject]
        private List<Wave> _waves = new();

        public override IEnumerator Spawn(EnemySpawner enemySpawner)
        {
            for(int i = 0; i < _waves.Count; ++i)
                yield return _waves[i].Spawn(enemySpawner);
        }
    }
}
