using Game.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Turrets.EnemySelectors
{
    public abstract class EnemySelector : ScriptableObject
    {
        public const string DefaultMenuPath = "ScriptableObjects/EnemySelectors/";

        public abstract Enemy? Select(IEnumerable<Enemy> enemies);
    }
}
