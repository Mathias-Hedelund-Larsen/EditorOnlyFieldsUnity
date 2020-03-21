using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HephaestusForge
{
    public static class UnityEngineObjectExtensions
    {
        private static MethodInfo _getObjectByInstanceID = typeof(Object).GetMethod("FindObjectFromInstanceID", BindingFlags.NonPublic | BindingFlags.Static);
        private static PropertyInfo _inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void GetSceneGuidAndObjectID(this Object source, out string sceneGuid, out int objectID)
        {
            if (AssetDatabase.Contains(source))
            {
                sceneGuid = "None";
                objectID = source.GetInstanceID();
            }
            else if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabStageUtility.GetCurrentPrefabStage().prefabAssetPath);
                var components = prefab.GetComponents(source.GetType()).ToList();
                components.AddRange(prefab.GetComponentsInChildren(source.GetType()));

                sceneGuid = "None";
                int localID = source.GetLocalID();
                objectID = localID;

                for (int i = 0; i < components.Count; i++)
                {
                    if (localID == components[i].GetLocalID())
                    {
                        objectID = components[i].GetInstanceID();
                    }
                }
            }
            else
            {
                var scene = (source as Component).gameObject.scene;
                sceneGuid = AssetDatabase.AssetPathToGUID(scene.path);

                objectID = source.GetLocalID();

                if (objectID == 0)
                {
                    if (EditorSceneManager.SaveScene(scene))
                    {
                        objectID = source.GetLocalID();
                    }
                }
            }
        }

        public static int GetLocalID(this Object source)
        {
            SerializedObject serializedObject = new SerializedObject(source);
            _inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

            SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");

            return localIdProp.intValue;
        }

        public static MonoScript GetScript(this Object source)
        {
            if (source is MonoBehaviour)
            {
                return MonoScript.FromMonoBehaviour((MonoBehaviour)source);
            }
            else if (source is ScriptableObject)
            {
                return MonoScript.FromScriptableObject((ScriptableObject)source);
            }

            return null;
        }

        public static Object GetObjectByInstanceID(int instanceID)
        {
            return (Object)_getObjectByInstanceID.Invoke(null, new object[] { instanceID });
        }
    }
}
