using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Extensions
{
    public static void debug(this object original)
    {
        Debug.Log(original);
    }
}
