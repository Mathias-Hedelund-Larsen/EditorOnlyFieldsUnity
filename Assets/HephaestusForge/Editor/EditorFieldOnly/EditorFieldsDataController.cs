﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HephaestusForge
{
    namespace EditorFieldOnly
    {
        public sealed class EditorFieldsDataController : ScriptableObject
        {
            #region Fields of serialized fields
            [SerializeField]
            private BoolField[] _boolFields;

            [SerializeField]
            private FloatField[] _floatFields;

            [SerializeField]
            private IntField[] _intFields;

            [SerializeField]
            private StringField[] _stringFields;

            [SerializeField]
            private Vector2Field[] _vector2Fields;

            [SerializeField]
            private Vector2IntField[] _vector2IntFields;

            [SerializeField]
            private Vector3Field[] _vector3Fields;

            [SerializeField]
            private Vector3IntField[] _vector3IntFields;

            [SerializeField]
            private BoolCollectionField[] _boolCollectionFields;

            [SerializeField]
            private FloatCollectionField[] _floatCollectionFields;

            [SerializeField]
            private IntCollectionField[] _intCollectionFields;

            [SerializeField]
            private StringCollectionField[] _stringCollectionFields;

            [SerializeField]
            private Vector2CollectionField[] _vector2CollectionFields;

            [SerializeField]
            private Vector2IntCollectionField[] _vector2IntCollectionFields;

            [SerializeField]
            private Vector3CollectionField[] _vector3CollectionFields;

            [SerializeField]
            private Vector3IntCollectionField[] _vector3IntCollectionFields;
            #endregion

            [System.NonSerialized]
            private Dictionary<string, Dictionary<string, object>> _fields = new Dictionary<string, Dictionary<string, object>>();

            [UnityEditor.MenuItem("Assets/Create/HephaestusForge/Limited to one/EditorFieldsDataController", false, 0)]
            private static void CreateInstance()
            {
                if (UnityEditor.AssetDatabase.FindAssets("t:EditorFieldsDataController").Length == 0)
                {
                    var path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);

                    if (path.Length > 0)
                    {
                        var obj = CreateInstance<EditorFieldsDataController>();

                        if (Directory.Exists(path))
                        {
                            UnityEditor.AssetDatabase.CreateAsset(obj, path + "/EditorFieldsDataController.asset");

                            return;
                        }

                        var pathSplit = path.Split('/').ToList();
                        pathSplit.RemoveAt(pathSplit.Count - 1);
                        path = string.Join("/", pathSplit);

                        if (Directory.Exists(path))
                        {
                            UnityEditor.AssetDatabase.CreateAsset(obj, path + "/EditorFieldsDataController.asset");

                            return;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("An instance of EditorFieldsDataController already exists");
                }
            }

            private object GetValueInEditorPlayMode(string fileGuid, string fieldName)
            {
                if (!_fields.ContainsKey(fileGuid))
                {
                    string[] fileData = null;
                    var path = $"{Application.persistentDataPath}/{BaseEditorFieldOnlyInspector.FIELDS_DIRECTORY}/{fileGuid}.txt";

                    using (var reader = new StreamReader(path))
                    {
                        fileData = reader.ReadToEnd().Split('\n');
                    }

                    _fields.Add(fileGuid, new Dictionary<string, object>());

                    for (int i = 0; i < fileData.Length; i++)
                    {
                        if (fileData[i] != string.Empty)
                        {
                            string nameOfField = fileData[i].Split(':')[0];
                            string fieldType = fileData[i].Split(':')[1].Split('=')[0];
                            var fieldValue = fileData[i].Split('=')[1];

                            if (fieldType == SerializedPropertyType.Boolean.ToString())
                            {
                                _fields[fileGuid].Add(nameOfField, ParseBool(fieldValue, path, nameOfField));                                
                            }                            
                            else if(fieldType == SerializedPropertyType.Integer.ToString())
                            {
                                _fields[fileGuid].Add(nameOfField, ParseInt(fieldValue, path, nameOfField));
                            }
                            else if(fieldType == SerializedPropertyType.Float.ToString())
                            {
                                _fields[fileGuid].Add(nameOfField, ParseFloat(fieldValue, path, nameOfField));
                            }
                            else if (fieldType == SerializedPropertyType.String.ToString())
                            {
                                _fields[fileGuid].Add(nameOfField, fieldValue.Replace(BaseEditorFieldOnlyInspector.OPEN_BRACKET_REPLACEMENT, "[").
                                    Replace(BaseEditorFieldOnlyInspector.VERTICAL_LINE_REPLACEMENT, "|").Replace(BaseEditorFieldOnlyInspector.CLOSED_BRACKET_REPLACEMENT, "]"));
                            }
                            else if(fieldType == SerializedPropertyType.Vector2.ToString())
                            {
                                _fields[fileGuid].Add(nameOfField, ParseVector2(fieldValue, path, nameOfField));
                            }
                            else if (fieldType == BaseEditorFieldOnlyInspector.BOOL_ARRAY)
                            {
                                GetSubStringBetweenChars(fieldValue, '[', ']', out string full, out string inside);
                                var valuesOfArray = inside.Split('|');

                                bool[] bools = new bool[valuesOfArray.Length];

                                for (int t = 0; t < bools.Length; t++)
                                {
                                    bools[t] = ParseBool(valuesOfArray[t], path, nameOfField);
                                }

                                _fields[fileGuid].Add(nameOfField, bools);
                            }
                        }
                    }
                }

                return _fields[fileGuid][fieldName];
            }

            private Vector2 ParseVector2(string vector2Value, string path, string fieldName)
            {
                GetSubStringBetweenChars(vector2Value, '(', ')', out string full, out string inside);
                var values = inside.Split(',');

                if(float.TryParse(values[0], out float x) && float.TryParse(values[1], out float y))
                {
                    return new Vector2(x, y);
                }
                else
                {
                    throw new FormatException($"Couldnt parse: {vector2Value} to vector2, something went wrong look into the file: {path} at {fieldName}.");
                }
            }

            private float ParseFloat(string floatValue, string path, string fieldName)
            {
                if (float.TryParse(floatValue, out float fieldValue))
                {
                    return fieldValue;
                }
                else
                {
                    throw new FormatException($"Couldnt parse: {floatValue} to float, something went wrong look into the file: {path} at {fieldName}.");
                }
            }

            private int ParseInt(string intValue, string path, string fieldName)
            {
                if (int.TryParse(intValue, out int fieldValue))
                {
                    return fieldValue;
                }
                else
                {
                    throw new FormatException($"Couldnt parse: {intValue} to int, something went wrong look into the file: {path} at {fieldName}.");
                }
            }

            private bool ParseBool(string boolValue, string path, string fieldName)
            {
                if (bool.TryParse(boolValue, out bool fieldValue))
                {
                    return fieldValue;
                }
                else
                {
                    throw new FormatException($"Couldnt parse: {boolValue} to bool, something went wrong look into the file: {path} at {fieldName}.");
                }
            }

            private void GetSubStringBetweenChars(string origin, char start, char end, out string fullMatch, out string insideEncapsulation)
            {
                var match = Regex.Match(origin, string.Format(@"\{0}(.*?)\{1}", start, end));
                fullMatch = match.Groups[0].Value;
                insideEncapsulation = match.Groups[1].Value;
            }
        }
    }
}