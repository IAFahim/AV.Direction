using UnityEditor;
using UnityEngine;

namespace AV.Direction.Editor.Infrastructure
{
    /// <summary>
    /// Reader for SerializedProperty values with fail-loud error handling.
    /// </summary>
    public static class PropertyReader
    {
        /// <summary>
        /// Reads a float value from a SerializedProperty.
        /// Throws an exception if the property is null or invalid (fail-loud policy).
        /// </summary>
        public static float ReadFloat(SerializedProperty property)
        {
            if (property == null)
            {
                Debug.LogError("[PropertyReader] Attempted to read from null SerializedProperty. Reflection logic has failed.");
                throw new System.InvalidOperationException("Cannot read from null SerializedProperty.");
            }

            return property.propertyType switch
            {
                SerializedPropertyType.Float => property.floatValue,
                SerializedPropertyType.Integer => (float)property.intValue,
                _ => throw new System.InvalidOperationException(
                    $"[PropertyReader] Property '{property.propertyPath}' has unsupported type '{property.propertyType}' for float reading.")
            };
        }

        /// <summary>
        /// Writes a float value to a SerializedProperty.
        /// Throws an exception if the property is null or invalid (fail-loud policy).
        /// </summary>
        public static void WriteFloat(SerializedProperty property, float value)
        {
            if (property == null)
            {
                Debug.LogError("[PropertyReader] Attempted to write to null SerializedProperty. Reflection logic has failed.");
                throw new System.InvalidOperationException("Cannot write to null SerializedProperty.");
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    property.floatValue = value;
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = Mathf.RoundToInt(value);
                    break;
                default:
                    throw new System.InvalidOperationException(
                        $"[PropertyReader] Property '{property.propertyPath}' has unsupported type '{property.propertyType}' for float writing.");
            }
        }
    }
}
