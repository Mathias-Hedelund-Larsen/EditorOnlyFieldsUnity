using HephaestusForge.EditorFieldOnly;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Test))]
public class TestPropertyDrawer : BaseEditorFieldOnlyPropertyDrawer
{
    protected override void Init()
    {
        RequestBoolField("EditorTestBool", enableFieldAvailabilityForEditorPlayMode: true);
    }

    protected override int GetPropertyHeight(SerializedProperty[] children)
    {
        return 0;
    }

    protected override void OnGUI(Rect position, SerializedProperty[] children)
    {
        
    }    
}
