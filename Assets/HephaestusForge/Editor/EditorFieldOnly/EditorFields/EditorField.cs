using System;
using UnityEditor;
using UnityEngine;

namespace HephaestusForge.EditorFieldOnly
{
    [Serializable]
    public abstract class EditorField<T> : Internal.EditorField
    {
#pragma warning disable 0649

        [SerializeField]
        private T _fieldValue;

        [SerializeField]
        private EditorFieldsDataController _controller;

#pragma warning restore 0649

        public T FieldValue { get => _fieldValue;
            set 
            {
                _fieldValue = value;

                EditorUtility.SetDirty(_controller);
            }
        }
    }
}