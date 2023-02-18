using Game.Enemies;
using Game.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Turrets.EnemySelectors
{
    [CreateAssetMenu(fileName = nameof(ByHealthEnemySelector), menuName = EnemySelector.DefaultMenuPath + nameof(ByHealthEnemySelector))]

    public class ByHealthEnemySelector : EnemySelector
    {
        public enum SelectionType
        {
            HighestHealth,
            LowestHealth
        }

        [SerializeField]
        private SelectionType _selectionType = SelectionType.HighestHealth;

        public override Enemy? Select(IEnumerable<Enemy> enemies)
        {
            if(enemies.Count() == 0)
                return null;

            return _selectionType switch
            {
                SelectionType.HighestHealth => enemies.MaxByOrDefault(enemy => enemy.CurrentHealth),
                SelectionType.LowestHealth => enemies.MinByOrDefault(enemy => enemy.CurrentHealth),
                _ => throw new InvalidOperationException($"{nameof(SelectionType)} '{_selectionType}' is not supported")
            };
        }


    }
}
