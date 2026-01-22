using UnityEngine;
using AV.Direction.Runtime.Enums;

namespace AV.Direction.Editor.State
{
    // ===================================================================================
    // LAYER A: DATA & DEFINITIONS
    // ===================================================================================

    public enum DirectionHandleType { Angle, Line, Radius }

    public class DirectionState
    {
        // Identity
        public Object Component;
        public string PropertyPath;
        public string PropertyName;
        public DirectionHandleType Type;

        // Visuals
        public DirectionStyle Style;
        public Color Color;
        public float VisualLength; 
        public float ArcAngle;     
        public Vector3 LocalAxis;  

        // Cached World State
        public Vector3 WorldPosition;
        public Quaternion WorldRotation;
        public float CurrentValue;

        // Validity Check
        public bool IsValid => Component != null;

        // ⭐️ DEBUG CARD
        public override string ToString() => $"[DIR] {PropertyName}: {CurrentValue:F1} ({Type})";
    }
}
