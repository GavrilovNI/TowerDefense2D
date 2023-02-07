using Game.Enemies;
using System.Collections;

namespace Game.Waves
{
    public interface IWave
    {
        IEnumerator Spawn(EnemySpawner enemySpawner);
    }
}
