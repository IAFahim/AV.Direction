using System;
using AV.Direction.Runtime.Enums;
using UnityEngine;

namespace AV.Direction.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DirectionAttribute : PropertyAttribute
    {
        public readonly DirectionStyle Style;
        public readonly float Length;
        public readonly float ArcAngle;
        public readonly bool UseCustomColor;
        public readonly Color CustomColor;

        /// <summary>
        /// Visualizes a float field as a direction in the Scene View.
        /// </summary>
        /// <param name="style">The visual style (Arrow, Cone, PieSlice, etc.).</param>
        /// <param name="length">The length of the arrow or radius of the arc.</param>
        /// <param name="arcAngle">The angle width for Cone/PieSlice styles.</param>
        public DirectionAttribute(DirectionStyle style = DirectionStyle.Arrow, float length = 5f, float arcAngle = 45f)
        {
            Style = style;
            Length = length;
            ArcAngle = arcAngle;
            UseCustomColor = false;
            CustomColor = Color.white;
        }

        /// <summary>
        /// Visualizes a float field with a custom color.
        /// </summary>
        public DirectionAttribute(float r, float g, float b, DirectionStyle style = DirectionStyle.Arrow, float length = 5f, float arcAngle = 45f)
        {
            Style = style;
            Length = length;
            ArcAngle = arcAngle;
            UseCustomColor = true;
            CustomColor = new Color(r, g, b, 1f);
        }
    }
}