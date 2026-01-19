using System;
using UnityEngine;

[HelpURL("https://github.com/IAFahim/AV.Direction")]

namespace AV.Direction.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RangeCircleAttribute : PropertyAttribute
    {
        public readonly bool UseCustomColor;
        public readonly Color CustomColor;

        public RangeCircleAttribute()
        {
            UseCustomColor = false;
            CustomColor = Color.white;
        }

        public RangeCircleAttribute(float r, float g, float b)
        {
            UseCustomColor = true;
            CustomColor = new Color(r, g, b, 1f);
        }
    }
}