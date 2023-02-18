using Game.Enemies;
using Game.Paths;
using Game.Waves;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core
{
    public class Level : MonoBehaviour, IHaveHealth
    {
        public event Action<Level> Won;
        public event Action<Level> Lost;
        public event Action HealthChanged;

        [SerializeField, Min(0)]
        private float _maxHealth = 3;
        [SerializeField]
        private List<WaveSpawner> _spawners = new();

        private float _health;

        private readonly HashSet<Enemy> _enemies = new();

        private int _finishedSpawnersCount = 0;
        private bool _isWaveRunning = false;

        public float CurrentHealth
        {
            get => _health;
            set
            {
                _health = value;
                HealthChanged?.Invoke();
            }
        }

        public float MaxHealth => _maxHealth;

        [ContextMenu("Start Level")]
        public void StartLevel()
        {
            if(_isWaveRunning)
                throw new InvalidOperationException("Wave is still running");

            _finishedSpawnersCount = 0;
            CurrentHealth = MaxHealth;
            _isWaveRunning = true;

            StartAllSpawners();

            TryWin();
            TryLose();
        }

        [ContextMenu("Stop Level")]
        public void StopLevel()
        {
            if(_isWaveRunning == false)
                throw new InvalidOperationException("Wave is not running");

            _isWaveRunning = false;

            StopAllSpawners();
            KillAllEnemies();
            NotifyLevelTimeObjectsAbountStopping();
        }

        private void NotifyLevelTimeObjectsAbountStopping()
        {
            foreach(LevelTimeObject levelTimeObject in GameObject.FindObjectsOfType<LevelTimeObject>())
                levelTimeObject.OnLevelStopped();
        }

        private void Start()
        {
            foreach(var spawner in _spawners)
            {
                spawner.EnemySpawner.EnemySpawned += OnEnemySpawned;
                spawner.SpawnedAll += OnSpawnerSpawnedAll;
            }
        }

        private void OnDestroy()
        {
            foreach(var spawner in _spawners)
            {
                spawner.EnemySpawner.EnemySpawned -= OnEnemySpawned;
                spawner.SpawnedAll -= OnSpawnerSpawnedAll;
            }

            foreach(var enemy in _enemies)
                UnsubscribeFromEnemyEvents(enemy);
        }

        private void Damage(float damage)
        {
            if(damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            if(_isWaveRunning == false)
                throw new InvalidOperationException("Wave is not running");

            CurrentHealth -= damage;
            if(_health <= 0)
                OnLost();
            TryLose();
        }

        private void KillAllEnemies()
        {
            foreach(var enemy in _enemies.Reverse())
                enemy.Kill();
        }

        private void StartAllSpawners()
        {
            foreach(var spawner in _spawners)
                spawner.StartSpawning();
        }

        private void StopAllSpawners()
        {
            foreach(var spawner in _spawners)
            {
                if(spawner.IsSpawning)
                    spawner.StopSpawning();
            }
        }

        private void OnLost()
        {
            StopLevel();
            Lost?.Invoke(this);

            Debug.Log("Lost", this);
        }

        private void OnWon()
        {
            StopLevel();
            Won?.Invoke(this);

            Debug.Log("Won", this);
        }

        private void TryLose()
        {
            bool canLose = _isWaveRunning && CurrentHealth <= 0;

            if(canLose)
                OnLost();
        }

        private void TryWin()
        {
            bool isWaveFinished = _finishedSpawnersCount == _spawners.Count && _enemies.Count == 0;
            bool canWin = _isWaveRunning && _health > 0 && isWaveFinished;

            if(canWin)
                OnWon();
        }


        private void OnSpawnerSpawnedAll(WaveSpawner waveSpawner)
        {
            _finishedSpawnersCount++;
            TryWin();
        }

        private void OnEnemySpawned(Enemy enemy)
        {
            _enemies.Add(enemy);
            SubscribeToEnemyEvents(enemy);
        }

        private void OnEnemyDied(Enemy enemy)
        {
            _enemies.Remove(enemy);
            UnsubscribeFromEnemyEvents(enemy);
            TryWin();
        }

        private void OnEnemyFinishedPath(PathFollower pathFollower)
        {
            Enemy enemy = pathFollower.GetComponent<Enemy>();
            if(enemy == null)
                throw new ArgumentException($"{nameof(pathFollower)} lost {nameof(Enemy)} component");

            if(_enemies.Contains(enemy) == false)
                throw new ArgumentException("This enemy wasn't marked as spawned");

            Damage(enemy.DamageToTower);
            enemy.Kill();
        }

        private void SubscribeToEnemyEvents(Enemy enemy)
        {
            enemy.Died += OnEnemyDied;

            PathFollower pathFollower = enemy.GetComponent<PathFollower>();
            if(pathFollower != null)
                pathFollower.Finished += OnEnemyFinishedPath;
        }

        private void UnsubscribeFromEnemyEvents(Enemy enemy)
        {
            enemy.Died -= OnEnemyDied;

            PathFollower pathFollower = enemy.GetComponent<PathFollower>();
            if(pathFollower != null)
                pathFollower.Finished -= OnEnemyFinishedPath;
        }
    }
}
