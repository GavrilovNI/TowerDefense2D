using Game.EnemyEffects;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Editors
{
    [CustomPropertyDrawer(typeof(IEnemyEffect), true)]
    public class IEnemyEffectPropertyDrawer : PropertyDrawer
    {
        private static Type[] _effectTypes = new Type[0];
        private static string[] _noneAndEffectNames = new string[0];

        public override void OnGUI(Rect initialRect, SerializedProperty property, GUIContent label)
        {
            IEnemyEffect effect = property.boxedValue as IEnemyEffect;

            var labelRect = new Rect(initialRect.x, initialRect.y,
                                     initialRect.width, EditorGUIUtility.singleLineHeight);
            var popupRect = new Rect(initialRect.x, labelRect.yMax + +EditorGUIUtility.standardVerticalSpacing,
                                     initialRect.width, EditorGUIUtility.singleLineHeight);
            var effectRect = new Rect(initialRect.x, popupRect.yMax + EditorGUIUtility.standardVerticalSpacing,
                                    initialRect.width, EditorGUIUtility.singleLineHeight);

            GUIContent effectLabel = new(label.text, label.image, label.tooltip);
            EditorGUI.LabelField(labelRect, effectLabel);

            var oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            int oldSelected = 0;
            if(effect != null)
                oldSelected = Array.IndexOf(_effectTypes, effect.GetType()) + 1;

            int newSelected = EditorGUI.Popup(popupRect, "EnemyEffect Type", oldSelected, _noneAndEffectNames);

            if(newSelected != oldSelected)
            {
                IEnemyEffect newEnemyEffect = null;
                if(newSelected > 0)
                {
                    var newType = _effectTypes[newSelected - 1];
                    newEnemyEffect = (IEnemyEffect)Activator.CreateInstance(newType);
                }
                property.boxedValue = newEnemyEffect;
            }

            if(effect != null)
                EditorGUI.PropertyField(effectRect, property, new GUIContent("EnemyEffect Object"), true);

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
        private static void UpdateEnemyEffectTypes()
        {
            _effectTypes = Inheritance.GetNonAbstractChildrenOfType<IEnemyEffect>().ToArray();
            var enemyEffectNames = _effectTypes.Select(type => type.Name).ToList();
            enemyEffectNames.Insert(0, "None");
            _noneAndEffectNames = enemyEffectNames.ToArray();
        }
    }
}
