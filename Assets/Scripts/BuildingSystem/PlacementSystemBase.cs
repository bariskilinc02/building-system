using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlacementSystemBase : MonoBehaviour
{
    public abstract void Run();
    
    public abstract void OnSystemEnabled();
    
    public abstract void OnSystemDisabled();
}
