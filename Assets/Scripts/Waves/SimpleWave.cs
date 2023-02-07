using Game.Enemies;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Waves
{
    [Serializable]
    public class SimpleWave : IWave
    {
        [SerializeField]
        private Enemy _enemyPrefab;
        [SerializeField, Min(0)]
        private int _count = 1;
        [SerializeField, Min(0.0001f)]
        private float _timeInSecondsBetweenSpawns = 1f;

        public IEnumerator Spawn(EnemySpawner enemySpawner)
        {
            for(int i = 0; i < _count; ++i)
            {
                enemySpawner.Spawn(_enemyPrefab);
                yield return new WaitForSeconds(_timeInSecondsBetweenSpawns);
            }
        }
    }
}
