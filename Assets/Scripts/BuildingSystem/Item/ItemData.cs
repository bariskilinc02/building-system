using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Item Data")]
public class ItemData : ScriptableObject
{
    public string id;
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