using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfExtensions
{
    /// <summary>
    /// Returns absolute value
    /// </summary>
    public static float Abs(this float original)
    {
        return Mathf.Abs(original);
    }
    
    public static int ToInt(this float original)
    {
        return (int)original;
    }
}
