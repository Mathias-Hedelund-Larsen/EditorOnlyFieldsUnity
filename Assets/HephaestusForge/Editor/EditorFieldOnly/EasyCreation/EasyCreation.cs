using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HephaestusForge.EditorFieldOnly
{
    public static class EasyCreation
    {
        [MenuItem("Assets/Create/HephaestusForge/EditorOnlyFields/Inherited inspector", false, 0)]
        private static void CreateInheritedInspector()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (path.Length > 0 && path.Split('/').Contains("Editor"))
            {
                List<string> typeNames = new List<string>();
                List<string> displayNames = new List<string>();

                GetUnityObjectInAssemblies(typeNames, displayNames);

                List<string> classText = new List<string>();

                classText.Add("using UnityEditor;");
                classText.Add("using HephaestusForge.EditorFieldOnly;");
                classText.Add("");
                classText.Add("[CustomEditor(typeof({0}))]");
                classText.Add("public sealed class {0}Inspector : BaseEditorFieldOnlyInspector");
                classText.Add("{");
                classText.Add("    protected override void CustomDraw(SerializedProperty serializedProperty)");
                classText.Add("    {");
                classText.Add("        throw new System.NotImplementedException();");
                classText.Add("    }");
                classText.Add("");
                classText.Add("    protected override void Enabled(out bool didRequestEditorField)");
                classText.Add("    {");
                classText.Add("        didRequestEditorField = true;");
                classText.Add("    }");
                classText.Add("}");

                if (Directory.Exists(path))
                {
                    FileCreationWindow.ShowWindow(path, typeNames.ToArray(), displayNames.ToArray(), classText.ToArray(), "Inspector");

                    return;
                }

                var pathSplit = path.Split('/').ToList();
                pathSplit.RemoveAt(pathSplit.Count - 1);
                path = string.Join("/", pathSplit);

                if (Directory.Exists(path))
                {
                    FileCreationWindow.ShowWindow(path, typeNames.ToArray(), displayNames.ToArray(), classText.ToArray(), "Inspector");

                    return;
                }
            }
            else
            {

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("An Inherited inspector needs to be inside an Editor folder.");
#endif

            }
        }

        [MenuItem("Assets/Create/HephaestusForge/EditorOnlyFields/Inherited propertydrawer", false, 0)]
        private static void CreateInheritedPropertyDrawer()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (path.Length > 0 && path.Split('/').Contains("Editor"))
            {
                List<string> typeNames = new List<string>();
                List<string> displayNames = new List<string>();

                GetNoneUnityObjectInAssemblies(typeNames, displayNames);

                List<string> classText = new List<string>();

                classText.Add("using UnityEditor;");
                classText.Add("using UnityEngine;");
                classText.Add("using HephaestusForge.EditorFieldOnly;");
                classText.Add("");
                classText.Add("[CustomPropertyDrawer(typeof({0}))]");
                classText.Add("public sealed class {0}PropertyDrawer : BaseEditorFieldOnlyPropertyDrawer");
                classText.Add("{");
                classText.Add("    protected override void Init()");
                classText.Add("    {");
                classText.Add("    }");
                classText.Add("");
                classText.Add("    protected override int GetPropertyHeight(SerializedProperty[] children)");
                classText.Add("    {");
                classText.Add("        return 0;");
                classText.Add("    }");
                classText.Add("");
                classText.Add("    protected override void OnGUI(Rect position, SerializedProperty[] children)");
                classText.Add("    {");
                classText.Add("    }");
                classText.Add("}");

                if (Directory.Exists(path))
                {
                    FileCreationWindow.ShowWindow(path, typeNames.ToArray(), displayNames.ToArray(), classText.ToArray(), "PropertyDrawer");

                    return;
                }

                var pathSplit = path.Split('/').ToList();
                pathSplit.RemoveAt(pathSplit.Count - 1);
                path = string.Join("/", pathSplit);

                if (Directory.Exists(path))
                {
                    FileCreationWindow.ShowWindow(path, typeNames.ToArray(), displayNames.ToArray(), classText.ToArray(), "PropertyDrawer");

                    return;
                }
            }
            else
            {

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("An Inherited inspector needs to be inside an Editor folder.");
#endif

            }
        }

        private static void GetUnityObjectInAssemblies(List<string> typeNames, List<string> displayNames)
        {
            var assemblyDefinitions = AssetDatabase.FindAssets("t:asmdef");
            List<UnityEngine.Object> assemblyObjects = new List<UnityEngine.Object>();

            for (int i = 0; i < assemblyDefinitions.Length; i++)
            {
                assemblyObjects.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(assemblyDefinitions[i])));
            }

            for (int i = assemblyObjects.Count - 1; i >= 0; i--)
            {
                if (assemblyObjects[i].name.Contains("Editor") || assemblyObjects[i].name.Contains("Tests") || assemblyObjects[i].name.Contains("Analytics"))
                {
                    assemblyObjects.RemoveAt(i);
                }
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Assembly-CSharp") && !a.FullName.Contains("Editor") ||
                assemblyObjects.Any(ao => ao.name.Contains(a.FullName)) || a.FullName.Split('.')[0].Contains("System")).ToArray();

            for (int i = 0; i < assemblies.Length; i++)
            {
                var assemblyClasses = assemblies[i].GetTypes();

                for (int t = 0; t < assemblyClasses.Length; t++)
                {
                    if (!assemblyClasses[t].IsGenericType && !assemblyClasses[t].IsAbstract && !assemblyClasses[t].FullName.Contains('+') &&
                        (assemblyClasses[t].IsSubclassOf(typeof(MonoBehaviour)) || assemblyClasses[t].IsSubclassOf(typeof(ScriptableObject))))
                    {
                        string[] display = assemblyClasses[t].ToString().Split('.');

                        displayNames.Add(display[display.Length - 1]);
                        typeNames.Add($"{assemblyClasses[t].FullName}, {assemblies[i].FullName.Split(',')[0]}");
                    }
                }
            }
        }

        private static void GetNoneUnityObjectInAssemblies(List<string> typeNames, List<string> displayNames)
        {
            var assemblyDefinitions = AssetDatabase.FindAssets("t:asmdef");
            List<UnityEngine.Object> assemblyObjects = new List<UnityEngine.Object>();

            for (int i = 0; i < assemblyDefinitions.Length; i++)
            {
                assemblyObjects.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(assemblyDefinitions[i])));
            }

            for (int i = assemblyObjects.Count - 1; i >= 0; i--)
            {
                if (assemblyObjects[i].name.Contains("Editor") || assemblyObjects[i].name.Contains("Tests") || assemblyObjects[i].name.Contains("Analytics"))
                {
                    assemblyObjects.RemoveAt(i);
                }
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Assembly-CSharp") && !a.FullName.Contains("Editor") ||
                assemblyObjects.Any(ao => ao.name.Contains(a.FullName))).ToArray();

            for (int i = 0; i < assemblies.Length; i++)
            {
                var assemblyClasses = assemblies[i].GetTypes();

                for (int t = 0; t < assemblyClasses.Length; t++)
                {
                    if (!assemblyClasses[t].IsGenericType && !assemblyClasses[t].IsAbstract && !assemblyClasses[t].FullName.Contains('+') &&
                            !assemblyClasses[t].IsSubclassOf(typeof(UnityEngine.Object)) && assemblyClasses[t].IsClass)
                    {
                        string[] display = assemblyClasses[t].ToString().Split('.');

                        displayNames.Add(display[display.Length - 1]);
                        typeNames.Add($"{assemblyClasses[t].FullName}, {assemblies[i].FullName.Split(',')[0]}");
                    }
                }
            }
        }
    }
}