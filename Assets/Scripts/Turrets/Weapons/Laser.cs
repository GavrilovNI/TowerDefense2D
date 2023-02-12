using Game.Core;
using Game.Enemies;
using Game.Extensions.Unity;
using UnityEngine;

namespace Game.Turrets.Weapons
{
    public class Laser : TargettedEffectTransmitter
    {
        [SerializeField]
        private Transform _beam;

        private void DisableBeam()
        {
            _beam.gameObject.SetActive(false);
        }

        private void EnableBeam()
        {
            _beam.gameObject.SetActive(true);
        }

        private void UpdateTransformTowardsTarget()
        {
            float length = Vector2.Distance(transform.position, Target.transform.position);

            var scale = transform.lossyScale;
            scale.y = length;
            transform.SetGlobalScale(scale);

            Vector3 direction = (Target.transform.position - transform.position).normalized;
            direction.z = 0;

            transform.up = direction;
        }

        protected override void OnTargetUpdated()
        {
            base.OnTargetUpdated();

            UpdateBeamActivityByTarget();
        }

        protected void UpdateBeamActivityByTarget()
        {
            if(Target == null)
                DisableBeam();
            else
                EnableBeam();
        }

        private void Awake()
        {
            UpdateBeamActivityByTarget();
        }

        private void Update()
        {
            if(Target.IsNotNull())
                UpdateTransformTowardsTarget();
        }
    }
}
