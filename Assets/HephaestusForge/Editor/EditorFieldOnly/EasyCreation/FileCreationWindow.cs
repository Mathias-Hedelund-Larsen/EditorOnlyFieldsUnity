using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HephaestusForge.EditorFieldOnly
{
    public class FileCreationWindow : EditorWindow
    {
        private string _type;
        private string _path;
        private string[] _classFile;
        private string[] _typeNames;
        private string[] _diplayNames;
        private string[] _classTypeNames;
        private string _chosenType = string.Empty;
        private Vector2 _scrollPos = Vector2.zero;

        public static void ShowWindow(string path, string[] typeNames, string[] displayNames, string[] classFile, string type)
        {
            var window = GetWindow<FileCreationWindow>();
            window._path = path;
            window._typeNames = typeNames;
            window._diplayNames = displayNames;
            window._classFile = classFile;
            window._type = type;

            var monoScriptGuids = AssetDatabase.FindAssets("t:MonoScript");
            List<string> classTypeNames = new List<string>();

            for (int i = 0; i < monoScriptGuids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(monoScriptGuids[i]);

                if (!string.IsNullOrEmpty(assetPath))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

                    if (asset != null && asset.GetClass() != null && !string.IsNullOrEmpty(asset.GetClass().Name))
                    {
                        classTypeNames.Add(asset.GetClass().Name);
                    }
                }
            }

            window._classTypeNames = classTypeNames.ToArray();
        }

        private void OnGUI()
        {
            _chosenType = EditorGUILayout.TextField("Chose type", _chosenType);

            var suggestions = _diplayNames.Where(s => s.ToLower().Contains(_chosenType.ToLower()) && !_classTypeNames.Contains($"{s}{_type}")).ToArray();

            if (suggestions.Length > 0)
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                for (int i = 0; i < suggestions.Length; i++)
                {
                    if (EditorGUILayout.DropdownButton(new GUIContent(suggestions[i]), FocusType.Keyboard, GUI.skin.button))
                    {
                        _chosenType = suggestions[i];

                        using (var stream = new FileStream($"{_path}/{_chosenType}{_type}.cs", FileMode.CreateNew))
                        {
                            using (var writer = new StreamWriter(stream))
                            {
                                for (int t = 0; t < _classFile.Length; t++)
                                {                                   
                                    if (_classFile[t].Contains("{0}"))
                                    {
                                        string formatingValue = _classFile[t].Contains("[CustomEditor(typeof(") ? 
                                            _typeNames[Array.IndexOf(_diplayNames, _chosenType)].Split(',')[0] : _chosenType;

                                        writer.WriteLine(string.Format(_classFile[t], formatingValue));
                                    }
                                    else
                                    {
                                        writer.WriteLine(_classFile[t]);
                                    }
                                }
                            }
                        }

                        Close();
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    }
                }

                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.TextField("No UnityEngine object classes are available.");
                GUI.enabled = true;
            }

        }
    }
}