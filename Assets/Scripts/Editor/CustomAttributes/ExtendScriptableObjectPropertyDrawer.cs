using Game.CustomAttributes;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editors.CustomAttributes
{
	// Developed by Tom Kail at Inkle
	// Released under the MIT Licence as held at https://opensource.org/licenses/MIT
	// Modified by Nikita Gavrilov
	// original: https://gist.github.com/tomkail/ba4136e6aa990f4dc94e0d39ec6a058c

	[CustomPropertyDrawer(typeof(ExtendScriptableObjectAttribute))]
    public class ExtendScriptableObjectPropertyDrawer : PropertyDrawer
	{
		private static readonly List<string> _ignoreClassFullNames = new() { "TMPro.TMP_FontAsset" };

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float totalHeight = EditorGUIUtility.singleLineHeight;
			if(property.objectReferenceValue == null || !AreAnySubPropertiesVisible(property))
			{
				return totalHeight;
			}
			if(property.isExpanded)
			{
				var data = property.objectReferenceValue as ScriptableObject;
				if(data == null)
					return EditorGUIUtility.singleLineHeight;
				SerializedObject serializedObject = new(data);
				SerializedProperty prop = serializedObject.GetIterator();
				if(prop.NextVisible(true))
				{
					do
					{
						if(prop.name == "m_Script")
							continue;
						var subProp = serializedObject.FindProperty(prop.name);
						float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
						totalHeight += height;
					}
					while(prop.NextVisible(false));
				}
				// Add a tiny bit of height if open for the background
				totalHeight += EditorGUIUtility.standardVerticalSpacing;
				serializedObject.Dispose();
			}
			return totalHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{

			EditorGUI.BeginProperty(position, label, property);
			var type = GetFieldType();

			if(type == null || _ignoreClassFullNames.Contains(type.FullName))
			{
				EditorGUI.PropertyField(position, property, label);
				EditorGUI.EndProperty();
				return;
			}

			var guiContent = new GUIContent(property.displayName);
			var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
			if(property.objectReferenceValue != null && AreAnySubPropertiesVisible(property))
			{
				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true);
			}
			else
			{
				// So yeah having a foldout look like a label is a weird hack 
				// but both code paths seem to need to be a foldout or 
				// the object field control goes weird when the codepath changes.
				// I guess because foldout is an interactable control of its own and throws off the controlID?
				foldoutRect.x += 13;
				EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true, EditorStyles.label);
			}
			var indentedPosition = EditorGUI.IndentedRect(position);
			var indentOffset = indentedPosition.x - position.x - 3;
			var propertyRect = new Rect(position.x + (EditorGUIUtility.labelWidth - indentOffset), position.y, position.width - (EditorGUIUtility.labelWidth - indentOffset), EditorGUIUtility.singleLineHeight);

			EditorGUI.ObjectField(propertyRect, property, type, GUIContent.none);

			ExtendScriptableObjectAttribute showScriptableObject = attribute as ExtendScriptableObjectAttribute;
			bool guiWasEnabled = GUI.enabled;
			GUI.enabled = showScriptableObject.Editable;

			if(GUI.changed)
				property.serializedObject.ApplyModifiedProperties();

			if(property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
			{
				var data = (ScriptableObject)property.objectReferenceValue;

				if(property.isExpanded)
				{
					// Draw a background that shows us clearly which fields are part of the ScriptableObject
					GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

					EditorGUI.indentLevel++;
					SerializedObject serializedObject = new(data);

					// Iterate over all the values and draw them
					SerializedProperty prop = serializedObject.GetIterator();
					float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					if(prop.NextVisible(true))
					{
						do
						{
							// Don't bother drawing the class file
							if(prop.name == "m_Script")
								continue;
							float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
							EditorGUI.PropertyField(new Rect(position.x + 1, y, position.width, height), prop, true);
							y += height + EditorGUIUtility.standardVerticalSpacing;
						}
						while(prop.NextVisible(false));
					}
					if(GUI.changed)
						serializedObject.ApplyModifiedProperties();
					serializedObject.Dispose();
					EditorGUI.indentLevel--;
				}
			}
			property.serializedObject.ApplyModifiedProperties();
			EditorGUI.EndProperty();

			GUI.enabled = guiWasEnabled;
		}

		public static T GUILayout<T>(string label, T objectReferenceValue, ref bool isExpanded) where T : ScriptableObject
		{
			return GUILayout<T>(new GUIContent(label), objectReferenceValue, ref isExpanded);
		}

		public static T GUILayout<T>(GUIContent label, T objectReferenceValue, ref bool isExpanded) where T : ScriptableObject
		{
			Rect position = EditorGUILayout.BeginVertical();

			var guiContent = label;
			var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
			if(objectReferenceValue != null)
			{
				isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, guiContent, true);
			}
			else
			{
				// So yeah having a foldout look like a label is a weird hack 
				// but both code paths seem to need to be a foldout or 
				// the object field control goes weird when the codepath changes.
				// I guess because foldout is an interactable control of its own and throws off the controlID?
				foldoutRect.x += 12;
				EditorGUI.Foldout(foldoutRect, isExpanded, guiContent, true, EditorStyles.label);
			}

			EditorGUILayout.BeginHorizontal();
			objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent(" "), objectReferenceValue, typeof(T), false) as T;

			EditorGUILayout.EndHorizontal();
			if(objectReferenceValue != null)
			{
				if(isExpanded)
				{
					DrawScriptableObjectChildFields(objectReferenceValue);
				}
			}
			EditorGUILayout.EndVertical();
			return objectReferenceValue;
		}

		static void DrawScriptableObjectChildFields<T>(T objectReferenceValue) where T : ScriptableObject
		{
			// Draw a background that shows us clearly which fields are part of the ScriptableObject
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical(GUI.skin.box);

			var serializedObject = new SerializedObject(objectReferenceValue);
			// Iterate over all the values and draw them
			SerializedProperty prop = serializedObject.GetIterator();
			if(prop.NextVisible(true))
			{
				do
				{
					// Don't bother drawing the class file
					if(prop.name == "m_Script")
						continue;
					EditorGUILayout.PropertyField(prop, true);
				}
				while(prop.NextVisible(false));
			}
			if(GUI.changed)
				serializedObject.ApplyModifiedProperties();
			serializedObject.Dispose();
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
		}

		public static T DrawScriptableObjectField<T>(GUIContent label, T objectReferenceValue, ref bool isExpanded) where T : ScriptableObject
		{
			Rect position = EditorGUILayout.BeginVertical();

			var guiContent = label;
			var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
			if(objectReferenceValue != null)
			{
				isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, guiContent, true);
			}
			else
			{
				// So yeah having a foldout look like a label is a weird hack 
				// but both code paths seem to need to be a foldout or 
				// the object field control goes weird when the codepath changes.
				// I guess because foldout is an interactable control of its own and throws off the controlID?
				foldoutRect.x += 12;
				EditorGUI.Foldout(foldoutRect, isExpanded, guiContent, true, EditorStyles.label);
			}

			EditorGUILayout.BeginHorizontal();
			objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent(" "), objectReferenceValue, typeof(T), false) as T;

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			return objectReferenceValue;
		}

		Type GetFieldType()
		{
			Type type = fieldInfo.FieldType;
			if(type.IsArray)
				type = type.GetElementType();
			else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
				type = type.GetGenericArguments()[0];
			return type;
		}

		static bool AreAnySubPropertiesVisible(SerializedProperty property)
		{
			var data = (ScriptableObject)property.objectReferenceValue;
			SerializedObject serializedObject = new(data);
			SerializedProperty prop = serializedObject.GetIterator();
			while(prop.NextVisible(true))
			{
				if(prop.name == "m_Script")
					continue;
				return true; //if theres any visible property other than m_script
			}
			serializedObject.Dispose();
			return false;
		}
	}
}
