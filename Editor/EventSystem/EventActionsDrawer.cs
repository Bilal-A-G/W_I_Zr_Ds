using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(EventActions))]
public class EventActionsDrawer : PropertyDrawer
{
    float propertyHeight = 20;
    float buttonDistance = 5;
    float scale = 5;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty events = property.FindPropertyRelative("events");
        SerializedProperty actions = property.FindPropertyRelative("actions");

        ShowList(events, new Rect(position.x - position.x/3.2f, position.y, position.width / 2.1f, position.height), true);
        ShowList(actions, new Rect(position.x + position.width / 2f, position.y, position.width / 2f, position.height), false);

        EditorGUI.EndProperty();
    }

    void ShowList(SerializedProperty list, Rect position, bool events)
    {
        bool dropdown = EditorGUI.PropertyField(position, list, false);

        if (dropdown)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + i * propertyHeight + propertyHeight, position.width, propertyHeight), list.GetArrayElementAtIndex(i), GUIContent.none);
            }

            if (GUI.Button(new Rect(position.x, position.y + list.arraySize * propertyHeight + propertyHeight + buttonDistance, position.width/scale, propertyHeight), "+"))
            {
                list.InsertArrayElementAtIndex(list.arraySize);
            }

            if(GUI.Button(new Rect(position.x + position.width/scale, position.y + list.arraySize * propertyHeight + propertyHeight + buttonDistance, position.width/scale, propertyHeight), "-"))
            {
                list.DeleteArrayElementAtIndex(list.arraySize - 1);
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float startingLines = 5;
        float eventLines = 0;
        float actionLines = 0;

        float greaterLines;

        SerializedProperty events = property.FindPropertyRelative("events");
        SerializedProperty actions = property.FindPropertyRelative("actions");

        for (int i = 0; i < actions.arraySize - 1; i++)
        {
            actionLines++;
        }

        for (int i = 0; i < events.arraySize - 1; i++)
        {
            eventLines++;
        }


        if (eventLines >= startingLines - 4 && eventLines > actionLines)
        {
            greaterLines = eventLines - 1;
        }
        else
        {
            greaterLines = actionLines - 1;
        }

        return EditorGUIUtility.singleLineHeight * (startingLines + greaterLines) + EditorGUIUtility.standardVerticalSpacing * ((startingLines + greaterLines) - 1);
    }
}