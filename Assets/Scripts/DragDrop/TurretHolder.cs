using Game.Turrets;
using System;
using UnityEngine;

namespace Game.DragDrop
{
    public class TurretHolder : MonoBehaviour, IObjectHolder<Turret>
    {
        [SerializeField]
        private Turret _turret;

        public bool CanTake(Turret turret) => _turret == null;
        public bool CanGet() => _turret != null;

        public void Take(Turret turret)
        {
            if(CanTake(turret) == false)
                throw new InvalidOperationException("Can't drop turret in here");

            SetTurret(turret);
        }

        public Turret Get()
        {
            if(CanGet() == false)
                throw new InvalidOperationException("Can't take turret from here");

            var turretToReturn = _turret;

            _turret.transform.SetParent(null, true);
            _turret = null;

            return turretToReturn;
        }

        private void SetTurret(Turret turret)
        {
            _turret = turret;
            _turret.transform.SetParent(transform);
            _turret.transform.localPosition = new Vector3(0, 0, -1);
        }

        private void Start()
        {
            if(_turret != null)
                SetTurret(_turret);
        }
    }
}
