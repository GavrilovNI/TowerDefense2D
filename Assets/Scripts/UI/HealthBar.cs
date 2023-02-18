using Game.Core;
using Game.CustomAttributes;
using Game.Extensions.Unity;
using UnityEngine;

namespace Game.UI
{
    public class HealthBar<T> : MonoBehaviour where T : IHaveHealth
    {
        [SerializeField]
        private Transform _backgroundBar;
        [SerializeField]
        private Transform _foregroundBar;

        [SerializeField, InitializationField]
        protected T HealthOwner;

        private void UpdateHealthBar()
        {
            float healthPercent = HealthOwner.CurrentHealth / HealthOwner.MaxHealth;

            _foregroundBar.localScale = new Vector3(healthPercent, _foregroundBar.localScale.y, _foregroundBar.localScale.z);
            _foregroundBar.localPosition = new Vector3(healthPercent / 2f - 0.5f, _foregroundBar.localPosition.y, _foregroundBar.localPosition.z);
        }

        protected bool DiactivateIfOwnerNotValid()
        {
            if(Application.isPlaying == false)
                return false;

            if(HealthOwner.IsNull())
            {
                Debug.LogWarning($"Component {nameof(IHaveHealth)} not found. Diactivating '{gameObject.name}'", this);
                gameObject.SetActive(false);
                return true;
            }
            return false;
        }

        protected virtual void OnEnable()
        {
            if(DiactivateIfOwnerNotValid())
                return;

            HealthOwner.HealthChanged += UpdateHealthBar;
            UpdateHealthBar();
        }

        protected virtual void OnDisable()
        {
            if(HealthOwner.IsNotNull())
                HealthOwner.HealthChanged -= UpdateHealthBar;
        }
    }
}
