using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

[CustomEditor(typeof(EventObject))]
public class EventObjectEditor : Editor
{
    SerializedProperty queuable;

    private void OnEnable()
    {
        queuable = serializedObject.FindProperty("queueable");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Rect rect = EditorGUILayout.GetControlRect();

        EditorGUI.PropertyField(rect, queuable);
        
        if (queuable.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dequeueLayer"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
