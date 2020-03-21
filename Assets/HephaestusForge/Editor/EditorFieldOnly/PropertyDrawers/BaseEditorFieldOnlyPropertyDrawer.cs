using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HephaestusForge.EditorFieldOnly
{
    public abstract class BaseEditorFieldOnlyPropertyDrawer : PropertyDrawer
    {
        private string _guid;
        private string _propertyPath;
        private List<string> _fieldNames = new List<string>();
        private List<string> _initialized = new List<string>();

        protected int _objectID;
        private string _filePath;
        protected string _sceneGuid;
        protected MonoScript _script;
        protected float _heightOffset = 3;
        private List<string> _fileDataList;
        protected bool _shouldBaseDraw = true;
        protected bool _shouldBaseDrawPropertyChildren = true;
        protected Dictionary<string, List<EditorFieldDrawingCriteria>> _requestedProperties = new Dictionary<string, List<EditorFieldDrawingCriteria>>();

        #region FieldRequestsAndSetup
        protected SerializedProperty RequestBoolField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var boolFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_boolFields");
                var serializedBoolParent = SearchForPropertyInArray(boolFields, fieldName);

                if (serializedBoolParent == null)
                {
                    serializedBoolParent = IncreaseArray(boolFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedBool = serializedBoolParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedBoolParent, fieldName, serializedBool.propertyType.ToString());
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedBool,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedBool;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestBoolCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var boolCollectionFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_boolCollectionFields");

                var serializedBoolCollectionParent = SearchForPropertyInArray(boolCollectionFields, fieldName);

                if (serializedBoolCollectionParent == null)
                {
                    serializedBoolCollectionParent = IncreaseArray(boolCollectionFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedBoolCollection = serializedBoolCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedBoolCollectionParent, fieldName, BaseEditorFieldOnlyInspector.BOOL_ARRAY);
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedBoolCollection,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedBoolCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestFloatField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var floatFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_floatFields");

                var serializedFloatParent = SearchForPropertyInArray(floatFields, fieldName);

                if (serializedFloatParent == null)
                {
                    serializedFloatParent = IncreaseArray(floatFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedFloat = serializedFloatParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedFloatParent, fieldName, serializedFloat.propertyType.ToString());
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedFloat,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedFloat;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestFloatCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var floatCollectionFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_floatCollectionFields");

                var serializedFloatCollectionParent = SearchForPropertyInArray(floatCollectionFields, fieldName);

                if (serializedFloatCollectionParent == null)
                {
                    serializedFloatCollectionParent = IncreaseArray(floatCollectionFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedFloatCollection = serializedFloatCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedFloatCollectionParent, fieldName, BaseEditorFieldOnlyInspector.FLOAT_ARRAY);
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedFloatCollection,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedFloatCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestIntField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var intFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_intFields");

                var serializedIntParent = SearchForPropertyInArray(intFields, fieldName);

                if (serializedIntParent == null)
                {
                    serializedIntParent = IncreaseArray(intFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedInt = serializedIntParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedIntParent, fieldName, serializedInt.propertyType.ToString());
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedInt,
                     enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedInt;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestIntCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var intCollectionFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_intCollectionFields");

                var serializedIntCollectionParent = SearchForPropertyInArray(intCollectionFields, fieldName);

                if (serializedIntCollectionParent == null)
                {
                    serializedIntCollectionParent = IncreaseArray(intCollectionFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedIntCollection = serializedIntCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedIntCollectionParent, fieldName, BaseEditorFieldOnlyInspector.INT_ARRAY);
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedIntCollection,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedIntCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestStringField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var stringFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_stringFields");

                var serializedStringParent = SearchForPropertyInArray(stringFields, fieldName);

                if (serializedStringParent == null)
                {
                    serializedStringParent = IncreaseArray(stringFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedString = serializedStringParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedStringParent, fieldName, serializedString.propertyType.ToString());
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedString,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedString;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestStringCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var stringCollectionFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_stringCollectionFields");

                var serializedStringCollectionParent = SearchForPropertyInArray(stringCollectionFields, fieldName);

                if (serializedStringCollectionParent == null)
                {
                    serializedStringCollectionParent = IncreaseArray(stringCollectionFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedStringcollection = serializedStringCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedStringCollectionParent, fieldName, BaseEditorFieldOnlyInspector.STRING_ARRAY);
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedStringcollection,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedStringcollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector2Field(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var vector2Fields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_vector2Fields");

                var serializedVector2Parent = SearchForPropertyInArray(vector2Fields, fieldName);

                if (serializedVector2Parent == null)
                {
                    serializedVector2Parent = IncreaseArray(vector2Fields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector2 = serializedVector2Parent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector2Parent, fieldName, serializedVector2.propertyType.ToString());
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedVector2,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedVector2;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector2CollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var vector2CollectionFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_vector2CollectionFields");

                var serializedVector2CollectionParent = SearchForPropertyInArray(vector2CollectionFields, fieldName);

                if (serializedVector2CollectionParent == null)
                {
                    serializedVector2CollectionParent = IncreaseArray(vector2CollectionFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector2Collection = serializedVector2CollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector2CollectionParent, fieldName, BaseEditorFieldOnlyInspector.VECTOR2_ARRAY);
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedVector2Collection,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedVector2Collection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector2IntField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var vector2IntFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_vector2IntFields");

                var serializedVector2IntParent = SearchForPropertyInArray(vector2IntFields, fieldName);

                if (serializedVector2IntParent == null)
                {
                    serializedVector2IntParent = IncreaseArray(vector2IntFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector2Int = serializedVector2IntParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector2IntParent, fieldName, serializedVector2Int.propertyType.ToString());
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedVector2Int,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedVector2Int;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector2IntCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var vector2IntCollectionFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_vector2IntCollectionFields");

                var serializedVector2IntCollectionParent = SearchForPropertyInArray(vector2IntCollectionFields, fieldName);

                if (serializedVector2IntCollectionParent == null)
                {
                    serializedVector2IntCollectionParent = IncreaseArray(vector2IntCollectionFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector2IntCollection = serializedVector2IntCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector2IntCollectionParent, fieldName, BaseEditorFieldOnlyInspector.VECTOR2INT_ARRAY);
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedVector2IntCollection,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedVector2IntCollection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector3Field(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var vector3Fields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_vector3Fields");

                var serializedVector3Parent = SearchForPropertyInArray(vector3Fields, fieldName);

                if (serializedVector3Parent == null)
                {
                    serializedVector3Parent = IncreaseArray(vector3Fields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector3 = serializedVector3Parent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector3Parent, fieldName, serializedVector3.propertyType.ToString());
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedVector3,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedVector3;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector3CollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var vector3CollectionFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_vector3CollectionFields");

                var serializedVector3CollectionParent = SearchForPropertyInArray(vector3CollectionFields, fieldName);

                if (serializedVector3CollectionParent == null)
                {
                    serializedVector3CollectionParent = IncreaseArray(vector3CollectionFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector3Collection = serializedVector3CollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector3CollectionParent, fieldName, BaseEditorFieldOnlyInspector.VECTOR3_ARRAY);
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedVector3Collection,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedVector3Collection;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector3IntField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var vector3IntFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_vector3IntFields");

                var serializedVector3IntParent = SearchForPropertyInArray(vector3IntFields, fieldName);

                if (serializedVector3IntParent == null)
                {
                    serializedVector3IntParent = IncreaseArray(vector3IntFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector3Int = serializedVector3IntParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector3IntParent, fieldName, serializedVector3Int.propertyType.ToString());
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedVector3Int,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

                return serializedVector3Int;
            }
            else
            {
                Debug.LogError($"The fieldname: {fieldName} is already in use please keep the fieldnames unique");
                return null;
            }
        }

        protected SerializedProperty RequestVector3IntCollectionField(string fieldName, bool enableFieldAvailabilityForEditorPlayMode = false,
            bool visibleAtEditorEditTime = true, bool visibleAtEditorPlayMode = false, System.Action<SerializedProperty> onDraw = null)
        {
            if (!_fieldNames.Contains($"{_propertyPath}.{fieldName}"))
            {
                var vector3IntCollectionFields = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_vector3IntCollectionFields");

                var serializedVector3IntCollectionParent = SearchForPropertyInArray(vector3IntCollectionFields, fieldName);

                if (serializedVector3IntCollectionParent == null)
                {
                    serializedVector3IntCollectionParent = IncreaseArray(vector3IntCollectionFields, fieldName);
                    BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                }

                var serializedVector3IntCollection = serializedVector3IntCollectionParent.FindPropertyRelative("_fieldValue");

                if (enableFieldAvailabilityForEditorPlayMode)
                {
                    EnableFieldForEditorRunTime(serializedVector3IntCollectionParent, fieldName, BaseEditorFieldOnlyInspector.VECTOR3INT_ARRAY);
                }

                _requestedProperties[_propertyPath].Add(new EditorFieldDrawingCriteria(fieldName, serializedVector3IntCollection,
                    enableFieldAvailabilityForEditorPlayMode, visibleAtEditorEditTime, visibleAtEditorPlayMode, onDraw));

                _fieldNames.Add($"{_propertyPath}.{fieldName}");

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
            targetPropertyHolder.FindPropertyRelative("_fieldName").stringValue = $"{_propertyPath}.{nameOfField}";
            targetPropertyHolder.FindPropertyRelative("_sceneGuid").stringValue = _sceneGuid;
            targetPropertyHolder.FindPropertyRelative("_objectID").intValue = _objectID;
            targetPropertyHolder.FindPropertyRelative("_usedInScript").objectReferenceValue = _script;
            targetPropertyHolder.FindPropertyRelative("_fieldID").stringValue = fieldID;
            targetPropertyHolder.FindPropertyRelative("_controller").objectReferenceValue = BaseEditorFieldOnlyInspector._EditorFieldsDataController.targetObject;
            targetPropertyHolder.FindPropertyRelative("_guidPath").stringValue = "";

            var allFieldsArray = BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_allFields");

            allFieldsArray.arraySize++;
            var allFieldsAtNewestIndex = allFieldsArray.GetArrayElementAtIndex(allFieldsArray.arraySize - 1);
            allFieldsAtNewestIndex.FindPropertyRelative("_fieldName").stringValue = $"{_propertyPath}.{nameOfField}";
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

                if (fieldName.stringValue == $"{_propertyPath}.{nameOfField}" && sceneGuid.stringValue == _sceneGuid && 
                    objectID.intValue == _objectID && script.objectReferenceValue == _script)
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
                string foundGUID = (BaseEditorFieldOnlyInspector._EditorFieldsDataController.targetObject as EditorFieldsDataController).FindGUID(_sceneGuid, _objectID);

                _guid = (string.IsNullOrEmpty(foundGUID)) ? GUID.Generate().ToString() : foundGUID;
            }

            guid.stringValue = _guid;

            BaseEditorFieldOnlyInspector._EditorFieldsDataController.FindProperty("_allFields").FindInArray(s => s.FindPropertyRelative("_sceneGuid").stringValue == _sceneGuid &&
                s.FindPropertyRelative("_objectID").intValue == _objectID && s.FindPropertyRelative("_fieldName").stringValue == $"{_propertyPath}.{fieldName}" &&
                s.FindPropertyRelative("_usedInScript").objectReferenceValue == _script, out int index).FindPropertyRelative("_guidPath").stringValue = _guid;

            _filePath = $"{Application.persistentDataPath}/{BaseEditorFieldOnlyInspector.FIELDS_DIRECTORY}/{_guid}.txt";

            if (!Directory.Exists($"{Application.persistentDataPath}/{BaseEditorFieldOnlyInspector.FIELDS_DIRECTORY}"))
            {
                Directory.CreateDirectory($"{Application.persistentDataPath}/{BaseEditorFieldOnlyInspector.FIELDS_DIRECTORY}");
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

            if (!_fileDataList.Any(s => s.Contains($"{_propertyPath}.{fieldName}:{fieldType}=")))
            {
                using (var writer = new StreamWriter(_filePath))
                {
                    for (int i = 0; i < _fileDataList.Count; i++)
                    {
                        writer.Write($"{_fileDataList[i]}\n");
                    }

                    var fieldValue = GetFieldValue(serializedFieldParent.FindPropertyRelative("_fieldValue"), fieldType);
                    _fileDataList.Add($"{_propertyPath}.{fieldName}:{fieldType}={fieldValue}");
                    writer.Write($"{_propertyPath}.{fieldName}:{fieldType}={fieldValue}\n");
                }
            }

            BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
        }
        #endregion

        private string GetFieldValue(SerializedProperty serializedField, string fieldType)
        {
            if (serializedField.isArray)
            {
                if (fieldType == BaseEditorFieldOnlyInspector.BOOL_ARRAY)
                {
                    List<string> boolValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        boolValues.Add(serializedField.GetArrayElementAtIndex(i).boolValue.ToString());
                    }

                    return $"[{string.Join("|", boolValues)}]";
                }
                else if (fieldType == BaseEditorFieldOnlyInspector.INT_ARRAY)
                {
                    List<string> intValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        intValues.Add(serializedField.GetArrayElementAtIndex(i).intValue.ToString());
                    }

                    return $"[{string.Join("|", intValues)}]";
                }
                else if (fieldType == BaseEditorFieldOnlyInspector.FLOAT_ARRAY)
                {
                    List<string> floatValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        floatValues.Add(serializedField.GetArrayElementAtIndex(i).floatValue.ToString());
                    }

                    return $"[{string.Join("|", floatValues)}]";
                }
                else if (fieldType == BaseEditorFieldOnlyInspector.STRING_ARRAY)
                {
                    List<string> stringValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        string value = serializedField.GetArrayElementAtIndex(i).stringValue.Replace("|", BaseEditorFieldOnlyInspector.VERTICAL_LINE_REPLACEMENT).
                            Replace("[", BaseEditorFieldOnlyInspector.OPEN_BRACKET_REPLACEMENT).Replace("]", BaseEditorFieldOnlyInspector.CLOSED_BRACKET_REPLACEMENT);

                        stringValues.Add(value);
                    }

                    return $"[{string.Join("|", stringValues)}]";
                }
                else if (fieldType == BaseEditorFieldOnlyInspector.VECTOR2_ARRAY)
                {
                    List<string> vector2Values = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        vector2Values.Add(serializedField.GetArrayElementAtIndex(i).vector2Value.ToString());
                    }

                    return $"[{string.Join("|", vector2Values)}]";
                }
                else if (fieldType == BaseEditorFieldOnlyInspector.VECTOR2INT_ARRAY)
                {
                    List<string> vector2IntValues = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        vector2IntValues.Add(serializedField.GetArrayElementAtIndex(i).vector2IntValue.ToString());
                    }

                    return $"[{string.Join("|", vector2IntValues)}]";
                }
                else if (fieldType == BaseEditorFieldOnlyInspector.VECTOR3_ARRAY)
                {
                    List<string> vector3Values = new List<string>();

                    for (int i = 0; i < serializedField.arraySize; i++)
                    {
                        vector3Values.Add(serializedField.GetArrayElementAtIndex(i).vector3Value.ToString());
                    }

                    return $"[{string.Join("|", vector3Values)}]";
                }
                else if (fieldType == BaseEditorFieldOnlyInspector.VECTOR3INT_ARRAY)
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
                    return serializedField.stringValue.Replace("|", BaseEditorFieldOnlyInspector.VERTICAL_LINE_REPLACEMENT).
                            Replace("[", BaseEditorFieldOnlyInspector.OPEN_BRACKET_REPLACEMENT).Replace("]", BaseEditorFieldOnlyInspector.CLOSED_BRACKET_REPLACEMENT);
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

        private void Init(SerializedProperty property)
        {
            Selection.selectionChanged += OnDisable;

            if (BaseEditorFieldOnlyInspector._EditorFieldsDataController == null)
            {
                BaseEditorFieldOnlyInspector._EditorFieldsDataController = new SerializedObject(AssetDatabase.LoadAssetAtPath<EditorFieldsDataController>(
                    AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:EditorFieldsDataController")[0])));
            }

            var target = property.serializedObject.targetObject;
            _script = target.GetScript();
            target.GetSceneGuidAndObjectID(out _sceneGuid, out _objectID);            

            _requestedProperties.Add(_propertyPath, new List<EditorFieldDrawingCriteria>());

            Init();
        }

        protected abstract void Init();

        protected abstract void OnGUI(Rect position, SerializedProperty[] children);

        protected abstract int GetPropertyHeight(SerializedProperty[] children);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _propertyPath = property.propertyPath;

            if (!_initialized.Contains(_propertyPath))
            {
                _initialized.Add(_propertyPath);
                Init(property);
            }

            float propertyHeight = 0;

            if (_shouldBaseDraw)
            {
                if (property.isExpanded)
                {
                    for (int i = 0; i < _requestedProperties[_propertyPath].Count; i++)
                    {
                        if (_requestedProperties[_propertyPath][i].VisibleAtEditorEditTime && !EditorApplication.isPlaying ||
                                _requestedProperties[_propertyPath][i].VisibleAtEditorPlayMode && EditorApplication.isPlaying)
                        {
                            if (_requestedProperties[_propertyPath][i].SerializedProperty.isArray && _requestedProperties[_propertyPath][i].SerializedProperty.isExpanded)
                            {
                                propertyHeight += (_requestedProperties[_propertyPath][i].SerializedProperty.arraySize + 1) *
                                    (EditorGUIUtility.singleLineHeight + _heightOffset);
                            }

                            propertyHeight += EditorGUIUtility.singleLineHeight + _heightOffset;
                        }
                    }

                    propertyHeight += EditorGUIUtility.singleLineHeight + _heightOffset;

                    var children = property.GetVisibleChildren().ToArray();

                    if (_shouldBaseDrawPropertyChildren)
                    {
                        for (int i = 0; i < children.Length; i++)
                        {
                            propertyHeight += EditorGUIUtility.singleLineHeight + _heightOffset;
                        }
                    }
                    else
                    {
                        GetPropertyHeight(children);
                    }
                }
                else
                {
                    propertyHeight += EditorGUIUtility.singleLineHeight + _heightOffset;
                }
            }

            if (_requestedProperties[_propertyPath].Any(rp => rp.EnableFieldAvailabilityForEditorPlayMode) && property.isExpanded)
            {
                propertyHeight += (EditorGUIUtility.singleLineHeight + _heightOffset) * 2;
            }

            return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _propertyPath = property.propertyPath;

            if (!_initialized.Contains(_propertyPath))
            {
                _initialized.Add(_propertyPath);
                Init(property);
            }

            if (_shouldBaseDraw)
            {
                position.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.BeginChangeCheck();

                EditorGUI.PropertyField(position, property, false);

                if (EditorGUI.EndChangeCheck())
                {
                    if (_requestedProperties[_propertyPath].Any(rp => rp.EnableFieldAvailabilityForEditorPlayMode))
                    {
                        for (int i = 0; i < _requestedProperties[_propertyPath].Count; i++)
                        {
                            if (_requestedProperties[_propertyPath][i].EnableFieldAvailabilityForEditorPlayMode)
                            {
                                string fieldName = _requestedProperties[_propertyPath][i].FieldName;

                                string field = _fileDataList.Find(s => s.Contains($"{fieldName}:"));
                                int index = _fileDataList.IndexOf(field);

                                var fieldValue = GetFieldValue(_requestedProperties[_propertyPath][i].SerializedProperty, field.Split('=')[0].Split(':')[1]);
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
                    }
                    property.serializedObject.ApplyModifiedProperties();
                }

                if (property.isExpanded)
                {                    
                    position.x += 10;
                    position.width -= 10;
                    position.y += EditorGUIUtility.singleLineHeight + _heightOffset;

                    EditorGUI.BeginChangeCheck();

                    for (int i = 0; i < _requestedProperties[_propertyPath].Count; i++)
                    {
                        if (_requestedProperties[_propertyPath][i].VisibleAtEditorEditTime && !EditorApplication.isPlaying ||
                           _requestedProperties[_propertyPath][i].VisibleAtEditorPlayMode && EditorApplication.isPlaying)
                        {
                            if (_requestedProperties[_propertyPath][i].SerializedProperty.isArray)
                            {
                                EditorGUI.PropertyField(position, _requestedProperties[_propertyPath][i].SerializedProperty,
                                    new GUIContent(_requestedProperties[_propertyPath][i].FieldName), true);

                                if (_requestedProperties[_propertyPath][i].SerializedProperty.isExpanded)
                                {
                                    position.y += (_requestedProperties[_propertyPath][i].SerializedProperty.arraySize + 1) *
                                        (EditorGUIUtility.singleLineHeight + _heightOffset);
                                }
                            }
                            else
                            {
                                EditorGUI.PropertyField(position, _requestedProperties[_propertyPath][i].SerializedProperty,
                                    new GUIContent(_requestedProperties[_propertyPath][i].FieldName), false);
                            }

                            _requestedProperties[_propertyPath][i].OnDraw?.Invoke(_requestedProperties[_propertyPath][i].SerializedProperty);
                            position.y += EditorGUIUtility.singleLineHeight + _heightOffset;
                        }
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        EditorUtility.SetDirty(BaseEditorFieldOnlyInspector._EditorFieldsDataController.targetObject);
                    }

                    EditorGUI.BeginChangeCheck();

                    var children = property.GetVisibleChildren().ToArray();

                    if (_shouldBaseDrawPropertyChildren)
                    {
                        for (int i = 0; i < children.Length; i++)
                        {
                            EditorGUI.PropertyField(position, children[i], false);
                            position.y += EditorGUIUtility.singleLineHeight;
                        }
                    }
                    else
                    {
                        OnGUI(position, children);
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }

                if (_requestedProperties[_propertyPath].Any(rp => rp.EnableFieldAvailabilityForEditorPlayMode) && property.isExpanded)
                {                    
                    position.y += 3;

                    if (EditorGUI.DropdownButton(position, new GUIContent("Open file for fields"), FocusType.Keyboard, GUI.skin.button))
                    {
                        System.Diagnostics.Process.Start(_filePath);
                        GUIUtility.ExitGUI();
                    }

                    position.y += EditorGUIUtility.singleLineHeight + _heightOffset;

                    if (EditorGUI.DropdownButton(position, new GUIContent("Get editor fields file ID"), FocusType.Keyboard, GUI.skin.button))
                    {
                        Debug.Log(_guid);
                        GUIUtility.ExitGUI();
                    }
                }
            }
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnDisable;
            _initialized.Clear();
            _fieldNames.Clear();
            _fileDataList?.Clear();

            if (BaseEditorFieldOnlyInspector._EditorFieldsDataController != null)
            {
                BaseEditorFieldOnlyInspector._EditorFieldsDataController.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                BaseEditorFieldOnlyInspector._EditorFieldsDataController = null;
            }
        }
    }
}