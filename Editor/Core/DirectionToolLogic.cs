using UnityEngine;
using AV.Direction.Editor.State;

namespace AV.Direction.Editor.Core
{
    public static class DirectionToolLogic
    {
        public static float CalculateNewAngle(Vector3 handlePos, Transform origin)
        {
            // Project handle position onto the plane defined by origin up
            Vector3 direction = handlePos - origin.position;
            Vector3 projectedDir = Vector3.ProjectOnPlane(direction, origin.up);

            float angle = Vector3.SignedAngle(origin.forward, projectedDir, origin.up);
            return angle;
        }

        public static float CalculateNewDistance(Vector3 handlePos, Transform origin, Vector3 localAxis)
        {
            Vector3 worldAxis = origin.TransformDirection(localAxis);
            Vector3 originPos = origin.position;

            // Project handle position onto the axis line
            Vector3 projectedPos = Vector3.Project(handlePos - originPos, worldAxis);

            // Dot product to determine sign (forward or backward along axis)
            float dot = Vector3.Dot(projectedPos, worldAxis);
            float dist = projectedPos.magnitude * Mathf.Sign(dot);

            return dist;
        }

        public static float SnapValue(float value, float interval)
        {
            if (interval <= 0) return value;
            return Mathf.Round(value / interval) * interval;
        }
    }
}
