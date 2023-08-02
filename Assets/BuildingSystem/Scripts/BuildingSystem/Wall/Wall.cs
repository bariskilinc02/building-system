using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public WallData wallData;
    public WallMesh wallMesh;

    private void Awake()
    {
        wallData = new WallData();
    }
}
