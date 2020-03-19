using System;
using UnityEditor;
using UnityEngine;

namespace HephaestusForge.EditorFieldOnly.Internal
{
    [Serializable]
    public class EditorField
    {
#pragma warning disable 0649

        [SerializeField]
        private MonoScript _usedInScript;

        [SerializeField]
        private string _fieldName;

        [SerializeField]
        private string _sceneGuid;

        [SerializeField]
        private int _objectID;

        [SerializeField]
        private string _fieldID;

        [SerializeField]
        private string _guidPath;

#pragma warning restore 0649

        public int ObjectID { get => _objectID; }
        public string GuidPath { get => _guidPath; }
        public string SceneGuid { get => _sceneGuid; }
        public string FieldName { get => _fieldName; }

        public static implicit operator bool(EditorField source) => source != null;
    }
}
