using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Reference")] 
    public string itemId;
    
    [Header("Components")]
    public ItemData itemData;
    public ItemMesh itemMesh;

    [Header("Data")]
    public Vector2Int lastCoordinate;
    public bool itemPlacedBefore;
    public List<Cell> occupiedCells;

    [Header("Connections")]
    public GridBase connectedGrid;


    protected virtual void Awake()
    {
        occupiedCells = new List<Cell>();
        itemData = ItemDatabase.Instance.CreateItemInstance(itemId);
    }

    protected virtual void Start()
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
