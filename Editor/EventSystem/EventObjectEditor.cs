using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

[CustomEditor(typeof(EventObject))]
public class EventObjectEditor : Editor
{
    SerializedProperty queuable;
    SerializedProperty global;

    private void OnEnable()
    {
        queuable = serializedObject.FindProperty("queueable");
        global = serializedObject.FindProperty("global");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(global);
        EditorGUILayout.PropertyField(queuable);
        
        if (queuable.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dequeueLayer"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
