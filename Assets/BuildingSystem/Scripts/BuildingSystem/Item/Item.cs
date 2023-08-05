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
    public Direction lastDirection;
    public bool itemPlacedBefore;
    public List<Cell> occupiedCells;

    [Header("Connections")]
    public GridBase connectedGrid;

    public Transform pivotTransform => transform.GetChild(0);


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

    public virtual void ChangeDirection()
    {
        itemData.direction += 1;

        if (itemData.direction >= (Direction)4)
        {
            itemData.direction = (Direction)0;
        }

        SetRotation();
    }

    public void SetRotation()
    {
        float yAngle = 0;
        switch (itemData.direction)
        {
            case Direction._0:
                yAngle = 0;
                break;
            case Direction._90:
                yAngle = 90;
                break;
            case Direction._180:
                yAngle = 180;
                break;
            case Direction._270:
                yAngle = 270;
                break;
        }

        var eulerAngles = pivotTransform.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, yAngle, eulerAngles.z);
        pivotTransform.eulerAngles = eulerAngles;
    }
}
