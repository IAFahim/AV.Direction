using Unity.Mathematics;
using UnityEngine;

namespace AV.Direction.Editor.Core
{
    /// <summary>
    /// Extension methods for Vector3 to float3 conversion.
    /// Shared between all editor components.
    /// </summary>
    public static class VectorExtensions
    {
        public static float3 ToFloat3(this Vector3 v) => new float3(v.x, v.y, v.z);
        public static Vector3 ToVector3(this float3 f) => new Vector3(f.x, f.y, f.z);
    }
}
