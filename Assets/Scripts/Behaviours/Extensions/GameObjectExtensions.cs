using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject FindChildWithName(this GameObject gameObject, string name)
    {
        Transform _transform = gameObject.transform.FindChildWithName(name);

        return _transform == null ? null : _transform.gameObject;
    }

    public static List<T> GetComponentAllInChilds<T>(this GameObject original)
    {
        List<T> componentList = new List<T>();

        for (int i = 0; i < original.transform.childCount; i++)
        {
            if (original.TryGetComponent<T>(out T temp))
            {
                componentList.Add(temp);
            }
        }

        return componentList;
    }


    public static bool HasComponent<T>(this GameObject gameObject)
    {
        return gameObject.GetComponent<T>() != null;
    }

    public static int ChildCount(this GameObject original)
    {
        return original.transform.childCount;
    }

    public static void SetActiveAllChilds(this Transform original, bool value)
    {
        for (int i = 0; i < original.childCount; i++)
        {
            original.GetChild(i).gameObject.SetActive(value);
        }
    }
}
