using System;
using UnityEditor;

namespace HephaestusForge.EditorFieldOnly
{
    public sealed class EditorFieldDrawingCriteria
    {
        public string FieldName { get; }
        public SerializedProperty SerializedProperty { get; }
        public bool EnableFieldAvailabilityForEditorPlayMode { get; }
        public bool VisibleAtEditorEditTime { get; }
        public bool VisibleAtEditorPlayMode { get; }
        public Action<SerializedProperty> OnDraw { get; }

        public EditorFieldDrawingCriteria(string fieldName, SerializedProperty serializedProperty, bool enableFieldAvailabilityForEditorPlayMode,
            bool visibleAtEditorEditTime, bool visibleAtEditorPlayMode, Action<SerializedProperty> onDraw)
        {
            FieldName = fieldName;
            SerializedProperty = serializedProperty;
            EnableFieldAvailabilityForEditorPlayMode = enableFieldAvailabilityForEditorPlayMode;
            VisibleAtEditorEditTime = visibleAtEditorEditTime;
            VisibleAtEditorPlayMode = visibleAtEditorPlayMode;
            OnDraw = onDraw;
        }
    }
}