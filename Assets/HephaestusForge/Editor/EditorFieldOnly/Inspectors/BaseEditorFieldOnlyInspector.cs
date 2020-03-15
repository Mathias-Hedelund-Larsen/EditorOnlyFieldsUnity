using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HephaestusForge
{
    namespace EditorFieldOnly
    {
        public abstract class BaseEditorFieldOnlyInspector : Editor
        {
            public static SerializedObject _EditorFieldsDataController;

            private int _objectID;
            private string _sceneGuid;
            protected MonoScript _script;
            protected List<Tuple<string, SerializedProperty>> _requestedProperties = new List<Tuple<string, SerializedProperty>>();

            protected virtual void OnEnable()
            {
                if (target is MonoBehaviour)
                {
                    _script = MonoScript.FromMonoBehaviour((MonoBehaviour)target);
                }
                else if(target is ScriptableObject)
                {
                    _script = MonoScript.FromScriptableObject((ScriptableObject)target);
                }

                if (AssetDatabase.Contains(target))
                {
                    _sceneGuid = "None";
                    _objectID = target.GetInstanceID();                    
                }
                else if (PrefabStageUtility.GetCurrentPrefabStage() != null)
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabStageUtility.GetCurrentPrefabStage().prefabAssetPath);
                    var component = prefab.GetComponent(target.GetType());

                    if (!component)
                    {
                        component = prefab.GetComponentInChildren(target.GetType());
                    }

                    _sceneGuid = "None";
                    _objectID = component.GetInstanceID();
                }
                else
                {
                    SerializedObject serializedObject = new SerializedObject(target);
                    PropertyInfo inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
                    inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

                    SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");   //note the misspelling!

                    _sceneGuid = AssetDatabase.AssetPathToGUID((target as Component).gameObject.scene.path);
                    _objectID = localIdProp.intValue;

                    if(_objectID == 0)
                    {
                        EditorSceneManager.SaveScene((target as Component).gameObject.scene);                        
                    }
                }

                if (_EditorFieldsDataController == null)
                {
                    _EditorFieldsDataController = new SerializedObject(AssetDatabase.LoadAssetAtPath<EditorFieldsDataController>(
                        AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:EditorFieldsDataController")[0])));
                }
            }

            protected SerializedProperty RequestBoolField(string fieldName)
            {
                var boolFields = _EditorFieldsDataController.FindProperty("_boolFields");

                var serializedBool = SearchPropertyArray(boolFields, fieldName);

                if (serializedBool == null)
                {
                    serializedBool = IncreaseArray(boolFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedBool));
                return serializedBool;
            }

            protected SerializedProperty RequestBoolCollectionField(string fieldName)
            {
                var boolCollectionFields = _EditorFieldsDataController.FindProperty("_boolCollectionFields");

                var serializedBoolCollection = SearchPropertyArray(boolCollectionFields, fieldName);

                if (serializedBoolCollection == null)
                {
                    serializedBoolCollection = IncreaseArray(boolCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedBoolCollection));
                return serializedBoolCollection;
            }

            protected SerializedProperty RequestFloatField(string fieldName)
            {
                var floatFields = _EditorFieldsDataController.FindProperty("_floatFields");

                var serializedFloat = SearchPropertyArray(floatFields, fieldName);

                if(serializedFloat == null)
                {
                    serializedFloat = IncreaseArray(floatFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedFloat));
                return serializedFloat;
            }

            protected SerializedProperty RequestFloatCollectionField(string fieldName)
            {
                var floatCollectionFields = _EditorFieldsDataController.FindProperty("_floatCollectionFields");

                var serializedFloatCollection = SearchPropertyArray(floatCollectionFields, fieldName);

                if (serializedFloatCollection == null)
                {
                    serializedFloatCollection = IncreaseArray(floatCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedFloatCollection));
                return serializedFloatCollection;
            }

            protected SerializedProperty RequestIntField(string fieldName)
            {
                var intFields = _EditorFieldsDataController.FindProperty("_intFields");

                var serializedInt = SearchPropertyArray(intFields, fieldName);

                if (serializedInt == null)
                {
                    serializedInt = IncreaseArray(intFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedInt));
                return serializedInt;
            }

            protected SerializedProperty RequestIntCollectionField(string fieldName)
            {
                var intCollectionFields = _EditorFieldsDataController.FindProperty("_intCollectionFields");

                var serializedIntCollection = SearchPropertyArray(intCollectionFields, fieldName);

                if (serializedIntCollection == null)
                {
                    serializedIntCollection = IncreaseArray(intCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedIntCollection));
                return serializedIntCollection;
            }

            protected SerializedProperty RequestStringField(string fieldName)
            {
                var stringFields = _EditorFieldsDataController.FindProperty("_stringFields");

                var serializedString = SearchPropertyArray(stringFields, fieldName);

                if (serializedString == null)
                {
                    serializedString = IncreaseArray(stringFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedString));
                return serializedString;
            }

            protected SerializedProperty RequestStringCollectionField(string fieldName)
            {
                var stringCollectionFields = _EditorFieldsDataController.FindProperty("_stringCollectionFields");

                var serializedStringCollection = SearchPropertyArray(stringCollectionFields, fieldName);

                if (serializedStringCollection == null)
                {
                    serializedStringCollection = IncreaseArray(stringCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedStringCollection));
                return serializedStringCollection;
            }           

            protected SerializedProperty RequestVector2Field(string fieldName)
            {
                var vector2Fields = _EditorFieldsDataController.FindProperty("_vector2Fields");

                var serializedVector2 = SearchPropertyArray(vector2Fields, fieldName);

                if (serializedVector2 == null)
                {
                    serializedVector2 = IncreaseArray(vector2Fields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedVector2));
                return serializedVector2;
            }

            protected SerializedProperty RequestVector2CollectionField(string fieldName)
            {
                var vector2CollectionFields = _EditorFieldsDataController.FindProperty("_vector2CollectionFields");

                var serializedVector2Collection = SearchPropertyArray(vector2CollectionFields, fieldName);

                if (serializedVector2Collection == null)
                {
                    serializedVector2Collection = IncreaseArray(vector2CollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedVector2Collection));
                return serializedVector2Collection;
            }

            protected SerializedProperty RequestVector2IntField(string fieldName)
            {
                var vector2IntFields = _EditorFieldsDataController.FindProperty("_vector2IntFields");

                var serializedVector2Int = SearchPropertyArray(vector2IntFields, fieldName);

                if (serializedVector2Int == null)
                {
                    serializedVector2Int = IncreaseArray(vector2IntFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedVector2Int));
                return serializedVector2Int;
            }

            protected SerializedProperty RequestVector2IntCollectionField(string fieldName)
            {
                var vector2IntCollectionFields = _EditorFieldsDataController.FindProperty("_vector2IntCollectionFields");

                var serializedVector2IntCollection = SearchPropertyArray(vector2IntCollectionFields, fieldName);

                if (serializedVector2IntCollection == null)
                {
                    serializedVector2IntCollection = IncreaseArray(vector2IntCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedVector2IntCollection));
                return serializedVector2IntCollection;
            }

            protected SerializedProperty RequestVector3Field(string fieldName)
            {
                var vector3Fields = _EditorFieldsDataController.FindProperty("_vector3Fields");

                var serializedVector3 = SearchPropertyArray(vector3Fields, fieldName);

                if (serializedVector3 == null)
                {
                    serializedVector3 = IncreaseArray(vector3Fields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedVector3));
                return serializedVector3;
            }

            protected SerializedProperty RequestVector3CollectionField(string fieldName)
            {
                var vector3CollectionFields = _EditorFieldsDataController.FindProperty("_vector3CollectionFields");

                var serializedVector3Collection = SearchPropertyArray(vector3CollectionFields, fieldName);

                if (serializedVector3Collection == null)
                {
                    serializedVector3Collection = IncreaseArray(vector3CollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedVector3Collection));
                return serializedVector3Collection;
            }
            protected SerializedProperty RequestVector3IntField(string fieldName)
            {
                var vector3IntFields = _EditorFieldsDataController.FindProperty("_vector3IntFields");

                var serializedVector3Int = SearchPropertyArray(vector3IntFields, fieldName);

                if (serializedVector3Int == null)
                {
                    serializedVector3Int = IncreaseArray(vector3IntFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedVector3Int));
                return serializedVector3Int;
            }

            protected SerializedProperty RequestVector3IntCollectionField(string fieldName)
            {
                var vector3IntCollectionFields = _EditorFieldsDataController.FindProperty("_vector3IntCollectionFields");

                var serializedVector3IntCollection = SearchPropertyArray(vector3IntCollectionFields, fieldName);

                if (serializedVector3IntCollection == null)
                {
                    serializedVector3IntCollection = IncreaseArray(vector3IntCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedVector3IntCollection));
                return serializedVector3IntCollection;
            }

            private SerializedProperty IncreaseArray(SerializedProperty array, string nameOfField)
            {
                array.arraySize++;

                var targetPropertyHolder = array.GetArrayElementAtIndex(array.arraySize - 1);
                targetPropertyHolder.FindPropertyRelative("_fieldName").stringValue = nameOfField;
                targetPropertyHolder.FindPropertyRelative("_sceneGuid").stringValue = _sceneGuid;
                targetPropertyHolder.FindPropertyRelative("_objectID").intValue = _objectID;
                targetPropertyHolder.FindPropertyRelative("_usedInScript").objectReferenceValue = _script;

                return targetPropertyHolder.FindPropertyRelative("_fieldValue");
            }

            private SerializedProperty SearchPropertyArray(SerializedProperty array, string nameOfField)
            {
                for (int i = 0; i < array.arraySize; i++)
                {
                    var fieldName = array.GetArrayElementAtIndex(i).FindPropertyRelative("_fieldName");
                    var sceneGuid = array.GetArrayElementAtIndex(i).FindPropertyRelative("_sceneGuid");
                    var objectID = array.GetArrayElementAtIndex(i).FindPropertyRelative("_objectID");
                    var script = array.GetArrayElementAtIndex(i).FindPropertyRelative("_usedInScript");

                    if (fieldName.stringValue == nameOfField && sceneGuid.stringValue == _sceneGuid && objectID.intValue == _objectID && script.objectReferenceValue == _script)
                    {
                        return array.GetArrayElementAtIndex(i).FindPropertyRelative("_fieldValue");
                    }
                }

                return null;
            }

            public override void OnInspectorGUI()
            {               
                base.OnInspectorGUI();

                EditorGUI.BeginChangeCheck();

                for (int i = 0; i < _requestedProperties.Count; i++)
                {
                    EditorGUILayout.PropertyField(_requestedProperties[i].Item2, new GUIContent(_requestedProperties[i].Item1));
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(_EditorFieldsDataController.targetObject);
                }
            }

            protected virtual void OnDisable()
            {
                if (_EditorFieldsDataController != null)
                {
                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                    _EditorFieldsDataController = null;
                }
            }
        }
    }
}