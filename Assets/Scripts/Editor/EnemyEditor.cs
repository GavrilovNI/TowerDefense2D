using Game.Enemies;
using UnityEditor;
using UnityEngine;

namespace Game.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Enemy))]
    public class EnemyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            Enemy enemy = (Enemy)target;

            if(GUILayout.Button("Kill"))
            {
                Undo.RecordObjects(new UnityEngine.Object[] { enemy, enemy.gameObject }, "Enemy Kill");
                enemy.Kill();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
