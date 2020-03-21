using System;
using UnityEngine;

[Serializable]
public sealed class Test 
{
    [SerializeField]
    private int _value;

#if UNITY_EDITOR
    public void GetEditorFields(string path)
    {

#if UNITY_EDITOR
		//Getting a reference to the container of all Editor Only fields.
		var editorFieldsDataController = UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(
			UnityEditor.AssetDatabase.FindAssets("t:EditorFieldsDataController")[0]), typeof(ScriptableObject));

		//Getting the type for of the container.
		Type editorFieldsDataControllerType = editorFieldsDataController.GetType();

		//Using reflection to find the required method to get a given field reference.
		var getFieldReferenceInEditorPlaymodeMethod = editorFieldsDataControllerType.
			GetMethod("GetFieldInEditorPlaymode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

		//Executing method to get the field reference.
		var editorField = getFieldReferenceInEditorPlaymodeMethod.Invoke(editorFieldsDataController, 
			new object[] { "2a25e5da16e917043b0967cb9ced68b0", $"{path}.EditorTestBool", /*The field type*/typeof(bool) });

		//Getting a PropertyInfo to reference the value in the Editor only field, set value or get value for functionality.
		var editorFieldValueReference = editorField.GetType().GetProperty("FieldValue", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
#endif

	}
#endif
      
}
