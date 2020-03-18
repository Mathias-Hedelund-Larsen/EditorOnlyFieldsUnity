using System;
using UnityEngine;

namespace HephaestusForge.EditorFieldOnly
{
    [Serializable]
    public abstract class EditorField<T> : Internal.EditorField
    {
        [SerializeField]
        private T _fieldValue;
    }
}