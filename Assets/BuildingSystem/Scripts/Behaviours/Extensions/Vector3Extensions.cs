using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Zero(this Vector3 original)
    {
        return new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Change vector3 indexes
    /// </summary>
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? original.x, y ?? original.y, y ?? original.y);
    }


}
