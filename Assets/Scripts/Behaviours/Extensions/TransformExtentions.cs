using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtentions
{
    public static Transform FindChildWithName(this Transform transform, string name)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name == name)
            {
                return child;
            }
        }

        return null;
    }

    public static Transform FindChildWithComponent<T>(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<T>() != null)
            {
                return child;
            }
        }

        return null;
    }

    public static void SetPosition(this Transform transform, float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }

    public static void SetEulerRotation(this Transform transform, float x, float y, float z)
    {
        transform.rotation = Quaternion.Euler(x, y, z);
    }

}
