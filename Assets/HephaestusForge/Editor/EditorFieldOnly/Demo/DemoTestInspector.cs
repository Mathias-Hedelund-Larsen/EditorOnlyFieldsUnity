using HephaestusForge.EditorFieldOnly;
using UnityEditor;

[CustomEditor(typeof(DemoTest))]
public sealed class DemoTestInspector : BaseEditorFieldOnlyInspector
{
    protected override void Enabled(out bool didRequestEditorField)
    {
        RequestBoolField("SomeBool", enableFieldAvailabilityForEditorPlayMode: true, visibleAtEditorPlayMode: true);
        RequestBoolField("EditorBoolField", visibleAtEditorPlayMode: true, enableFieldAvailabilityForEditorPlayMode: true);
        RequestBoolCollectionField("BoolCollectionInEditor", enableFieldAvailabilityForEditorPlayMode: true);
        RequestFloatField("EditorFloat", enableFieldAvailabilityForEditorPlayMode: true);
        RequestFloatCollectionField("EditorFloatCollection", enableFieldAvailabilityForEditorPlayMode: true);
        RequestIntField("EditorInt", enableFieldAvailabilityForEditorPlayMode: true);
        RequestIntCollectionField("EditorIntCollection", enableFieldAvailabilityForEditorPlayMode: true);
        RequestStringField("EditorString", enableFieldAvailabilityForEditorPlayMode: true);
        RequestStringCollectionField("EditorStringCollection", enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector2Field("EditorVector2", enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector2CollectionField("EditorVector2Collection", enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector2IntField("EditorVector2Int", visibleAtEditorPlayMode: true, enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector2IntCollectionField("EditorVector2IntCollection", enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector3Field("EditorVector3", enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector3CollectionField("EditorVector3Collection", enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector3IntField("EditorVector3Int", enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector3IntCollectionField("EdtorVector3IntCollection", enableFieldAvailabilityForEditorPlayMode: true);

        didRequestEditorField = true;
    }
}
