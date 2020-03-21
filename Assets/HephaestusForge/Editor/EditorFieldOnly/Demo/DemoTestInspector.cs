using UnityEditor;
using HephaestusForge.EditorFieldOnly;

[CustomEditor(typeof(Random.DemoTest))]
public sealed class DemoTestInspector : BaseEditorFieldOnlyInspector
{
    protected override void CustomDraw(SerializedProperty serializedProperty)
    {
        throw new System.NotImplementedException();
    }

    protected override void Enabled(out bool didRequestEditorField)
    {
        didRequestEditorField = true;
    }
}
