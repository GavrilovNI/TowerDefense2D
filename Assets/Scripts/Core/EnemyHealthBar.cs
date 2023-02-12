using Game.Enemies;
using UnityEngine;

namespace Game.Core
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField]
        private Transform _backgroundBar;
        [SerializeField]
        private Transform _foregroundBar;

        [SerializeField]
        private Enemy _enemy;

        private void UpdateHealthBar()
        {
            float healthPercent = _enemy.CurrentHealth / _enemy.MaxHealth;

            _foregroundBar.localScale = new Vector3(healthPercent, _foregroundBar.localScale.y, _foregroundBar.localScale.z);
            _foregroundBar.localPosition = new Vector3(healthPercent / 2f - 0.5f, _foregroundBar.localPosition.y, _foregroundBar.localPosition.z);
        }

        private void OnEnable()
        {
            _enemy.HealthChanged += UpdateHealthBar;
            UpdateHealthBar();
        }

        private void OnDisable()
        {
            _enemy.HealthChanged -= UpdateHealthBar;
        }
    }
}
