using Game.Enemies;
using System.Collections;
using UnityEngine;

namespace Game.Waves
{
    public abstract class Wave : ScriptableObject
    {
        public abstract IEnumerator Spawn(EnemySpawner enemySpawner);
    }
}
