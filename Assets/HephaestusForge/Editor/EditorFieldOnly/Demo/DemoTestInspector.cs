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

        _fieldsAvailableAtEditorRunTime = true;
        RequestBoolField("EditorBoolField");
        RequestBoolCollectionField("BoolCollectionInEditor");
    }
}
