using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

public class DemoTest : MonoBehaviour
{
    //[SerializeField]
    //private Test _t;

    [SerializeField]
    private Test[] _ts;

#if UNITY_EDITOR

    [NonSerialized]
    private string _stringEditorOnly;

    [NonSerialized]
    private Vector2 _vector2EditorOnly;

#endif

    private void Awake()
    {
#if UNITY_EDITOR
        var editorFieldsDataController = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:EditorFieldsDataController")[0]),
            typeof(ScriptableObject));

        Type editorFieldsDataControllerType = editorFieldsDataController.GetType();

        var getValueInEditorPlaymodeMethod = editorFieldsDataControllerType.
            GetMethod("GetValueInEditorPlayMode", BindingFlags.Instance | BindingFlags.NonPublic);
        var getFieldInEditorPlaymodeMethod = editorFieldsDataControllerType.
            GetMethod("GetFieldInEditorPlaymode", BindingFlags.Instance | BindingFlags.NonPublic);

        var someBoolField = getFieldInEditorPlaymodeMethod.Invoke(editorFieldsDataController, new object[] { "d345c319699245942bde79f8225bc3ec", "SomeBool", typeof(bool) });

        var fieldValueReference = someBoolField.GetType().GetProperty("FieldValue", BindingFlags.Instance | BindingFlags.Public);
        fieldValueReference.SetValue(someBoolField, true);

        _stringEditorOnly = (string)getValueInEditorPlaymodeMethod.Invoke(editorFieldsDataController, new object[] { "d345c319699245942bde79f8225bc3ec", "EditorString" });
        _vector2EditorOnly = (Vector2)getValueInEditorPlaymodeMethod.Invoke(editorFieldsDataController, new object[] { "d345c319699245942bde79f8225bc3ec", "EditorVector2" });
#endif
    }
}
