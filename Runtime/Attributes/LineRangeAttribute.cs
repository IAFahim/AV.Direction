using System;
using UnityEngine;

namespace AV.Direction.Runtime.Attributes
{
    /// <summary>
    /// Visualizes a float as a linear distance along a local axis.
    /// Example: [LineRange(0, 0, 1)] for forward distance.
    /// </summary>
    [HelpURL("https://github.com/IAFahim/AV.Direction")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LineRangeAttribute : PropertyAttribute
    {
        public readonly Vector3 Axis;
        public readonly bool UseCustomColor;
        public readonly Color CustomColor;

        /// <summary>
        /// Creates a line range visualizer along the specified local axis.
        /// </summary>
        /// <param name="x">Local X axis component</param>
        /// <param name="y">Local Y axis component</param>
        /// <param name="z">Local Z axis component</param>
        public LineRangeAttribute(float x, float y, float z)
        {
            Axis = new Vector3(x, y, z).normalized;
            UseCustomColor = false;
            CustomColor = Color.white;
        }

        public LineRangeAttribute(float x, float y, float z, float r, float g, float b)
        {
            Axis = new Vector3(x, y, z).normalized;
            UseCustomColor = true;
            CustomColor = new Color(r, g, b, 1f);
        }
    }
}