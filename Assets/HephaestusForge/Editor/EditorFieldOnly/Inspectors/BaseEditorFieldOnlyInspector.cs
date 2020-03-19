using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HephaestusForge.EditorFieldOnly
{
    public abstract class BaseEditorFieldOnlyInspector : Editor
    {
        public const string INT_ARRAY = "IntArray";
        public const string BOOL_ARRAY = "BoolArray";
        public const string FLOAT_ARRAY = "FloatArray";
        public const string STRING_ARRAY = "StringArray";
        public const string VECTOR2_ARRAY = "Vector2Array";
        public const string VECTOR3_ARRAY = "Vector3Array";
        public const string OPEN_BRACKET_REPLACEMENT = "@#%&";
        public const string VERTICAL_LINE_REPLACEMENT = "!#@%";
        public const string CLOSED_BRACKET_REPLACEMENT = "&%#@";
        public const string VECTOR2INT_ARRAY = "Vector2IntArray";
        public const string VECTOR3INT_ARRAY = "Vector3IntArray";
        public const string FIELDS_DIRECTORY = "HephaestusForge/EditorFieldOnly";

        public static SerializedObject _EditorFieldsDataController;

        private string _guid;
        private int _objectID;
        private string _filePath;
        private string _sceneGuid;
        private List<string> _fileDataList;
        private List<string> _fieldNames = new List<string>();

        protected MonoScript _script;
        protected bool _shouldDrawBaseInspector = true;
        protected List<EditorFieldDrawingCriteria> _requestedProperties = new List<EditorFieldDrawingCriteria>();

        private void OnEnable()
        {
            _script = target.GetScript();
            target.GetSceneGuidAndObjectID(out _sceneGuid, out _objectID);            

            if (_EditorFieldsDataController == null)
            {
                _EditorFieldsDataController = new SerializedObject(AssetDatabase.LoadAssetAtPath<EditorFieldsDataController>(
                    AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:EditorFieldsDataController")[0])));
            }

            _fieldNames.Add("FileID");

            Enabled(out bool didRequestEditorField);

            if (didRequestEditorField)
            {
                AssetDatabase.SaveAssets();
            }
        }     

        protected abstract void Enabled(out bool didRequestEditorField);

        #region FieldRequestsAndSetup
        protected SerializedProperty RequestBoolField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var boolFields = _EditorFieldsDataController.FindProperty("_boolFields");
                var serializedBoolParent = SearchForPropertyInArray(boolFields, fieldName);

                if (serializedBoolParent == null)
                {
                    serializedBoolParent = IncreaseArray(boolFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedBool = serializedBoolParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedBoolParent, fieldName, serializedBool.propertyType.ToString());
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedBool, enableFieldAvailabilityForEditorPlayMode,
                    visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedBool;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestBoolCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var boolCollectionFields = _EditorFieldsDataController.FindProperty("_boolCollectionFields");

                var serializedBoolCollectionParent = SearchForPropertyInArray(boolCollectionFields, fieldName);

                if (serializedBoolCollectionParent == null)
                {
                    serializedBoolCollectionParent = IncreaseArray(boolCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedBoolCollection = serializedBoolCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedBoolCollectionParent, fieldName, BOOL_ARRAY);
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedBoolCollection, enableFieldAvailabilityForEditorPlayMode,
                    visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedBoolCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestFloatField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var floatFields = _EditorFieldsDataController.FindProperty("_floatFields");

                var serializedFloatParent = SearchForPropertyInArray(floatFields, fieldName);

                if (serializedFloatParent == null)
                {
                    serializedFloatParent = IncreaseArray(floatFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedFloat = serializedFloatParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedFloatParent, fieldName, serializedFloat.propertyType.ToString());
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedFloat, enableFieldAvailabilityForEditorPlayMode,
                        visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedFloat;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestFloatCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var floatCollectionFields = _EditorFieldsDataController.FindProperty("_floatCollectionFields");

                var serializedFloatCollectionParent = SearchForPropertyInArray(floatCollectionFields, fieldName);

                if (serializedFloatCollectionParent == null)
                {
                    serializedFloatCollectionParent = IncreaseArray(floatCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedFloatCollection = serializedFloatCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedFloatCollectionParent, fieldName, FLOAT_ARRAY);
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedFloatCollection, enableFieldAvailabilityForEditorPlayMode,
                           visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedFloatCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestIntField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var intFields = _EditorFieldsDataController.FindProperty("_intFields");

                var serializedIntParent = SearchForPropertyInArray(intFields, fieldName);

                if (serializedIntParent == null)
                {
                    serializedIntParent = IncreaseArray(intFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedInt = serializedIntParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedIntParent, fieldName, serializedInt.propertyType.ToString());
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedInt, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedInt;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestIntCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var intCollectionFields = _EditorFieldsDataController.FindProperty("_intCollectionFields");

                var serializedIntCollectionParent = SearchForPropertyInArray(intCollectionFields, fieldName);

                if (serializedIntCollectionParent == null)
                {
                    serializedIntCollectionParent = IncreaseArray(intCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedIntCollection = serializedIntCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedIntCollectionParent, fieldName, INT_ARRAY);
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedIntCollection, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedIntCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestStringField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var stringFields = _EditorFieldsDataController.FindProperty("_stringFields");

                var serializedStringParent = SearchForPropertyInArray(stringFields, fieldName);

                if (serializedStringParent == null)
                {
                    serializedStringParent = IncreaseArray(stringFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedString = serializedStringParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedStringParent, fieldName, serializedString.propertyType.ToString());
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedString, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedString;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestStringCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var stringCollectionFields = _EditorFieldsDataController.FindProperty("_stringCollectionFields");

                var serializedStringCollectionParent = SearchForPropertyInArray(stringCollectionFields, fieldName);

                if (serializedStringCollectionParent == null)
                {
                    serializedStringCollectionParent = IncreaseArray(stringCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedStringcollection = serializedStringCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedStringCollectionParent, fieldName, STRING_ARRAY);
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedStringcollection, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedStringcollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector2Field(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var vector2Fields = _EditorFieldsDataController.FindProperty("_vector2Fields");

                var serializedVector2Parent = SearchForPropertyInArray(vector2Fields, fieldName);

                if (serializedVector2Parent == null)
                {
                    serializedVector2Parent = IncreaseArray(vector2Fields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector2 = serializedVector2Parent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector2Parent, fieldName, serializedVector2.propertyType.ToString());
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedVector2, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedVector2;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector2CollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var vector2CollectionFields = _EditorFieldsDataController.FindProperty("_vector2CollectionFields");

                var serializedVector2CollectionParent = SearchForPropertyInArray(vector2CollectionFields, fieldName);

                if (serializedVector2CollectionParent == null)
                {
                    serializedVector2CollectionParent = IncreaseArray(vector2CollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector2Collection = serializedVector2CollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector2CollectionParent, fieldName, VECTOR2_ARRAY);
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedVector2Collection, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedVector2Collection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector2IntField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var vector2IntFields = _EditorFieldsDataController.FindProperty("_vector2IntFields");

                var serializedVector2IntParent = SearchForPropertyInArray(vector2IntFields, fieldName);

                if (serializedVector2IntParent == null)
                {
                    serializedVector2IntParent = IncreaseArray(vector2IntFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector2Int = serializedVector2IntParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector2IntParent, fieldName, serializedVector2Int.propertyType.ToString());
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedVector2Int, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedVector2Int;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector2IntCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var vector2IntCollectionFields = _EditorFieldsDataController.FindProperty("_vector2IntCollectionFields");

                var serializedVector2IntCollectionParent = SearchForPropertyInArray(vector2IntCollectionFields, fieldName);

                if (serializedVector2IntCollectionParent == null)
                {
                    serializedVector2IntCollectionParent = IncreaseArray(vector2IntCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector2IntCollection = serializedVector2IntCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector2IntCollectionParent, fieldName, VECTOR2INT_ARRAY);
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedVector2IntCollection, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedVector2IntCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector3Field(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var vector3Fields = _EditorFieldsDataController.FindProperty("_vector3Fields");

                var serializedVector3Parent = SearchForPropertyInArray(vector3Fields, fieldName);

                if (serializedVector3Parent == null)
                {
                    serializedVector3Parent = IncreaseArray(vector3Fields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector3 = serializedVector3Parent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector3Parent, fieldName, serializedVector3.propertyType.ToString());
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedVector3, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedVector3;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector3CollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var vector3CollectionFields = _EditorFieldsDataController.FindProperty("_vector3CollectionFields");

                var serializedVector3CollectionParent = SearchForPropertyInArray(vector3CollectionFields, fieldName);

                if (serializedVector3CollectionParent == null)
                {
                    serializedVector3CollectionParent = IncreaseArray(vector3CollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector3Collection = serializedVector3CollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector3CollectionParent, fieldName, VECTOR3_ARRAY);
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedVector3Collection, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedVector3Collection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector3IntField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var vector3IntFields = _EditorFieldsDataController.FindProperty("_vector3IntFields");

                var serializedVector3IntParent = SearchForPropertyInArray(vector3IntFields, fieldName);

                if (serializedVector3IntParent == null)
                {
                    serializedVector3IntParent = IncreaseArray(vector3IntFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector3Int = serializedVector3IntParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector3IntParent, fieldName, serializedVector3Int.propertyType.ToString());
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedVector3Int, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedVector3Int;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector3IntCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false)
        {
            if (!_fieldNames.Contains(fieldName))
            {
                var vector3IntCollectionFields = _EditorFieldsDataController.FindProperty("_vector3IntCollectionFields");

                var serializedVector3IntCollectionParent = SearchForPropertyInArray(vector3IntCollectionFields, fieldName);

                if (serializedVector3IntCollectionParent == null)
                {
                    serializedVector3IntCollectionParent = IncreaseArray(vector3IntCollectionFields, fieldName);
                    _EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector3IntCollection = serializedVector3IntCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector3IntCollectionParent, fieldName, VECTOR3INT_ARRAY);
                }

                _requestedProperties.Add(new EditorFieldDrawingCriteria(fieldName, serializedVector3IntCollection, enableFieldAvailabilityForEditorPlayMode,
                          visibleAtEditorEditTime, visibleAtEditorPlayMode));

                _fieldNames.Add(fieldName);

                return serializedVector3IntCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        private SerializedProperty IncreaseArray(SerializedProperty array, string nameOfField)
        {
            array.arraySize++;

            string fieldID = GUID.Generate().ToString();

            var targetPropertyHolder = array.GetArrayElementAtIndex(array.arraySize - 1);
            targetPropertyHolder.FindPropertyRelative("_fieldName").stringValue = nameOfField;
            targetPropertyHolder.FindPropertyRelative("_sceneGuid").stringValue = _sceneGuid;
            targetPropertyHolder.FindPropertyRelative("_objectID").intValue = _objectID;
            targetPropertyHolder.FindPropertyRelative("_usedInScript").objectReferenceValue = _script;
            targetPropertyHolder.FindPropertyRelative("_fieldID").stringValue = fieldID;
            targetPropertyHolder.FindPropertyRelative("_controller").objectReferenceValue = _EditorFieldsDataController.targetObject;
            targetPropertyHolder.FindPropertyRelative("_guidPath").stringValue = "";

            var allFieldsArray = _EditorFieldsDataController.FindProperty("_allFields");

            allFieldsArray.arraySize++;
            var allFieldsAtNewestIndex = allFieldsArray.GetArrayElementAtIndex(allFieldsArray.arraySize - 1);
            allFieldsAtNewestIndex.FindPropertyRelative("_fieldName").stringValue = nameOfField;
            allFieldsAtNewestIndex.FindPropertyRelative("_sceneGuid").stringValue = _sceneGuid;
            allFieldsAtNewestIndex.FindPropertyRelative("_objectID").intValue = _objectID;
            allFieldsAtNewestIndex.FindPropertyRelative("_usedInScript").objectReferenceValue = _script;
            allFieldsAtNewestIndex.FindPropertyRelative("_fieldID").stringValue = fieldID;
            allFieldsAtNewestIndex.FindPropertyRelative("_guidPath").stringValue = "";

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

        private void EnableFieldForEditorRunTime(SerializedProperty serializedFieldParent, string fieldName, string fieldType)
        {
            var guid = serializedFieldParent.FindPropertyRelative("_guidPath");

            if (!string.IsNullOrEmpty(guid.stringValue))
            {
                _guid = guid.stringValue;
            }

            if (string.IsNullOrEmpty(_guid))
            {
                string foundGUID = (_EditorFieldsDataController.targetObject as EditorFieldsDataController).FindGUID(_sceneGuid, _objectID);

                _guid = (string.IsNullOrEmpty(foundGUID)) ? GUID.Generate().ToString() : foundGUID;
            }

            guid.stringValue = _guid;

            _EditorFieldsDataController.FindProperty("_allFields").FindInArray(s => s.FindPropertyRelative("_sceneGuid").stringValue == _sceneGuid &&
                s.FindPropertyRelative("_objectID").intValue == _objectID && s.FindPropertyRelative("_fieldName").stringValue == fieldName &&
                s.FindPropertyRelative("_usedInScript").objectReferenceValue == _script, out int index).FindPropertyRelative("_guidPath").stringValue = _guid;

            _filePath = $"{Application.persistentDataPath}/{FIELDS_DIRECTORY}/{_guid}.txt";

            if (!Directory.Exists($"{Application.persistentDataPath}/{FIELDS_DIRECTORY}"))
            {
                Directory.CreateDirectory($"{Application.persistentDataPath}/{FIELDS_DIRECTORY}");
            }

            if (!File.Exists(_filePath))
            {
                using (var stream = File.Create(_filePath)) { }
            }

            string fileData = string.Empty;

            using (var reader = new StreamReader(_filePath))
            {
                fileData = reader.ReadToEnd();
            }

            _fileDataList = fileData.Split('\n').ToList();

            for (int i = _fileDataList.Count - 1; i >= 0; i--)
            {
                if (_fileDataList[i] == string.Empty)
                {
                    _fileDataList.RemoveAt(i);
                }
            }

            if (!_fileDataList.Any(s => s.Contains($"{fieldName}:{fieldType}=")))
            {
                using (var writer = new StreamWriter(_filePath))
                {
                    for (int i = 0; i < _fileDataList.Count; i++)
                    {
                        writer.Write($"{_fileDataList[i]}\n");
                    }

                    var fieldValue = GetFieldValue(serializedFieldParent.FindPropertyRelative("_fieldValue"), fieldType);
                    _fileDataList.Add($"{fieldName}:{fieldType}={fieldValue}");
                    writer.Write($"{fieldName}:{fieldType}={fieldValue}\n");
                }
            }

            _EditorFieldsDataController.ApplyModifiedProperties();
        }
        
        #endregion

        public override void OnInspectorGUI()
        {
            if (_shouldDrawBaseInspector)
            {
                base.OnInspectorGUI();
            }

            EditorGUI.BeginChangeCheck();

            for (int i = 0; i < _requestedProperties.Count; i++)
            {
                if (_requestedProperties[i].VisibleAtEditorEditTime && !EditorApplication.isPlaying ||
                    _requestedProperties[i].VisibleAtEditorPlayMode && EditorApplication.isPlaying)
                {
                    EditorGUILayout.PropertyField(_requestedProperties[i].SerializedProperty, new GUIContent(_requestedProperties[i].FieldName), true);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (_requestedProperties.Any(rp => rp.EnableFieldAvailabilityForEditorPlayMode))
                {
                    for (int i = 0; i < _requestedProperties.Count; i++)
                    {
                        if (_requestedProperties[i].EnableFieldAvailabilityForEditorPlayMode)
                        {
                            string fieldName = _requestedProperties[i].FieldName;

                            string field = _fileDataList.Find(s => s.Contains($"{fieldName}:"));
                            int index = _fileDataList.IndexOf(field);

                            var fieldValue = GetFieldValue(_requestedProperties[i].SerializedProperty, field.Split('=')[0].Split(':')[1]);
                            _fileDataList[index] = $"{field.Split('=')[0]}={fieldValue}";
                        }
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

                    EditorUtility.SetDirty(_EditorFieldsDataController.targetObject);
                }
            }

            if (_requestedProperties.Any(rp => rp.EnableFieldAvailabilityForEditorPlayMode))
            {
                if (GUILayout.Button("Open file for fields"))
                {
                    System.Diagnostics.Process.Start(_filePath);
                    GUIUtility.ExitGUI();
                }

                if (GUILayout.Button("Get editor fields file ID"))
                {
                    Debug.Log(_guid);
                    GUIUtility.ExitGUI();
                }
            }
        }

        private string GetFieldValue(SerializedProperty serializedField, string fieldType)
        {
            if (serializedField.isArray)
            {
                if (fieldType == BOOL_ARRAY)
                {
                    List<string> boolValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        boolValues.Add(serializedField.GetArrayElementAtIndex(i).boolValue.ToString());
                    }

                    return $"[{string.Join("|", boolValues)}]";
                }
                else if (fieldType == INT_ARRAY)
                {
                    List<string> intValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        intValues.Add(serializedField.GetArrayElementAtIndex(i).intValue.ToString());
                    }

                    return $"[{string.Join("|", intValues)}]";
                }
                else if (fieldType == FLOAT_ARRAY)
                {
                    List<string> floatValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        floatValues.Add(serializedField.GetArrayElementAtIndex(i).floatValue.ToString());
                    }

                    return $"[{string.Join("|", floatValues)}]";
                }
                else if (fieldType == STRING_ARRAY)
                {
                    List<string> stringValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        string value = serializedField.GetArrayElementAtIndex(i).stringValue.Replace("|", VERTICAL_LINE_REPLACEMENT).
                            Replace("[", OPEN_BRACKET_REPLACEMENT).Replace("]", CLOSED_BRACKET_REPLACEMENT);

                        stringValues.Add(value);
                    }

                    return $"[{string.Join("|", stringValues)}]";
                }
                else if (fieldType == VECTOR2_ARRAY)
                {
                    List<string> vector2Values = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        vector2Values.Add(serializedField.GetArrayElementAtIndex(i).vector2Value.ToString());
                    }

                    return $"[{string.Join("|", vector2Values)}]";
                }
                else if (fieldType == VECTOR2INT_ARRAY)
                {
                    List<string> vector2IntValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        vector2IntValues.Add(serializedField.GetArrayElementAtIndex(i).vector2IntValue.ToString());
                    }

                    return $"[{string.Join("|", vector2IntValues)}]";
                }
                else if (fieldType == VECTOR3_ARRAY)
                {
                    List<string> vector3Values = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        vector3Values.Add(serializedField.GetArrayElementAtIndex(i).vector3Value.ToString());
                    }

                    return $"[{string.Join("|", vector3Values)}]";
                }
                else if (fieldType == VECTOR3INT_ARRAY)
                {
                    List<string> vector3IntValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        vector3IntValues.Add(serializedField.GetArrayElementAtIndex(i).vector3IntValue.ToString());
                    }

                    return $"[{string.Join("|", vector3IntValues)}]";
                }
                else if (fieldType == SerializedPropertyType.String.ToString())
                {
                    return serializedField.stringValue.Replace("|", VERTICAL_LINE_REPLACEMENT).
                            Replace("[", OPEN_BRACKET_REPLACEMENT).Replace("]", CLOSED_BRACKET_REPLACEMENT);
                }
            }
            else
            {
                switch (serializedField.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        return serializedField.intValue.ToString();

                    case SerializedPropertyType.Boolean:
                        return serializedField.boolValue.ToString();

                    case SerializedPropertyType.Float:
                        return serializedField.floatValue.ToString();

                    case SerializedPropertyType.Vector2:
                        return serializedField.vector2Value.ToString();

                    case SerializedPropertyType.Vector3:
                        return serializedField.vector3Value.ToString();

                    case SerializedPropertyType.Vector2Int:
                        return serializedField.vector2IntValue.ToString();

                    case SerializedPropertyType.Vector3Int:
                        return serializedField.vector3IntValue.ToString();

                }
            }

            return "Default";
        }

        protected virtual void OnDisable()
        {
            _fieldNames.Clear();
            _requestedProperties.Clear();
            _fileDataList?.Clear();

            if (_EditorFieldsDataController != null)
            {
                _EditorFieldsDataController.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                _EditorFieldsDataController = null;
            }
        }
    }
}