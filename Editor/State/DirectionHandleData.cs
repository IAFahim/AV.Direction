using UnityEngine;
using AV.Direction.Runtime.Enums;

[HelpURL("https://github.com/IAFahim/AV.Direction")]

namespace AV.Direction.Editor.State
{
    public enum HandleType { Angle, Line, Radius }

    public class DirectionHandleData
    {
        // Identity
        public Object Component;
        public string PropertyPath;
        public string PropertyName;
        public HandleType Type;

        // Visuals
        public DirectionStyle Style;
        public Color Color;
        public float VisualLength; // For arrows
        public float ArcAngle;     // For pie slices
        public Vector3 LocalAxis;  // For LineRange

        // Cached World State (calculated once per frame/check)
        public Vector3 WorldPosition;
        public Quaternion WorldRotation;
        public float CurrentValue;

        // Helper to check validity without try-catch
        public bool IsValid => Component != null;
    }
}
