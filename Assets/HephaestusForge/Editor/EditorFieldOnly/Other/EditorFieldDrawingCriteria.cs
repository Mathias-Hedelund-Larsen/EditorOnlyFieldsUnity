using UnityEditor;

public sealed class EditorFieldDrawingCriteria 
{
    public string FieldName { get; }
    public SerializedProperty SerializedProperty { get; }
    public bool EnableFieldAvailabilityForEditorPlayMode { get; }
    public bool VisibleAtEditorEditTime { get; }
    public bool VisibleAtEditorPlayMode { get; }

    public EditorFieldDrawingCriteria(string fieldName, SerializedProperty serializedProperty, bool enableFieldAvailabilityForEditorPlayMode, 
        bool visibleAtEditorEditTime, bool visibleAtEditorPlayMode)
    {
        FieldName = fieldName;
        SerializedProperty = serializedProperty;
        EnableFieldAvailabilityForEditorPlayMode = enableFieldAvailabilityForEditorPlayMode;
        VisibleAtEditorEditTime = visibleAtEditorEditTime;
        VisibleAtEditorPlayMode = visibleAtEditorPlayMode;
    }
}
