using System.Collections.Generic;
using UnityEngine;

namespace Game.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        protected readonly HashSet<Enemy> _enemies = new();

        protected void OnEnemyDied(Enemy enemy)
        {
            RemoveEnemy(enemy);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
                AddEnemy(enemy);
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if(enemy != null)
                RemoveEnemy(enemy);
        }

        protected void AddEnemy(Enemy enemy)
        {
            bool added = _enemies.Add(enemy);
            if(added)
                enemy.Died += OnEnemyDied;
        }

        protected void RemoveEnemy(Enemy enemy)
        {
            bool removed = _enemies.Remove(enemy);
            if(removed)
                enemy.Died -= OnEnemyDied;
        }
    }
}
