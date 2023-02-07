using Game.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Waves
{
    [Serializable]
    public class CombinedWave : IWave
    {
        [SerializeReference]
        private List<IWave> _waves = new();

        public IEnumerator Spawn(EnemySpawner enemySpawner)
        {
            for(int i = 0; i < _waves.Count; ++i)
                yield return _waves[i].Spawn(enemySpawner);
        }
    }
}
