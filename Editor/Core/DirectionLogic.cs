using System.Runtime.CompilerServices;
using UnityEngine;

namespace AV.Direction.Editor.Core
{
    // ===================================================================================
    // LAYER B: LOGIC (Stateless)
    // ===================================================================================

    public static class DirectionLogic
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateNewAngle(Vector3 handlePos, Transform origin)
        {
            Vector3 direction = handlePos - origin.position;
            Vector3 projectedDir = Vector3.ProjectOnPlane(direction, origin.up);

            return Vector3.SignedAngle(origin.forward, projectedDir, origin.up);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateNewDistance(Vector3 handlePos, Transform origin, Vector3 localAxis)
        {
            Vector3 worldAxis = origin.TransformDirection(localAxis);
            Vector3 projectedPos = Vector3.Project(handlePos - origin.position, worldAxis);

            float dot = Vector3.Dot(projectedPos, worldAxis);
            return projectedPos.magnitude * Mathf.Sign(dot);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SnapValue(float value, float interval)
        {
            if (interval <= 0) return value;
            return Mathf.Round(value / interval) * interval;
        }
    }
}
