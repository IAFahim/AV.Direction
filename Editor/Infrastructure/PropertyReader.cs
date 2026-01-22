using UnityEditor;

namespace AV.Direction.Editor.Infrastructure
{
    public static class PropertyReader
    {
        public static float ReadFloat(SerializedProperty property)
        {
            if (property == null) return 0f;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    return property.floatValue;
                case SerializedPropertyType.Integer:
                    return (float)property.intValue;
                default:
                    return 0f;
            }
        }

        public static void WriteFloat(SerializedProperty property, float value)
        {
            if (property == null) return;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    property.floatValue = value;
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = UnityEngine.Mathf.RoundToInt(value);
                    break;
            }
        }
    }
}