using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public Vector2Int size;
    public Direction direction;
    public bool requireWall;
}

public enum Direction
{
    _0,
    _90,
    _180,
    _270
}