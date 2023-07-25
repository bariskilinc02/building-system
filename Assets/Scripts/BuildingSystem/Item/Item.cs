using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public ItemMesh itemMesh;

    public Vector2Int lastCoordinate;
    public bool itemPlacedBefore;
    
   [Header("Connections")]
    public GridBase connectedGrid;
    public List<Cell> occupiedCells;

    private void Awake()
    {
        occupiedCells = new List<Cell>();
        itemData = new ItemData();
        itemData.size = new Vector2Int(1, 1);
        itemData.direction = Direction._0;
        itemData.requireWall = true;
    }

    private void Start()
    {
        EnableTrigger();
    }

    public void EnableTrigger()
    {
        itemMesh.EnableTrigger();
    }
    
    public void DisableTrigger()
    {
        itemMesh.DisableTrigger();
    }
}
