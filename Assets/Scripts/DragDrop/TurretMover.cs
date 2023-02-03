using Game.Turrets;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DragDrop
{
    public class TurretMover : MonoBehaviour
    {
        private const int MouseButtonIndex = 0;

        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private LayerMask _turretHoldersMask;
        [SerializeField]
        private Vector3 _turretMovingOffset = Vector3.forward;
        [SerializeField]
        private bool _destroyIfCantFindPlaceToDrop = true;

        private Turret _holdingTurret;
        private IObjectHolder<Turret> _oldTurretHolder;

        private void OnTookTurret()
        {
            _holdingTurret.enabled = false;
        }

        private void OnDroppingTurret()
        {
            _holdingTurret.enabled = true;
        }

        private void DropTurret()
        {
            List<IObjectTaker<Turret>> drops = GetComponentsUnderMouse<IObjectTaker<Turret>>();

            foreach(var drop in drops)
            {
                if(TryDropAt(drop))
                    return;
            }

            if(TryDropAt(_oldTurretHolder))
                return;

            if(_destroyIfCantFindPlaceToDrop)
                GameObject.Destroy(_holdingTurret.gameObject);
        }

        private bool TryDropAt(IObjectTaker<Turret> turretTaker)
        {
            if(turretTaker.CanTake(_holdingTurret))
            {
                OnDroppingTurret();
                turretTaker.Take(_holdingTurret);
                _holdingTurret = null;
                _oldTurretHolder = null;
                return true;
            }
            return false;
        }

        private bool TryTakeTurret()
        {
            List<IObjectHolder<Turret>> turretHolders = GetComponentsUnderMouse<IObjectHolder<Turret>>();

            foreach(var turretHolder in turretHolders)
            {
                if(turretHolder.CanGet())
                {
                    _holdingTurret = turretHolder.Get();
                    _oldTurretHolder = turretHolder;
                    OnTookTurret();
                    return true;
                }
            }

            return false;
        }

        private List<T> GetComponentsUnderMouse<T>()
        {
            List<T> foundComponents = new();

            Vector3 raycastPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(raycastPosition, Vector2.zero);//, int.MaxValue, _turretHoldersMask.value);
            for (int i = 0; i < hits.Length; ++i)
            {
                var foundComponent = hits[i].transform.GetComponent<T>();
                if(foundComponent != null)
                    foundComponents.Add(foundComponent);
            }

            return foundComponents;
        }

        private void MoveTurret()
        {
            Vector3 newTurretPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            newTurretPosition += _turretMovingOffset;
            _holdingTurret.transform.position = newTurretPosition;
        }
        
        private void Update()
        {
            if(_holdingTurret == null)
            {
                if(Input.GetMouseButtonDown(MouseButtonIndex))
                    TryTakeTurret();
            }
            else
            {
                if(Input.GetMouseButtonUp(MouseButtonIndex))
                    DropTurret();
                else
                    MoveTurret();
            }
        }
    }
}
