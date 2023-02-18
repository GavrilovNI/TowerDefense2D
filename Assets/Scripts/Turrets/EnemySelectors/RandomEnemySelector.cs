using Game.Enemies;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Turrets.EnemySelectors
{
    [CreateAssetMenu(fileName = nameof(RandomEnemySelector), menuName = EnemySelector.DefaultMenuPath + nameof(RandomEnemySelector))]

    public class RandomEnemySelector : EnemySelector
    {
        public override Enemy? Select(IEnumerable<Enemy> enemies)
        {
            int count = enemies.Count();

            if(count == 0)
                return null;

            return enemies.ElementAt(UnityEngine.Random.Range(0, count));
        }
    }
}
