using Game.Core;
using Game.CustomAttributes;
using Game.Extensions.Unity;
using UnityEngine;

namespace Game.UI
{
    public class AnyHealthBar : HealthBar<IHaveHealth>
    {
        [SerializeField, InitializationField]
        private Transform _healthOwner;

        protected override void OnEnable()
        {
            if(_healthOwner.IsNotNull())
                HealthOwner = _healthOwner.GetComponent<IHaveHealth>();
            else
                HealthOwner = null;

            base.OnEnable();
        }
    }
}
