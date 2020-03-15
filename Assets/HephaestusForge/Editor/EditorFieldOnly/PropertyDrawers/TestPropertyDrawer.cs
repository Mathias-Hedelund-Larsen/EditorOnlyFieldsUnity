using HephaestusForge.EditorFieldOnly;
using UnityEditor;

[CustomPropertyDrawer(typeof(Test))]
public class TestPropertyDrawer : BaseEditorFieldOnlyPropertyDrawer
{
    protected override void RequestFields()
    {
        RequestBoolField("EditorTestBool");
    }
}
