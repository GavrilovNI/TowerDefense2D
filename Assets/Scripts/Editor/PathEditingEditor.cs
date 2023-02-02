using Game.Editing;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editors
{
    [CustomEditor(typeof(PathEditing))]
    public class PathEditingEditor : Editor
    {
        private const int AddNewPointMouseButtonIndex = 0;
        private const int RemovePointMouseButtonIndex = 1;

        private SerializedProperty _showPointsProperty;

        private PathEditing _pathEditing;
        private Path _path;

        private int _coveredHandleIndex = -1;
        private int _coveredLineIndex = -1;
        private Vector3 _pointOnCoveredLine;

        private bool ShowPoints
        {
            get => _showPointsProperty.boolValue;
            set => _showPointsProperty.boolValue = value;
        }

        private bool Editing => _pathEditing.Editing;

        private void OnEnable()
        {
            _showPointsProperty = serializedObject.FindProperty("_showPoints");
        }

        private void UpdateTarget()
        {
            _pathEditing = (PathEditing)target;
            _path = _pathEditing.Path;
        }

        private static float GetHandleSphereRadius(Vector3 position, float radius, bool keepConstSize)
        {
            if(keepConstSize)
                return radius * HandleUtility.GetHandleSize(position);
            return radius;
        }

        private static void DrawHandleSphere(Vector3 position, float radius, bool keepConstSize)
        {
            float realRadius = GetHandleSphereRadius(position, radius, keepConstSize);
            Handles.SphereHandleCap(0, position, Quaternion.identity, realRadius * 2, EventType.Repaint);
        }

        private static bool IsCoveringHandleSphere(Vector3 position, float radius, bool keepConstSize)
        {
            float realRadius = GetHandleSphereRadius(position, radius, keepConstSize);
            float distance = HandleUtility.DistanceToCircle(position, realRadius);

            return distance == 0;
        }

        private int FindCoveringHandleIndex()
        {
            for(int i = 0; i < _path.Count; ++i)
            {
                bool isHoveringHandle = IsCoveringHandleSphere(_path[i], _pathEditing.PointsRadius, _pathEditing.KeepConstHandlesSize);
                if(isHoveringHandle)
                    return i;
            }
            return -1;
        }

        private void DrawPathLines()
        {
            var prevPoint = _path[0];
            for(int i = 1; i < _path.Count; ++i)
            {
                bool isCoveringLine = _coveredHandleIndex == -1 && _coveredLineIndex == i - 1;

                var nextPoint = _path[i];

                Handles.color = isCoveringLine ? _pathEditing.CoveringColor : _pathEditing.LinesColor;
                Handles.DrawLine(prevPoint, nextPoint);

                prevPoint = nextPoint;
            }
        }

        private void DrawPointSpheres()
        {
            for(int i = 0; i < _path.Count; ++i)
            {
                var point = _path[i];

                bool isHoveringHandle = i == _coveredHandleIndex;
                Handles.color = isHoveringHandle ? _pathEditing.CoveringColor : _pathEditing.GetPathColor(i);
                DrawHandleSphere(point, _pathEditing.PointsRadius, _pathEditing.KeepConstHandlesSize);
            }
        }

        private void DrawPointPoistionHandles()
        {
            for(int i = 0; i < _path.Count; ++i)
            {
                var oldPoint = _path[i];
                var newPoint = Handles.PositionHandle(oldPoint, Tools.handleRotation);

                bool changed = newPoint != oldPoint;
                if(changed)
                    SetPoint(i, newPoint);
            }
        }


        private void OnSceneGUI()
        {
            UpdateTarget();

            Event guiEvent = Event.current;
            if(guiEvent.type == EventType.Layout)
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if(_path.Count == 0)
                return;

            if(Editing)
            {
                _coveredHandleIndex = FindCoveringHandleIndex();
                if(_coveredHandleIndex == -1)
                    TryFindClosestPointOnPathToMouse(_pathEditing.LineColliderWidthInPixels, out _coveredLineIndex, out _pointOnCoveredLine);
            }
            else
            {
                _coveredHandleIndex = -1;
                _coveredLineIndex = -1;
            }

            DrawPathLines();
            if(Editing)
                DrawPointPoistionHandles();
            DrawPointSpheres();

            if(Editing)
            {
                if(_coveredLineIndex != -1)
                {
                    Handles.color = _pathEditing.CoveringColor;
                    DrawHandleSphere(_pointOnCoveredLine, _pathEditing.CreatingPointsRadius, _pathEditing.KeepConstHandlesSize);
                }

                if(guiEvent.type == EventType.MouseDown)
                {
                    if(_coveredHandleIndex != -1)
                        OnMouseClickedOnHandle(guiEvent.button, _coveredHandleIndex);
                    else if(_coveredLineIndex != -1)
                        OnMouseClickedOnLine(guiEvent.button, _coveredLineIndex, _pointOnCoveredLine);
                }
            }
        }

        private bool TryFindClosestPointOnPathToMouse(float maxDistanceToLineInPixels,
                                                      out int lineIndex, out Vector3 point)
        {
            lineIndex = -1;
            point = Vector3.zero;

            if(_path.Count < 2)
                return false;

            int closestLineIndex = -1;
            float smallestDistance = float.MaxValue;
            Vector3 closestPoint = Vector3.zero;

            Vector3 startPoint = _path[0];
            for(int i = 1; i < _path.Count; ++i)
            {
                Vector3 endPoint = _path[i];

                var currentDistance = HandleUtility.DistanceToLine(startPoint, endPoint);

                if(currentDistance < smallestDistance && currentDistance < maxDistanceToLineInPixels)
                {
                    smallestDistance = currentDistance;
                    closestLineIndex = i - 1;
                    closestPoint = HandleUtility.ClosestPointToPolyLine(startPoint, endPoint);
                }

                startPoint = endPoint;
            }
            if(closestLineIndex >= 0)
            {
                lineIndex = closestLineIndex;
                point = closestPoint;
                return true;
            }

            return false;
        }

        private void OnMouseClickedOnLine(int mouseButton, int lineIndex, Vector3 position)
        {
            if(mouseButton == AddNewPointMouseButtonIndex)
                AddPoint(lineIndex + 1, position);
        }

        private void OnMouseClickedOnHandle(int mouseButton, int handleIndex)
        {
            if(mouseButton == RemovePointMouseButtonIndex)
                RemovePoint(handleIndex);
        }

        private void RemoveDuplicates()
        {
            if(_path.Count < 2)
                return;

            List<int> indicesToRemove = new();

            Vector3 currentPoint = _path[0];
            for(int i = 1; i < _path.Count; ++i)
            {
                Vector3 nextPoint = _path[i];
                bool isDuplicate = Vector3.Distance(currentPoint, nextPoint) < _pathEditing.DuplicatesThreshold;
                
                if(isDuplicate)
                    indicesToRemove.Add(i);
                else
                    currentPoint = nextPoint;
            }

            if(indicesToRemove.Count > 0)
            {
                Undo.RegisterCompleteObjectUndo(_path, "Remove duplicates");

                for(int i = indicesToRemove.Count - 1; i >=0; --i)
                    _path.RemovePoint(indicesToRemove[i]);

                EditorUtility.SetDirty(_path);

                Debug.Log($"Removed {indicesToRemove.Count} duplicates.");
            }
            else
            {
                Debug.Log($"No duplicates found.");
            }

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UpdateTarget();

            DrawDefaultInspector();

            if(Editing && GUILayout.Button("Remove Duplicates"))
                RemoveDuplicates();

            ShowPoints = EditorGUILayout.BeginFoldoutHeaderGroup(ShowPoints, "Points");
            if(ShowPoints)
            {
                GUI.enabled = Editing;

                for(int i = 0; i < _path.Count; ++i)
                {
                    Vector3 point = _path[i];

                    EditorGUILayout.BeginHorizontal();
                    var newPoint = EditorGUILayout.Vector3Field($"{i}", point);
                    var removePressed = GUILayout.Button("-", GUILayout.Width(50));
                    var addPressed = GUILayout.Button("+", GUILayout.Width(50));
                    if(removePressed)
                    {
                        RemovePoint(i);
                    }
                    else
                    {
                        if(point != newPoint)
                            SetPoint(i, newPoint);
                        if(addPressed)
                            AddPoint(i, newPoint);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                GUI.enabled = true;
            }

            if(Editing && GUILayout.Button("Add Point"))
            {
                Vector3 newPoint = _path.Count == 0 ? Vector3.zero : _path[^1];
                AddPoint(_path.Count, newPoint);
            }


            serializedObject.ApplyModifiedProperties();
            if(GUI.changed)
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private void AddPoint(int index, Vector3 point)
        {
            Undo.RegisterCompleteObjectUndo(_path, "Add point");
            _path.AddPoint(index, point);
            EditorUtility.SetDirty(_path);
        }

        private void RemovePoint(int index)
        {
            Undo.RegisterCompleteObjectUndo(_path, "Remove point");
            _path.RemovePoint(index);
            EditorUtility.SetDirty(_path);
        }

        private void SetPoint(int index, Vector3 point)
        {
            Undo.RegisterCompleteObjectUndo(_path, "Set point");
            _path.SetPoint(index, point);
            EditorUtility.SetDirty(_path);
        }
    }
}
