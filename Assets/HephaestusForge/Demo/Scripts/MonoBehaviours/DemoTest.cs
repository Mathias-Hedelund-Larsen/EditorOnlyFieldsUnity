using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DemoTest : MonoBehaviour
{

#if UNITY_EDITOR

    [NonSerialized]
    private string _stringEditorOnly;

    [NonSerialized]
    private Vector2 _vector2EditorOnly;

#endif


    private void Awake()
    {
#if UNITY_EDITOR
        Type editorFieldsDataControllerType = Type.GetType("HephaestusForge.EditorFieldOnly.EditorFieldsDataController, Assembly-CSharp-Editor");
        var editorFieldsDataController = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:EditorFieldsDataController")[0]),
            editorFieldsDataControllerType);

        var getValueOnEditorRuntimeMethod = editorFieldsDataControllerType.GetMethod("GetValueInEditorPlayMode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        _stringEditorOnly = (string)getValueOnEditorRuntimeMethod.Invoke(editorFieldsDataController, new object[] { "1548a1c5171ffde43a8bdfecc72ce1ff", "EditorString" });
        _vector2EditorOnly = (Vector2)getValueOnEditorRuntimeMethod.Invoke(editorFieldsDataController, new object[] { "1548a1c5171ffde43a8bdfecc72ce1ff", "EditorVector2" });
#endif
    }
}
