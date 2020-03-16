using HephaestusForge.EditorFieldOnly;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Test))]
public class TestPropertyDrawer : BaseEditorFieldOnlyPropertyDrawer
{
    protected override void Init()
    {
        RequestBoolField("EditorTestBool");
    }

    protected override int GetPropertyHeight(SerializedProperty[] property)
    {
        return 0;
    }

    protected override void OnGUI(Rect position, SerializedProperty[] children)
    {
        
    }    
}
