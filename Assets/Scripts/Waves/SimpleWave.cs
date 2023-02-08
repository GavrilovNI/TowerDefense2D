using Game.Enemies;
using System.Collections;
using UnityEngine;

namespace Game.Waves
{
    [CreateAssetMenu(fileName = nameof(SimpleWave), menuName = "ScriptableObjects/Wave/" + nameof(SimpleWave))]
    public class SimpleWave : Wave
    {
        [SerializeField]
        private Enemy _enemyPrefab;
        [SerializeField, Min(0)]
        private int _count = 1;
        [SerializeField, Min(0.0001f)]
        private float _timeInSecondsBetweenSpawns = 1f;

        public override IEnumerator Spawn(EnemySpawner enemySpawner)
        {
            for(int i = 0; i < _count; ++i)
            {
                enemySpawner.Spawn(_enemyPrefab);
                yield return new WaitForSeconds(_timeInSecondsBetweenSpawns);
            }
        }
    }
}
