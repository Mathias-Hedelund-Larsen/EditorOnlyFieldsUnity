using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            public const string FIELDS_DIRECTORY = "HephaestusForge/EditorFieldOnly";
            public const string BOOLEAN_ARRAY = "BooleanArray";            

            public static SerializedObject _EditorFieldsDataController;

            private string _guid;
            private int _objectID;
            private string _filePath;
            private string _sceneGuid;
            private List<string> _fileDataList;
            private List<string> _fieldNames = new List<string>();

            protected MonoScript _script;
            protected bool _fieldsAvailableAtEditorRunTime = false;
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

                _fieldNames.Add("FileID");
            }

            protected SerializedProperty RequestBoolField(string fieldName)
            {
                if (!_fieldNames.Contains(fieldName))
                {
                    var boolFields = _EditorFieldsDataController.FindProperty("_boolFields");
                    var serializedBoolParent = SearchForPropertyInArray(boolFields, fieldName);

                    if (serializedBoolParent == null)
                    {
                        serializedBoolParent = IncreaseArray(boolFields, fieldName);
                        _EditorFieldsDataController.ApplyModifiedProperties();
                        AssetDatabase.SaveAssets();
                    }

                    var serializedBool = serializedBoolParent.FindPropertyRelative("_fieldValue");

                    _EditorFieldsDataController.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();

                    if (_fieldsAvailableAtEditorRunTime)
                    {
                        EnableFieldForEditorRunTime(serializedBoolParent, fieldName, serializedBool.propertyType.ToString());
                    }

                    _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedBool));
                    return serializedBool;
                }
                else
                {
                    Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                    return null;
                }
            }

            protected SerializedProperty RequestBoolCollectionField(string fieldName)
            {
                if (!_fieldNames.Contains(fieldName))
                {
                    var boolCollectionFields = _EditorFieldsDataController.FindProperty("_boolCollectionFields");

                    var serializedBoolCollectionParent = SearchForPropertyInArray(boolCollectionFields, fieldName);

                    if (serializedBoolCollectionParent == null)
                    {
                        serializedBoolCollectionParent = IncreaseArray(boolCollectionFields, fieldName);
                        _EditorFieldsDataController.ApplyModifiedProperties();
                        AssetDatabase.SaveAssets();
                    }

                    var serializedBoolCollection = serializedBoolCollectionParent.FindPropertyRelative("_fieldValue");


                    if (_fieldsAvailableAtEditorRunTime)
                    {
                        EnableFieldForEditorRunTime(serializedBoolCollectionParent, fieldName, BOOLEAN_ARRAY);
                    }

                    _requestedProperties.Add(new Tuple<string, SerializedProperty>(fieldName, serializedBoolCollection));
                    return serializedBoolCollection;
                }
                else
                {
                    Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                    return null;
                }
            }

            protected SerializedProperty RequestFloatField(string fieldName)
            {
                var floatFields = _EditorFieldsDataController.FindProperty("_floatFields");

                var serializedFloat = SearchForPropertyInArray(floatFields, fieldName);

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

                var serializedFloatCollection = SearchForPropertyInArray(floatCollectionFields, fieldName);

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

                var serializedInt = SearchForPropertyInArray(intFields, fieldName);

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

                var serializedIntCollection = SearchForPropertyInArray(intCollectionFields, fieldName);

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

                var serializedString = SearchForPropertyInArray(stringFields, fieldName);

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

                var serializedStringCollection = SearchForPropertyInArray(stringCollectionFields, fieldName);

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

                var serializedVector2 = SearchForPropertyInArray(vector2Fields, fieldName);

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

                var serializedVector2Collection = SearchForPropertyInArray(vector2CollectionFields, fieldName);

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

                var serializedVector2Int = SearchForPropertyInArray(vector2IntFields, fieldName);

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

                var serializedVector2IntCollection = SearchForPropertyInArray(vector2IntCollectionFields, fieldName);

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

                var serializedVector3 = SearchForPropertyInArray(vector3Fields, fieldName);

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

                var serializedVector3Collection = SearchForPropertyInArray(vector3CollectionFields, fieldName);

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

                var serializedVector3Int = SearchForPropertyInArray(vector3IntFields, fieldName);

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

                var serializedVector3IntCollection = SearchForPropertyInArray(vector3IntCollectionFields, fieldName);

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

                return targetPropertyHolder;
            }

            private SerializedProperty SearchForPropertyInArray(SerializedProperty array, string nameOfField)
            {
                for (int i = 0; i < array.arraySize; i++)
                {
                    var fieldName = array.GetArrayElementAtIndex(i).FindPropertyRelative("_fieldName");
                    var sceneGuid = array.GetArrayElementAtIndex(i).FindPropertyRelative("_sceneGuid");
                    var objectID = array.GetArrayElementAtIndex(i).FindPropertyRelative("_objectID");
                    var script = array.GetArrayElementAtIndex(i).FindPropertyRelative("_usedInScript");

                    if (fieldName.stringValue == nameOfField && sceneGuid.stringValue == _sceneGuid && objectID.intValue == _objectID && script.objectReferenceValue == _script)
                    {
                        return array.GetArrayElementAtIndex(i);
                    }
                }

                return null;
            }

            private void EnableFieldForEditorRunTime(SerializedProperty serializedField, string fieldName, string fieldType)
            {
                var guid = serializedField.FindPropertyRelative("_guidPath");

                if (guid.stringValue != string.Empty)
                {
                    _guid = guid.stringValue;
                }

                if (_guid == null)
                {
                    _guid = GUID.Generate().ToString();
                }

                guid.stringValue = _guid;

                _filePath = $"{Application.persistentDataPath}/{FIELDS_DIRECTORY}/{_guid}.txt";

                if (!Directory.Exists($"{Application.persistentDataPath}/{FIELDS_DIRECTORY}"))
                {
                    Directory.CreateDirectory($"{Application.persistentDataPath}/{FIELDS_DIRECTORY}");
                }


                if (!File.Exists(_filePath))
                {
                    using (var stream = File.Create(_filePath)){}
                }

                string fileData = string.Empty;

                using (var reader = new StreamReader(_filePath))
                {
                    fileData = reader.ReadToEnd();
                }

                _fileDataList = fileData.Split('\n').ToList();

                for (int i = _fileDataList.Count - 1; i >= 0; i--)
                {
                    if(_fileDataList[i] == string.Empty)
                    {
                        _fileDataList.RemoveAt(i);
                    }
                }

                if(!_fileDataList.Any(s => s.Contains($"{fieldName}:{fieldType}=")))
                {
                    using(var writer = new StreamWriter(_filePath))
                    {
                        for (int i = 0; i < _fileDataList.Count; i++)
                        {
                            writer.Write($"{_fileDataList[i]}\n");
                        }

                        _fileDataList.Add($"{fieldName}:{fieldType}=Default");
                        writer.Write($"{fieldName}:{fieldType}=Default\n");
                    }
                }

                _EditorFieldsDataController.ApplyModifiedProperties();
            }

            public override void OnInspectorGUI()
            {               
                base.OnInspectorGUI();

                if (!EditorApplication.isPlaying)
                {
                    EditorGUI.BeginChangeCheck();

                    for (int i = 0; i < _requestedProperties.Count; i++)
                    {
                        EditorGUILayout.PropertyField(_requestedProperties[i].Item2, new GUIContent(_requestedProperties[i].Item1), true);
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (_fieldsAvailableAtEditorRunTime)
                        {
                            for (int i = 0; i < _requestedProperties.Count; i++)
                            {
                                string fieldName = _requestedProperties[i].Item1;

                                string field = _fileDataList.Find(s => s.Contains($"{fieldName}:"));
                                int index = _fileDataList.IndexOf(field);

                                string fieldValue = GetFieldValue(_requestedProperties[i].Item2, field.Split('=')[0].Split(':')[1]);
                                _fileDataList[index] = $"{field.Split('=')[0]}={fieldValue}";
                            }

                            using (var stream = File.Open(_filePath, FileMode.Create))
                            {
                                using (var writer = new StreamWriter(stream))
                                {
                                    for (int i = 0; i < _fileDataList.Count; i++)
                                    {
                                        writer.Write($"{_fileDataList[i]}\n");
                                    }
                                }
                            }
                        }

                        EditorUtility.SetDirty(_EditorFieldsDataController.targetObject);
                    }

                    if (_fieldsAvailableAtEditorRunTime)
                    {
                        if (GUILayout.Button("Get editor fields file guid"))
                        {
                            Debug.Log(_guid);
                        }
                    }
                }
            }

            private string GetFieldValue(SerializedProperty item2, string fieldType)
            {
                if (item2.isArray)
                {
                    if(fieldType == BOOLEAN_ARRAY)
                    {
                        List<string> boolValues = new List<string>();

                        for (int i = 0; i < item2.arraySize; i++)
                        {
                            boolValues.Add(item2.GetArrayElementAtIndex(i).boolValue.ToString());
                        }

                        return $"[{string.Join("|", boolValues)}]";
                    }
                }
                else
                {
                    switch (item2.propertyType)
                    {
                        case SerializedPropertyType.Generic:
                            break;
                        case SerializedPropertyType.Integer:
                            break;
                        case SerializedPropertyType.Boolean:
                            return item2.boolValue.ToString();
                            
                        case SerializedPropertyType.Float:
                            break;
                        case SerializedPropertyType.String:
                            break;
                        case SerializedPropertyType.Color:
                            break;
                        case SerializedPropertyType.ObjectReference:
                            break;
                        case SerializedPropertyType.LayerMask:
                            break;
                        case SerializedPropertyType.Enum:
                            break;
                        case SerializedPropertyType.Vector2:
                            break;
                        case SerializedPropertyType.Vector3:
                            break;
                        case SerializedPropertyType.Vector4:
                            break;
                        case SerializedPropertyType.Rect:
                            break;
                        case SerializedPropertyType.ArraySize:
                            break;
                        case SerializedPropertyType.Character:
                            break;
                        case SerializedPropertyType.AnimationCurve:
                            break;
                        case SerializedPropertyType.Bounds:
                            break;
                        case SerializedPropertyType.Gradient:
                            break;
                        case SerializedPropertyType.Quaternion:
                            break;
                        case SerializedPropertyType.ExposedReference:
                            break;
                        case SerializedPropertyType.FixedBufferSize:
                            break;
                        case SerializedPropertyType.Vector2Int:
                            break;
                        case SerializedPropertyType.Vector3Int:
                            break;
                        case SerializedPropertyType.RectInt:
                            break;
                        case SerializedPropertyType.BoundsInt:
                            break;
                        default:
                            break;
                    }
                }

                return "Default";
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