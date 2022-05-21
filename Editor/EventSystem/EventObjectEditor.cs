using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

[CustomEditor(typeof(EventObject))]
public class EventObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Rect rect = EditorGUILayout.GetControlRect();

        EditorGUI.PropertyField(rect, serializedObject.FindProperty("queueable"));
        
        if (serializedObject.FindProperty("queueable").boolValue)
        {
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, rect.height), serializedObject.FindProperty("dequeueLayer"));
        }
    }
}
