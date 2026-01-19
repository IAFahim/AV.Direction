using Unity.Burst;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

namespace AV.Direction.Editor.Core
{
    [BurstCompile]
    public static class DirectionMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AngleToDirection(in float angleDegrees, in float3 forward, in float3 up, out float3 direction)
        {
            quaternion rot = quaternion.AxisAngle(up, math.radians(angleDegrees));
            direction = math.rotate(rot, forward);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PositionToAngle(in float3 pivot, in float3 targetPos, in float3 forward, in float3 up, out float angle)
        {
            float3 dirToTarget = math.normalize(targetPos - pivot);
            float3 cross = math.cross(forward, dirToTarget);
            float sign = math.dot(cross, up);
            float dot = math.clamp(math.dot(forward, dirToTarget), -1f, 1f);
            float unsignedAngle = math.degrees(math.acos(dot));
            angle = (sign >= 0) ? unsignedAngle : -unsignedAngle;
        }

        /// <summary>
        /// Projects a world position onto a ray to find the distance parameter along the line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProjectPointOnLine(in float3 lineOrigin, in float3 lineDirection, in float3 point, out float distance)
        {
            float3 offset = point - lineOrigin;
            distance = math.dot(offset, lineDirection);
        }
    }
}