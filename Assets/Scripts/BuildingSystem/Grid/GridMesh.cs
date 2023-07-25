using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMesh : MonoBehaviour
{
    private GridBase _gridBase;
    private Material _material;
    public GridBase GetGrid()
    {
        if (_gridBase == null)
            _gridBase = GetComponentInParent<GridBase>();
        
        return _gridBase;
    }

    public Material GetMaterial()
    {
        if (_material == null)
            _material = GetComponent<MeshRenderer>().material;
        
        return _material;
    }
}
