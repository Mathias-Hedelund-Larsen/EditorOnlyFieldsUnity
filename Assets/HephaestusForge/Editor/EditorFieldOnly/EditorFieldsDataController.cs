using System;
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

                            #region Fiel type checks
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
                            else if(fieldType == SerializedPropertyType.Vector2Int.ToString())
                            {
                                _fields[fileGuid].Add(nameOfField, ParseVector2Int(fieldValue, path, nameOfField));
                            }
                            else if (fieldType == SerializedPropertyType.Vector3.ToString())
                            {
                                _fields[fileGuid].Add(nameOfField, ParseVector3(fieldValue, path, nameOfField));
                            }
                            else if (fieldType == SerializedPropertyType.Vector3Int.ToString())
                            {
                                _fields[fileGuid].Add(nameOfField, ParseVector3Int(fieldValue, path, nameOfField));
                            }
                            #endregion
                            #region Field type collection checks
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
                            else if (fieldType == BaseEditorFieldOnlyInspector.INT_ARRAY)
                            {
                                GetSubStringBetweenChars(fieldValue, '[', ']', out string full, out string inside);
                                var valuesOfArray = inside.Split('|');

                                int[] ints = new int[valuesOfArray.Length];

                                for (int t = 0; t < ints.Length; t++)
                                {
                                    ints[t] = ParseInt(valuesOfArray[t], path, nameOfField);
                                }

                                _fields[fileGuid].Add(nameOfField, ints);
                            }
                            else if (fieldType == BaseEditorFieldOnlyInspector.FLOAT_ARRAY)
                            {
                                GetSubStringBetweenChars(fieldValue, '[', ']', out string full, out string inside);
                                var valuesOfArray = inside.Split('|');

                                float[] floats = new float[valuesOfArray.Length];

                                for (int t = 0; t < floats.Length; t++)
                                {
                                    floats[t] = ParseFloat(valuesOfArray[t], path, nameOfField);
                                }

                                _fields[fileGuid].Add(nameOfField, floats);
                            }
                            else if (fieldType == BaseEditorFieldOnlyInspector.STRING_ARRAY)
                            {
                                GetSubStringBetweenChars(fieldValue, '[', ']', out string full, out string inside);
                                var valuesOfArray = inside.Split('|');

                                string[] strings = new string[valuesOfArray.Length];

                                for (int t = 0; t < strings.Length; t++)
                                {
                                    strings[t] = valuesOfArray[t].Replace(BaseEditorFieldOnlyInspector.OPEN_BRACKET_REPLACEMENT, "[").
                                        Replace(BaseEditorFieldOnlyInspector.VERTICAL_LINE_REPLACEMENT, "|").Replace(BaseEditorFieldOnlyInspector.CLOSED_BRACKET_REPLACEMENT, "]");
                                }

                                _fields[fileGuid].Add(nameOfField, strings);
                            }
                            else if (fieldType == BaseEditorFieldOnlyInspector.VECTOR2_ARRAY)
                            {
                                GetSubStringBetweenChars(fieldValue, '[', ']', out string full, out string inside);
                                var valuesOfArray = inside.Split('|');

                                Vector2[] vectors = new Vector2[valuesOfArray.Length];

                                for (int t = 0; t < vectors.Length; t++)
                                {
                                    vectors[t] = ParseVector2(valuesOfArray[t], path, nameOfField);
                                }

                                _fields[fileGuid].Add(nameOfField, vectors);
                            }
                            else if (fieldType == BaseEditorFieldOnlyInspector.VECTOR2INT_ARRAY)
                            {
                                GetSubStringBetweenChars(fieldValue, '[', ']', out string full, out string inside);
                                var valuesOfArray = inside.Split('|');

                                Vector2Int[] vectors = new Vector2Int[valuesOfArray.Length];

                                for (int t = 0; t < vectors.Length; t++)
                                {
                                    vectors[t] = ParseVector2Int(valuesOfArray[t], path, nameOfField);
                                }

                                _fields[fileGuid].Add(nameOfField, vectors);
                            }
                            else if (fieldType == BaseEditorFieldOnlyInspector.VECTOR3_ARRAY)
                            {
                                GetSubStringBetweenChars(fieldValue, '[', ']', out string full, out string inside);
                                var valuesOfArray = inside.Split('|');

                                Vector3[] vectors = new Vector3[valuesOfArray.Length];

                                for (int t = 0; t < vectors.Length; t++)
                                {
                                    vectors[t] = ParseVector3(valuesOfArray[t], path, nameOfField);
                                }

                                _fields[fileGuid].Add(nameOfField, vectors);
                            }
                            else if (fieldType == BaseEditorFieldOnlyInspector.VECTOR3INT_ARRAY)
                            {
                                GetSubStringBetweenChars(fieldValue, '[', ']', out string full, out string inside);
                                var valuesOfArray = inside.Split('|');

                                Vector3Int[] vectors = new Vector3Int[valuesOfArray.Length];

                                for (int t = 0; t < vectors.Length; t++)
                                {
                                    vectors[t] = ParseVector3Int(valuesOfArray[t], path, nameOfField);
                                }

                                _fields[fileGuid].Add(nameOfField, vectors);
                            }
                            #endregion
                        }
                    }
                }

                return _fields[fileGuid][fieldName];
            }

            private Vector3Int ParseVector3Int(string vector3IntValue, string path, string fieldName)
            {
                GetSubStringBetweenChars(vector3IntValue, '(', ')', out string full, out string inside);
                var values = inside.Split(',');

                if (int.TryParse(values[0], out int x) && int.TryParse(values[1], out int y) && int.TryParse(values[2], out int z))
                {
                    return new Vector3Int(x, y, z);
                }
                else
                {
                    throw new FormatException($"Couldnt parse: {vector3IntValue} to vector3, something went wrong look into the file: {path} at {fieldName}.");
                }
            }

            private Vector3 ParseVector3(string vector3Value, string path, string fieldName)
            {
                GetSubStringBetweenChars(vector3Value, '(', ')', out string full, out string inside);
                var values = inside.Split(',');

                if (float.TryParse(values[0], out float x) && float.TryParse(values[1], out float y) && float.TryParse(values[2], out float z))
                {
                    return new Vector3(x, y, z);
                }
                else
                {
                    throw new FormatException($"Couldnt parse: {vector3Value} to vector3, something went wrong look into the file: {path} at {fieldName}.");
                }
            }

            private Vector2Int ParseVector2Int(string vector2IntValue, string path, string fieldName)
            {
                GetSubStringBetweenChars(vector2IntValue, '(', ')', out string full, out string inside);
                var values = inside.Split(',');

                if (int.TryParse(values[0], out int x) && int.TryParse(values[1], out int y))
                {
                    return new Vector2Int(x, y);
                }
                else
                {
                    throw new FormatException($"Couldnt parse: {vector2IntValue} to vector2int, something went wrong look into the file: {path} at {fieldName}.");
                }
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