using System.Collections.Generic;
using UnityEditor;

namespace HephaestusForge
{
    namespace EditorFieldOnly
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
        }
    }
}
