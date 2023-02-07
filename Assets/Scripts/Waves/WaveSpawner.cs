using Game.Enemies;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Waves
{
    public class WaveSpawner : MonoBehaviour
    {
        public event Action<WaveSpawner> SpawnedAll;

        public bool IsSpawning => _spawnCoroutine != null;
        public EnemySpawner EnemySpawner => _enemySpawner;

        [SerializeField]
        private EnemySpawner _enemySpawner;
        [SerializeReference]
        private IWave _wave = new SimpleWave();

        private Coroutine _spawnCoroutine;

        [ContextMenu("Start Spawning")]
        public void StartSpawning()
        {
            if(IsSpawning)
                throw new InvalidOperationException("Another spawning is in progress.");

            _spawnCoroutine = StartCoroutine(Spawn());
        }

        [ContextMenu("Start Spawning")]
        public void StopSpawning()
        {
            if(IsSpawning == false)
                throw new InvalidOperationException("Spawning is not in progress");

            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator Spawn()
        {
            yield return _wave.Spawn(_enemySpawner);
            _spawnCoroutine = null;
            SpawnedAll?.Invoke(this);
        }
    }
}
