using Game.Enemies;
using Game.Extensions.Linq;
using Game.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Turrets.EnemySelectors
{
    [CreateAssetMenu(fileName = nameof(ByPassedDistanceEnemySelector), menuName = EnemySelector.DefaultMenuPath + nameof(ByPassedDistanceEnemySelector))]

    public class ByPassedDistanceEnemySelector : EnemySelector
    {
        public enum SelectionType
        {
            First,
            Last
        }

        [SerializeField]
        private SelectionType _selectionType = SelectionType.First;

        public override Enemy? Select(IEnumerable<Enemy> enemies)
        {
            List<PathFollower> pathFollowers = new();

            foreach(var enemy in enemies)
            {
                var pathFollower = enemy.GetComponent<PathFollower>();
                if(pathFollower != null)
                    pathFollowers.Add(pathFollower);
            }

            if(pathFollowers.Count() == 0)
                return null;

            return _selectionType switch
            {
                SelectionType.First => pathFollowers.MaxByOrDefault(pathFollower => pathFollower.PassedDistanceInPercent).GetComponent<Enemy>(),
                SelectionType.Last => pathFollowers.MinByOrDefault(pathFollower => pathFollower.PassedDistanceInPercent).GetComponent<Enemy>(),
                _ => throw new InvalidOperationException($"{nameof(SelectionType)} '{_selectionType}' is not supported")
            };
        }
    }
}
