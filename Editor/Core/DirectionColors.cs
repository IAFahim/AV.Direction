using UnityEngine;

namespace AV.Direction.Editor.Core
{
    public static class DirectionColors
    {
        public static readonly Color DefaultDirection = new Color(0.2f, 0.8f, 1f, 1f);  // Sky Blue
        public static readonly Color DefaultRange = new Color(0.2f, 1f, 0.4f, 1f);      // Green
        public static readonly Color DefaultLineRange = new Color(1f, 0.6f, 0.2f, 1f);  // Orange

        public static readonly Color HandleActive = new Color(1f, 0.92f, 0.016f, 1f);   // Gold/Yellow
        public static readonly Color TextShadow = new Color(0f, 0f, 0f, 0.8f);
        public static readonly Color InfinityLine = new Color(1f, 1f, 1f, 0.1f);

        public static Color GetFillColor(Color baseColor)
        {
            return new Color(baseColor.r, baseColor.g, baseColor.b, 0.15f);
        }

        public static Color GetOutlineColor(Color baseColor)
        {
            return new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
        }

        public static Color ApplyGlobalOpacity(Color baseColor, float globalOpacity)
        {
            return new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * globalOpacity);
        }
    }
}