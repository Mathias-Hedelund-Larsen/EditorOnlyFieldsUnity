using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DemoTest : MonoBehaviour
{
    [SerializeField]
    private Test[] _test;

    [SerializeField]
    private Test _please;


#if UNITY_EDITOR

    [NonSerialized]
    private bool _editorBoolField;

    [NonSerialized]
    private bool[] _bools;

#endif
      

    private void Awake()
    {
#if UNITY_EDITOR
        Type editorFieldsDataControllerType = Type.GetType("HephaestusForge.EditorFieldOnly.EditorFieldsDataController, Assembly-CSharp-Editor");
        var editorFieldsDataController = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:EditorFieldsDataController")[0]), 
            editorFieldsDataControllerType);

        var getValueOnEditorRuntimeMethod = editorFieldsDataControllerType.GetMethod("GetValueOnEditorRuntime", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        _editorBoolField = (bool)getValueOnEditorRuntimeMethod.Invoke(editorFieldsDataController, new object[] { "baa07cbaf537ae646a208e8b3e208b1b", "EditorBoolField" });
        _bools = (bool[])getValueOnEditorRuntimeMethod.Invoke(editorFieldsDataController, new object[] { "baa07cbaf537ae646a208e8b3e208b1b", "BoolCollectionInEditor" });
#endif

        Debug.Log(_editorBoolField);
    }
}
