using Game.Core;
using Game.CustomAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Paths
{
    public class Path : MonoBehaviour, IEnumerable<Vector3>
    {
        private const float Epsilon = 0.0001f;

        [SerializeField, ReadOnly]
        private float _distance;
        public float Distance => _distance;

        public int Count => _points.Count;
        public Vector3 this[int index] => _points[index];


        [SerializeField, HideInInspector]
        private List<Vector3> _points = new();
        [SerializeField, HideInInspector]
        private List<float> _distanceToNextPoint = new();


        public void AddPoint(int index, Vector3 point)
        {
            if(index < 0 || index > _points.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _points.Insert(index, point);

            RecalculateDistancesAfterAddingNewPoint(index);
        }

        public void RemovePoint(int index)
        {
            if(index < 0 || index >= _points.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _points.RemoveAt(index);

            RecalculateDistancesAfterRemovingPoint(index);
        }

        public void SetPoint(int index, Vector3 newValue)
        {
            if(index < 0 || index >= _points.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _points[index] = newValue;
            RecalculateDistancesOfChangedPoint(index);
        }

        public Vector3 GetClosestPosition(Vector3 fromPosition)
        {
            return Math3d.GetClosestPointOnPath(fromPosition, _points.ToArray());
        }
    
        public Vector3 GetPositionByDistanceFromStart(float distanceFromStart)
        {
            if(distanceFromStart < 0 || distanceFromStart > _distance)
                throw new ArgumentOutOfRangeException(nameof(distanceFromStart));

            if(_points.Count == 0)
                throw new InvalidOperationException("Path has no points");

            if(distanceFromStart <= Epsilon)
                return _points[0];

            float currentDistance = 0;

            for(int i = 0; i < _points.Count; ++i)
            {
                var distanceToNextPoint = _distanceToNextPoint[i];
                if(distanceToNextPoint < Epsilon)
                    continue;

                var newDistance = currentDistance + distanceToNextPoint;

                if(newDistance > distanceFromStart)
                {
                    float t = (newDistance - distanceFromStart) / distanceToNextPoint;
                    return Vector3.Lerp(_points[i], _points[i + 1], t);
                }

                currentDistance = newDistance;
            }

            return _points[^1];
        }

        private void Awake()
        {
            RecalculateDistances();
        }

        private void RecalculateDistances()
        {
            _distance = 0;
            _distanceToNextPoint.Clear();

            for(int i = 0; i < _points.Count - 1; ++i)
            {
                var distance = Vector3.Distance(_points[i], _points[i + 1]);
                _distanceToNextPoint.Add(distance);
                _distance += distance;
            }
        }

        private void RecalculateDistanceBetweenPointAndNext(int index)
        {
            _distance -= _distanceToNextPoint[index];
            var newDistance = Vector3.Distance(_points[index], _points[index + 1]);
            _distance += newDistance;
            _distanceToNextPoint[index] = newDistance;
        }

        private void RecalculateDistancesOfChangedPoint(int changedPointIndex)
        {
            if(_points.Count < 2)
                return;

            bool hasNext = changedPointIndex < _points.Count - 1;
            if(hasNext)
                RecalculateDistanceBetweenPointAndNext(changedPointIndex);

            bool hasPrev = changedPointIndex > 0;
            if(hasPrev)
                RecalculateDistanceBetweenPointAndNext(changedPointIndex - 1);
        }

        private void RecalculateDistancesAfterAddingNewPoint(int newPointIndex)
        {
            if(_points.Count == 1)
                return;

            if(newPointIndex == 0)
            {
                float distanceToNext = Vector3.Distance(_points[0], _points[1]);
                _distanceToNextPoint.Insert(0, distanceToNext);
                _distance += distanceToNext;
            }
            else if(newPointIndex == _points.Count - 1)
            {
                float distanceToPrev = Vector3.Distance(_points[^2], _points[^1]);
                _distanceToNextPoint.Insert(_distanceToNextPoint.Count, distanceToPrev);
                _distance += distanceToPrev;
            }
            else
            {
                _distance -= _distanceToNextPoint[newPointIndex - 1];

                Vector3 newPoint = _points[newPointIndex];

                float distanceToPrev = Vector3.Distance(_points[newPointIndex - 1], newPoint);
                _distanceToNextPoint[newPointIndex - 1] = distanceToPrev;
                _distance += distanceToPrev;

                float distanceToNext = Vector3.Distance(newPoint, _points[newPointIndex + 1]);
                _distanceToNextPoint.Insert(newPointIndex, distanceToNext);
                _distance += distanceToNext;
            }
        }

        private void RecalculateDistancesAfterRemovingPoint(int oldPointIndex)
        {
            if(_points.Count < 2)
            {
                _distanceToNextPoint.Clear();
                _distance = 0;
            }
            else
            {
                bool wasLast = oldPointIndex == _points.Count;

                if(wasLast)
                {
                    _distance -= _distanceToNextPoint[oldPointIndex - 1];
                    _distanceToNextPoint.RemoveAt(oldPointIndex - 1);
                }
                else
                {
                    _distance -= _distanceToNextPoint[oldPointIndex];
                    _distanceToNextPoint.RemoveAt(oldPointIndex);

                    if(oldPointIndex != 0)
                    {
                        float distanceBetweenOldNeighbours = Vector3.Distance(_points[oldPointIndex - 1], _points[oldPointIndex]);
                        _distanceToNextPoint[oldPointIndex - 1] = distanceBetweenOldNeighbours;
                        _distance += distanceBetweenOldNeighbours;
                    }
                }
            }
        }

        public Vector3[] ToArray() => _points.ToArray();

        public IEnumerator<Vector3> GetEnumerator() => _points.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();
    }
}
