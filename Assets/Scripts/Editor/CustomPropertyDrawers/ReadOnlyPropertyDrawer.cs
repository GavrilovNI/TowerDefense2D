using Game.CustomAttributes;
using UnityEditor;
using UnityEngine;

namespace Game.Editors.CustomPropertyDrawers
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            var propertyPath = property.propertyPath.Split('.');
            bool isArrayPart = propertyPath.Length > 1 && propertyPath[^2] == "Array";
            if(isArrayPart)
            {
                EditorGUILayout.LabelField($"{nameof(ReadOnlyAttribute)} not working properly with arrays!!!");
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
            GUI.enabled = true;
        }
    }
}
