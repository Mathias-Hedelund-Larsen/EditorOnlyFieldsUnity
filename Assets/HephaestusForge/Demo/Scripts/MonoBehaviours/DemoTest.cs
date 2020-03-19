using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var editorFieldsDataController = UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(
            UnityEditor.AssetDatabase.FindAssets("t:EditorFieldsDataController")[0]), typeof(ScriptableObject));

        Type editorFieldsDataControllerType = editorFieldsDataController.GetType();

        var getValueInEditorPlaymodeMethod = editorFieldsDataControllerType.
            GetMethod("GetValueInEditorPlayMode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var getFieldInEditorPlaymodeMethod = editorFieldsDataControllerType.
            GetMethod("GetFieldInEditorPlaymode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        var someBoolField = getFieldInEditorPlaymodeMethod.Invoke(editorFieldsDataController, new object[] { "d345c319699245942bde79f8225bc3ec", "SomeBool", typeof(bool) });

        var fieldValueReference = someBoolField.GetType().GetProperty("FieldValue", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        fieldValueReference.SetValue(someBoolField, true);

        _stringEditorOnly = (string)getValueInEditorPlaymodeMethod.Invoke(editorFieldsDataController, new object[] { "d345c319699245942bde79f8225bc3ec", "EditorString" });
        _vector2EditorOnly = (Vector2)getValueInEditorPlaymodeMethod.Invoke(editorFieldsDataController, new object[] { "d345c319699245942bde79f8225bc3ec", "EditorVector2" });
#endif
    }
}
