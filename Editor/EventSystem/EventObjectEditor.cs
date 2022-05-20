using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

[CustomEditor(typeof(EventObject))]
public class EventObjectEditor : Editor
{
    VisualTreeAsset customEditor;
    Button invokeEventButton;

    EventObject currentEventObject;

    private void OnEnable()
    {
        var csScriptPath = AssetDatabase.GUIDToAssetPath("0a79c89479c2ab543965eb1a7013be37");
        var csFileName = System.IO.Path.GetFileNameWithoutExtension(csScriptPath);
        var csDirectory = System.IO.Path.GetDirectoryName(csScriptPath);

        customEditor = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{csDirectory}/{csFileName}.uxml");
        currentEventObject = (EventObject)target;
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement rootVisualElement = new VisualElement();
        customEditor.CloneTree(rootVisualElement);

        invokeEventButton = rootVisualElement.Q<Button>("InvokeEvent");
        PropertyField queueable = rootVisualElement.Q<PropertyField>("Queueable");

        queueable.BindProperty(serializedObject.FindProperty("queueable"));

        invokeEventButton.clicked += OnEventInvoked;

        return rootVisualElement;
    }

    void OnEventInvoked()
    {
        currentEventObject.InvokeInEditor();
    }
}
