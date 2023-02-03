using Game.Waves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Game.Editors
{
    [CustomPropertyDrawer(typeof(IWave), true)]
    public class IWavePropertyDrawer : PropertyDrawer
    {
        private static Type[] _waveTypes = new Type[0];
        private static string[] _noneAndWaveNames = new string[0];

        public override void OnGUI(Rect initialRect, SerializedProperty property, GUIContent label)
        {
            IWave wave = property.boxedValue as IWave;

            var labelRect = new Rect(initialRect.x, initialRect.y,
                                     initialRect.width, EditorGUIUtility.singleLineHeight);
            var popupRect = new Rect(initialRect.x, labelRect.yMax + +EditorGUIUtility.standardVerticalSpacing,
                                     initialRect.width, EditorGUIUtility.singleLineHeight);
            var waveRect = new Rect(initialRect.x, popupRect.yMax + EditorGUIUtility.standardVerticalSpacing,
                                    initialRect.width, EditorGUIUtility.singleLineHeight);

            GUIContent waveLabel = new(label.text, label.image, label.tooltip);
            EditorGUI.LabelField(labelRect, waveLabel);

            var oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            int oldSelected = 0;
            if(wave != null)
                oldSelected = Array.IndexOf(_waveTypes, wave.GetType()) + 1;

            int newSelected = EditorGUI.Popup(popupRect, "Wave Type", oldSelected, _noneAndWaveNames);

            if(newSelected != oldSelected)
            {
                IWave newWave = null;
                if(newSelected > 0)
                {
                    var newType = _waveTypes[newSelected - 1];
                    newWave = (IWave)Activator.CreateInstance(newType);
                }
                property.boxedValue = newWave;
            }

            if(wave != null)
                EditorGUI.PropertyField(waveRect, property, new GUIContent("Wave Object"), true);

            EditorGUI.indentLevel = oldIndentLevel;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            bool hasBoxedValue = property.boxedValue != null;
            if(hasBoxedValue)
            {
                height += EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(property, true);
            }

            return height;
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void UpdateWaveTypes()
        {
            _waveTypes = Inheritance.GetNonAbstractChildrenOfType<IWave>().ToArray();
            var waveNames = _waveTypes.Select(type => type.Name).ToList();
            waveNames.Insert(0, "None");
            _noneAndWaveNames = waveNames.ToArray();
        }
    }
}
