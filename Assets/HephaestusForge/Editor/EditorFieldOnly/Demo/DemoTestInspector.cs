using HephaestusForge.EditorFieldOnly;
using UnityEditor;

[CustomEditor(typeof(DemoTest))]
public sealed class DemoTestInspector : BaseEditorFieldOnlyInspector
{
    protected override void CustomDraw(SerializedProperty serializedProperty)
    {
        throw new System.NotImplementedException();
    }

    protected override void Enabled(out bool didRequestEditorField)
    {
        RequestBoolField("EditorOnlyBool", visibleAtEditorPlayMode: true, enableFieldAvailabilityForEditorPlayMode: true);

        RequestVector3IntCollectionField("EdtorOnlyVector3Array", visibleAtEditorPlayMode: true, enableFieldAvailabilityForEditorPlayMode: true);

        didRequestEditorField = true;
    }
}
