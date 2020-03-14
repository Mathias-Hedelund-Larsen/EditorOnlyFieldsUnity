using System.IO;
using System.Linq;
using UnityEngine;

namespace HephaestusForge
{
    namespace EditorFieldOnly
    {
        public sealed class EditorFieldsDataController : ScriptableObject
        {
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
        }
    }
}