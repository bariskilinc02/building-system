using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;



[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
public class MethodAttribute : PropertyAttribute
{
    public bool onlyPlayMode;
    public bool showWarnings;
    public MethodAttribute(bool onlyPlayMode = false, bool showWarnings = false)
    {
        this.onlyPlayMode = onlyPlayMode;
        this.showWarnings = showWarnings;
    }
}
