using System;
using UnityEngine;

namespace HephaestusForge
{
    namespace EditorFieldOnly
    {
        [Serializable]
        public abstract class EditorField<T>
        {
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