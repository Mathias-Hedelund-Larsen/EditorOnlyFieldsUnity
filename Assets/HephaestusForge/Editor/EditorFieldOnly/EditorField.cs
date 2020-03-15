using System;
using UnityEditor;
using UnityEngine;

namespace HephaestusForge
{
    namespace EditorFieldOnly
    {
        [Serializable]
        public abstract class EditorField<T>
        {
            [SerializeField]
            private MonoScript _usedInScript;

            [SerializeField]
            private string _fieldName;

            [SerializeField]
            private string _sceneGuid;

            [SerializeField]
            private int _objectID;

            [SerializeField]
            private T _fieldValue;
        }
    }
}