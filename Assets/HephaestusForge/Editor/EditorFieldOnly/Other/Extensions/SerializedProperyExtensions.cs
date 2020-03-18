using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HephaestusForge.EditorFieldOnly
{
    public static class SerializedProperyExtensions
    {
        /// <summary>
        /// Gets all children of `SerializedProperty` at 1 level depth.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty serializedProperty)
        {
            SerializedProperty currentProperty = serializedProperty.Copy();
            SerializedProperty nextSiblingProperty = serializedProperty.Copy();
            {
                nextSiblingProperty.Next(false);
            }

            if (currentProperty.Next(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                        break;

                    yield return currentProperty;
                }
                while (currentProperty.Next(false));
            }
        }

        /// <summary>
        /// Gets visible children of `SerializedProperty` at 1 level depth.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(this SerializedProperty serializedProperty)
        {
            SerializedProperty currentProperty = serializedProperty.Copy();

            if (currentProperty.NextVisible(true))
            {
                do
                {
                    if (currentProperty.propertyPath.Contains(serializedProperty.propertyPath))
                    {
                        yield return currentProperty.Copy();
                    }
                }
                while (currentProperty.NextVisible(false));
            }
        }

        public static SerializedProperty FindInArray(this SerializedProperty array, Predicate<SerializedProperty> predicate, out int index)
        {
            if (array.isArray)
            {
                for (int i = array.arraySize - 1; i >= 0; i--)
                {
                    if (predicate.Invoke(array.GetArrayElementAtIndex(i)))
                    {
                        index = i;
                        return array.GetArrayElementAtIndex(i);
                    }
                }

                Debug.LogWarning("Couldnt find what you were searching for in array");
                index = -1;
                return array;
            }
            else
            {

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("The serialized property was not an array");
#endif

                index = -1;
                return array;
            }
        }
    }
}
