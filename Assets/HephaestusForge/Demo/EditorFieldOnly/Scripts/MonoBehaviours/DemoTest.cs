using System;
using UnityEngine;

namespace Random
{
    public class DemoTest : MonoBehaviour
    {
#pragma warning disable 0649

        [SerializeField]
        private Test[] _ts;

#pragma warning restore 0649

#if UNITY_EDITOR

        [NonSerialized]
        private bool _editorBool;

        [NonSerialized]
        private Vector3Int[] _editorVector3IntCollection;

#endif

        private void Awake()
        {

#if UNITY_EDITOR
            ////Getting a reference to the container of all Editor Only fields.
            //var editorFieldsDataController = UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(
            //    UnityEditor.AssetDatabase.FindAssets("t:EditorFieldsDataController")[0]), typeof(ScriptableObject));

            ////Getting the type for of the container.
            //Type editorFieldsDataControllerType = editorFieldsDataController.GetType();

            ////Using reflection to find the required method to get a given field reference.
            //var getFieldReferenceInEditorPlaymodeMethod = editorFieldsDataControllerType.
            //    GetMethod("GetFieldInEditorPlaymode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            ////Executing method to get the field reference.
            //var editorField = getFieldReferenceInEditorPlaymodeMethod.Invoke(editorFieldsDataController, 
            //    new object[] { "2a25e5da16e917043b0967cb9ced68b0", "EditorOnlyBool", /*The field type*/typeof(bool) });

            ////Getting a PropertyInfo to reference the value in the Editor only field, set value or get value for functionality.
            //var editorFieldValueReference = editorField.GetType().GetProperty("FieldValue", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            //for (int i = 0; i < _ts.Length; i++)
            //{
            //    _ts[i].GetEditorFields($"_ts.Array.data[{i}]");
            //}
#endif


#if UNITY_EDITOR
            //Getting a reference to the container of all Editor Only fields.
            var editorFieldsDataController = UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(
                UnityEditor.AssetDatabase.FindAssets("t:EditorFieldsDataController")[0]), typeof(ScriptableObject));

            //Getting the type for of the container.
            Type editorFieldsDataControllerType = editorFieldsDataController.GetType();

            //Using reflection to find the required method to get a given field value.
            var getFieldValueInEditorPlaymodeMethod = editorFieldsDataControllerType.
                GetMethod("GetValueInEditorPlayMode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            //Invoking the method to get the field value, get the FileID in the inspector.
            System.Object fieldVal = getFieldValueInEditorPlaymodeMethod.Invoke(editorFieldsDataController, new System.Object[] { "76a0fdee9a07a9f4d9eaef67f7125105", "MyVector" });
#endif


        }
    }
}