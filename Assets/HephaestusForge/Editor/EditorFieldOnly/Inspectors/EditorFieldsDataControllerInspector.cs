using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HephaestusForge
{
    namespace EditorFieldOnly
    {
        [CustomEditor(typeof(EditorFieldsDataController))]
        public sealed class EditorFieldsDataControllerInspector : Editor
        {
            private MonoScript _script;
            private SerializedObject _serializedTarget;

            private void OnEnable()
            {
                _script = MonoScript.FromScriptableObject((ScriptableObject)target);
                _serializedTarget = new SerializedObject(target);
            }

            public override void OnInspectorGUI()
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField(new GUIContent("Script"), _script, typeof(MonoScript), false);
                GUI.enabled = true;

                var boolFieldsArray = _serializedTarget.FindProperty("_boolFields");
                var floatFieldsArray = _serializedTarget.FindProperty("_floatFields");
                var intFieldsArray = _serializedTarget.FindProperty("_intFields");
                var stringFieldsArray = _serializedTarget.FindProperty("_stringFields");
                var vector2FieldsArray = _serializedTarget.FindProperty("_vector2Fields");
                var vector2IntFieldsArray = _serializedTarget.FindProperty("_vector2IntFields");
                var vector3FieldsArray = _serializedTarget.FindProperty("_vector3Fields");
                var vector3IntFieldsArray = _serializedTarget.FindProperty("_vector3IntFields");

                var boolCollectionFieldsArray = _serializedTarget.FindProperty("_boolCollectionFields");
                var floatCollectionFieldsArray = _serializedTarget.FindProperty("_floatCollectionFields");
                var intCollectionFieldsArray = _serializedTarget.FindProperty("_intCollectionFields");
                var stringCollectionFieldsArray = _serializedTarget.FindProperty("_stringCollectionFields");
                var vector2CollectionFieldsArray = _serializedTarget.FindProperty("_vector2CollectionFields");
                var vector2IntCollectionFieldsArray = _serializedTarget.FindProperty("_vector2IntCollectionFields");
                var vector3CollectionFieldsArray = _serializedTarget.FindProperty("_vector3CollectionFields");
                var vector3IntCollectionFieldsArray = _serializedTarget.FindProperty("_vector3IntCollectionFields");

                List<Scene> openScenes = new List<Scene>();
                List<int> indexesToClear = new List<int>();
                PropertyInfo inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

                for (int sceneIndex = 0; sceneIndex < EditorSceneManager.sceneCount; sceneIndex++)
                {
                    openScenes.Add(EditorSceneManager.GetSceneAt(sceneIndex));
                }

                bool removedSomething = false;

                CheckForRemove(boolFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(boolFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(floatFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(floatFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(intFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(intFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(stringFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(stringFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(vector2FieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(vector2FieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(vector2IntFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(vector2IntFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(vector3FieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(vector3FieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(vector3IntFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(vector3IntFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(boolCollectionFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(boolCollectionFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(floatCollectionFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(floatCollectionFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(intCollectionFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(intCollectionFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(stringCollectionFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(stringCollectionFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(vector2CollectionFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(vector2CollectionFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(vector2IntCollectionFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(vector2IntCollectionFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(vector3CollectionFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(vector3CollectionFieldsArray, openScenes, inspectorModeInfo);

                CheckForRemove(vector3IntCollectionFieldsArray, openScenes, indexesToClear, inspectorModeInfo, ref removedSomething);
                Draw(vector3IntCollectionFieldsArray, openScenes, inspectorModeInfo);

                if (removedSomething)
                {
                    _serializedTarget.ApplyModifiedProperties();
                    AssetDatabase.SaveAssets();
                }

                if (GUILayout.Button("Open folder for available fields"))
                {
                    if (Directory.Exists($"{Application.persistentDataPath}/{BaseEditorFieldOnlyInspector.FIELDS_DIRECTORY}"))
                    {
                        System.Diagnostics.Process.Start($"{Application.persistentDataPath}/{BaseEditorFieldOnlyInspector.FIELDS_DIRECTORY}");
                    }
                    else
                    {
                        System.Diagnostics.Process.Start($"{Application.persistentDataPath}");
                    }
                }
            }

            private void CheckForRemove(SerializedProperty fieldsArray, List<Scene> openScenes, List<int> indexesToClear, PropertyInfo inspectorModeInfo, ref bool removedSomething)
            {
                for (int arrayIndex = fieldsArray.arraySize - 1; arrayIndex >= 0; arrayIndex--)
                {
                    string scenePath = string.Empty;
                    var sceneGuid = fieldsArray.GetArrayElementAtIndex(arrayIndex).FindPropertyRelative("_sceneGuid");
                    var usedInScript = fieldsArray.GetArrayElementAtIndex(arrayIndex).FindPropertyRelative("_usedInScript");
                    var objectID = fieldsArray.GetArrayElementAtIndex(arrayIndex).FindPropertyRelative("_objectID");
                    var fieldName = fieldsArray.GetArrayElementAtIndex(arrayIndex).FindPropertyRelative("_fieldName");

                    bool exists = false;

                    if (sceneGuid.stringValue != "None")
                    {
                        scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid.stringValue);

                        if (openScenes.Any(s => s.path == scenePath))
                        {
                            var rootObjects = openScenes.Find(s => s.path == scenePath).GetRootGameObjects();

                            for (int rootObjectIndex = 0; rootObjectIndex < rootObjects.Length; rootObjectIndex++)
                            {
                                var components = rootObjects[rootObjectIndex].GetComponents((usedInScript.objectReferenceValue as MonoScript).GetClass()).ToList();

                                components.AddRange(rootObjects[rootObjectIndex].GetComponentsInChildren((usedInScript.objectReferenceValue as MonoScript).GetClass()));

                                for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
                                {
                                    if (objectID.intValue == components[componentIndex].GetLocalID())
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (exists)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            exists = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);                            
                        }
                    }
                    else
                    {
                        exists = UnityEngineObjectExtensions.GetObjectByInstanceID(objectID.intValue);
                    }

                    if (!exists)
                    {
                        indexesToClear.Add(arrayIndex);
                    }
                }

                List<int> indexesInAllFieldsToClear = new List<int>();
                var allFieldsProperty = _serializedTarget.FindProperty("_allFields");

                for (int i = 0; i < indexesToClear.Count; i++)
                {
                    if(allFieldsProperty.FindInArray(s => s.FindPropertyRelative("_fieldID").stringValue ==
                        fieldsArray.GetArrayElementAtIndex(indexesToClear[i]).FindPropertyRelative("_fieldID").stringValue, out int indexToDelete) != null)
                    {
                        indexesInAllFieldsToClear.Add(indexToDelete);
                    }

                    removedSomething = true;
                    fieldsArray.DeleteArrayElementAtIndex(indexesToClear[i]);
                }

                indexesInAllFieldsToClear.OrderByDescending(i => i);

                for (int i = 0; i < indexesInAllFieldsToClear.Count; i++)
                {
                    allFieldsProperty.DeleteArrayElementAtIndex(indexesInAllFieldsToClear[i]);
                }

                indexesToClear.Clear();                
            }

            private void Draw(SerializedProperty fieldsArray, List<Scene> openScenes, PropertyInfo inspectorModeInfo)
            {
                EditorGUILayout.PropertyField(fieldsArray, false);

                if (fieldsArray.isExpanded)
                {
                    GUI.enabled = false;
                    fieldsArray.arraySize = EditorGUILayout.IntField(new GUIContent("Size"), fieldsArray.arraySize);
                    GUI.enabled = true;

                    for (int arrayIndex = fieldsArray.arraySize - 1; arrayIndex >= 0; arrayIndex--)
                    {
                        string scenePath = string.Empty;
                        var sceneGuid = fieldsArray.GetArrayElementAtIndex(arrayIndex).FindPropertyRelative("_sceneGuid");
                        var usedInScript = fieldsArray.GetArrayElementAtIndex(arrayIndex).FindPropertyRelative("_usedInScript");
                        var objectID = fieldsArray.GetArrayElementAtIndex(arrayIndex).FindPropertyRelative("_objectID");
                        var fieldName = fieldsArray.GetArrayElementAtIndex(arrayIndex).FindPropertyRelative("_fieldName");

                        EditorGUI.indentLevel++;

                        EditorGUILayout.PropertyField(fieldsArray.GetArrayElementAtIndex(arrayIndex), new GUIContent("Element"), false);

                        if (fieldsArray.GetArrayElementAtIndex(arrayIndex).isExpanded)
                        {
                            if (sceneGuid.stringValue != "None")
                            {
                                scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid.stringValue);

                                if (openScenes.Any(s => s.path == scenePath))
                                {
                                    var rootObjects = openScenes.Find(s => s.path == scenePath).GetRootGameObjects();
                                    bool found = false;

                                    for (int rootObjectIndex = 0; rootObjectIndex < rootObjects.Length; rootObjectIndex++)
                                    {
                                        var components = rootObjects[rootObjectIndex].GetComponents((usedInScript.objectReferenceValue as MonoScript).GetClass()).ToList();

                                        components.AddRange(rootObjects[rootObjectIndex].GetComponentsInChildren((usedInScript.objectReferenceValue as MonoScript).GetClass()));

                                        for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
                                        {
                                            if (objectID.intValue == components[componentIndex].GetLocalID())
                                            {
                                                GUI.enabled = false;

                                                EditorGUILayout.TextField(new GUIContent("FieldName"), fieldName.stringValue);

                                                EditorGUILayout.ObjectField(new GUIContent("Reference"), components[componentIndex],
                                                    (usedInScript.objectReferenceValue as MonoScript).GetClass(), true);

                                                GUI.enabled = true;
                                                found = true;
                                                break;
                                            }
                                        }

                                        if (found)
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    GUI.enabled = false;

                                    EditorGUILayout.TextField(new GUIContent("FieldName"), fieldName.stringValue);

                                    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

                                    if (sceneAsset)
                                    {
                                        EditorGUILayout.ObjectField(new GUIContent("Reference"), sceneAsset, typeof(SceneAsset), true);
                                    }

                                    GUI.enabled = true;
                                }                                
                            }
                            else
                            {
                                UnityEngine.Object obj = UnityEngineObjectExtensions.GetObjectByInstanceID(objectID.intValue);

                                if (obj)
                                {
                                    GUI.enabled = false;
                                    EditorGUILayout.TextField(new GUIContent("FieldName"), fieldName.stringValue);
                                    EditorGUILayout.ObjectField(new GUIContent("Reference"), obj, typeof(UnityEngine.Object), true);
                                    GUI.enabled = true;
                                }
                            }
                        }

                        EditorGUI.indentLevel--;
                    }                   
                }
            }
        }
    }
}