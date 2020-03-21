using UnityEditor;
using UnityEngine;
using HephaestusForge.EditorFieldOnly;

[CustomPropertyDrawer(typeof(Test))]
public sealed class TestPropertyDrawer : BaseEditorFieldOnlyPropertyDrawer
{
    protected override void Init()
    {
    }

    protected override int GetPropertyHeight(SerializedProperty[] children)
    {
        return 0;
    }

    protected override void OnGUI(Rect position, SerializedProperty[] children)
    {
    }
}
