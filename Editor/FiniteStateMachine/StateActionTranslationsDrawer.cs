using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StateActionTranslationPairs))]
public class StateActionTranslationsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty translate = property.FindPropertyRelative("translate");
        SerializedProperty translateTo = property.FindPropertyRelative("translateTo");
        SerializedProperty action = property.FindPropertyRelative("action");
        SerializedProperty isFolded = property.FindPropertyRelative("isFolded");

        if (EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), isFolded.boolValue, label, true))
        {
            isFolded.boolValue = true;
            EditorGUI.indentLevel = 1;

            EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), action);

            EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight), translate);

            if (translate.boolValue)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, position.width, EditorGUIUtility.singleLineHeight), translateTo);
            }
        }
        else
        {
            isFolded.boolValue = false;
        }

        property.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty isFolded = property.FindPropertyRelative("isFolded");
        SerializedProperty translate = property.FindPropertyRelative("translate");

        int spacing;

        if (isFolded.boolValue)
        {
            if (translate.boolValue)
            {
                spacing = 3;
            }
            else
            {
                spacing = 2;
            }

            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * spacing;
        }

        return base.GetPropertyHeight(property, label);

    }
}
