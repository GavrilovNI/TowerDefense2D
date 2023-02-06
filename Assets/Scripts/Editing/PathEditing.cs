using UnityEditor;
using UnityEngine;

namespace Game.Editing
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [RequireComponent(typeof(Path))]
    public class PathEditing : MonoBehaviour
    {
        [SerializeField]
        private bool _editing;
#pragma warning disable 0414 // warning CS0414: The field '' is assigned but its value is never used
        [SerializeField, HideInInspector]
        private bool _showPoints = true; // using In PathEditingEditor
#pragma warning restore 0414

        [SerializeField]
        private float _lineColliderWidthInPixels = 5f;
        [SerializeField]
        private float _pointsRadius = 0.1f;
        [SerializeField]
        private bool _keepConstHandlesSize = false;

        [SerializeField]
        private Color _startColor = Color.green;
        [SerializeField]
        private Color _middleColor = Color.gray;
        [SerializeField]
        private Color _endColor = Color.red;
        [SerializeField]
        private Color _linesColor = Color.gray;
        [SerializeField]
        private Color _coveringColor = Color.magenta;
        [SerializeField]
        private float _duplicatesThreshold = 0.001f;


        public bool Editing => _editing;

        public Color LinesColor => _linesColor;
        public Color CoveringColor => _coveringColor;

        public float LineColliderWidthInPixels => _lineColliderWidthInPixels;
        public float PointsRadius => _pointsRadius;
        public float CreatingPointsRadius => PointsRadius / 2f;
        public bool KeepConstHandlesSize => _keepConstHandlesSize;
        public float DuplicatesThreshold => _duplicatesThreshold;

        public Path Path
        {
            get
            {
                EnsureHavePath();
                return _path;
            }
        }

        private Path _path;

        public Color GetPathColor(int pointIndex)
        {
            EnsureHavePath();

            if(pointIndex == 0)
                return _startColor;
            if(pointIndex == _path.Count - 1)
                return _endColor;
            return _middleColor;
        }


        private void Awake()
        {
            EnsureHavePath();
        }

        private void EnsureHavePath()
        {
            _path = GetComponent<Path>();
            if(_path == null)
                throw new MissingComponentException($"{nameof(Path)} not found.");
        }
    }
#endif
}

