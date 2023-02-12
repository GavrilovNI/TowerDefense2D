using Game.Core;
using Game.Paths;
using System;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public event Action<Enemy> EnemySpawned;

        [SerializeField]
        private Path _path;

        public Enemy Spawn(Enemy prefab)
        {
            if(_path.Count == 0)
                throw new InvalidOperationException($"{nameof(_path)} no points.");

            Enemy spawnedEnemy = GameObject.Instantiate(prefab, _path[0], Quaternion.identity, transform);

            PathFollower pathFollower = spawnedEnemy.GetComponent<PathFollower>();
            if(pathFollower != null)
                pathFollower.StartPathFromBegin(_path);

            EnemySpawned?.Invoke(spawnedEnemy);

            return spawnedEnemy;
        }
    }
}
