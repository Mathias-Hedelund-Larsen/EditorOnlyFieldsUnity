using HephaestusForge.EditorFieldOnly;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DemoTest))]
public sealed class DemoTestInspector : BaseEditorFieldOnlyInspector
{
    protected override void OnEnable()
    {
        base.OnEnable();

        RequestBoolField("EditorBoolField", visibleAtEditorPlayMode: true);
        RequestBoolCollectionField("BoolCollectionInEditor");
        RequestFloatField("EditorFloat");
        RequestFloatCollectionField("EditorFloatCollection");
        RequestIntField("EditorInt");
        RequestIntCollectionField("EditorIntCollection");
        RequestStringField("EditorString", enableFieldAvailabilityForEditorPlayMode: true);
        RequestStringCollectionField("EditorStringCollection");
        RequestVector2Field("EditorVector2", enableFieldAvailabilityForEditorPlayMode: true);
        RequestVector2CollectionField("EditorVector2Collection");
        RequestVector2IntField("EditorVector2Int", visibleAtEditorPlayMode: true);
        RequestVector2IntCollectionField("EditorVector2IntCollection");
        RequestVector3Field("EditorVector3");
        RequestVector3CollectionField("EditorVector3Collection");
        RequestVector3IntField("EditorVector3Int");
        RequestVector3IntCollectionField("EdtorVector3IntCollection");

        AssetDatabase.SaveAssets();
    }
}
