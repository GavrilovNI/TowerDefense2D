using Game.Core;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Paths
{
    public class PathFollower : MonoBehaviour
    {
        public event Action<PathFollower> Finished;

        public enum FollowState
        {
            NotStarted,
            Following,
            Paused,
            Finished
        }

        public FollowState State { get; private set; } = FollowState.NotStarted;
        public Path Path => _path;
        public Vector3 CurrentTarget => _currentTarget;
        public float Speed
        {
            get => _speed;
            set
            {
                if(value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _speed = value;
            }
        }

        [SerializeField, Min(0)]
        private float _speed = 1f;
        [SerializeField, Min(0)]
        private float _reachDistance = 0.0001f;
        [SerializeField]
        private Path _path;
#if UNITY_EDITOR
        [Header("Editor")]
        [SerializeField]
        private Color _targetColor = Color.magenta;
        [SerializeField, Min(0)]
        private float _targetGizmosRadius = 0.1f;
        [SerializeField]
        private Color _textColor = Color.red;
        [SerializeField]
        private int _textSize = 18;
#endif

        private Vector3 _currentTarget;
        private int _currentTargetIndex;

        [ContextMenu("Start Path From Begin")]
        public void RestartCurrentPathFromBegin()
        {
            StartPathFromBegin(_path);
        }

        [ContextMenu("Start Path From Closest Point")]
        public void RestartCurrentPathFromClosestPoint()
        {
            StartPathFromClosestPoint(_path);
        }

        [ContextMenu("Pause")]
        public void Pause()
        {
            if(State != FollowState.Following)
                throw new InvalidOperationException("Can' t pause, because not following.");
            State = FollowState.Paused;
        }

        [ContextMenu("Continue")]
        public void Continue()
        {
            if(State != FollowState.Paused)
                throw new InvalidOperationException("Can' t continue, because wasn't paused.");
            State = FollowState.Following;
        }

        public void StartPathFromBegin(Path path)
        {
            AssertPathIsGood(path);

            _path = path;
            SetCurrentTarget(0, _path[0]);
            State = FollowState.Following;
        }

        public void StartPathFromClosestPoint(Path path)
        {
            AssertPathIsGood(path);

            _path = path;

            Vector3 closestPoint = Math3d.GetClosestPointOnPath(transform.position, path.ToArray(), out int prevPointIndex);

            SetCurrentTarget(prevPointIndex, closestPoint);
            State = FollowState.Following;
        }

        private void FixedUpdate()
        {
            if(State != FollowState.Following)
                return;

            float distanceToGo = _speed * Time.fixedDeltaTime;

            bool finished = FollowPath(distanceToGo);

            if(finished)
            {
                State = FollowState.Finished;
                Finished?.Invoke(this);
            }
        }

        private void AssertPathIsGood(Path path)
        {
            if(path == null)
                throw new ArgumentNullException(nameof(path));
            if(path.Count == 0)
                throw new InvalidOperationException("Path has no points");
        }

        private void SetCurrentTarget(int prevPathIndex, Vector3 newTarget)
        {
            _currentTargetIndex = prevPathIndex;
            _currentTarget = newTarget;
        }

        private void SetTargetToNextPointOnPath()
        {
            _currentTargetIndex++;
            _currentTarget = _path[_currentTargetIndex];
        }

        private bool FollowPath(float distanceToGo)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _currentTarget);
            bool reachedTarget = distanceToTarget < _reachDistance;

            if(reachedTarget)
            {
                bool isLastTarget = _currentTargetIndex >= _path.Count - 1;
                if(isLastTarget)
                {
                    return true;
                }
                else
                {
                    SetTargetToNextPointOnPath();
                    if(distanceToGo < distanceToTarget)
                        return false;
                    return FollowPath(distanceToGo - distanceToTarget);
                }
            }
            else
            {
                float maxDistanceCanGo = MathF.Min(distanceToGo, distanceToTarget);
                Vector3 direction = (_currentTarget - transform.position).normalized;
                transform.position += direction * maxDistanceCanGo;

                float leftDistance = distanceToGo - maxDistanceCanGo;
                if(leftDistance > 0)
                    return FollowPath(leftDistance);
            }
            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            GUIStyle textStyle = new();
            textStyle.fontSize = _textSize;
            textStyle.normal.textColor = _textColor;

            Handles.Label(transform.position, State.ToString(), textStyle);
            if(State == FollowState.Following || State == FollowState.Paused)
            {
                Handles.Label(_currentTarget, "target", textStyle);
                Gizmos.color = _targetColor;
                Gizmos.DrawWireSphere(_currentTarget, _targetGizmosRadius);
            }
        }
#endif
    }
}
