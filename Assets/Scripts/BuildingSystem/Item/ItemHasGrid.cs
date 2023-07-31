using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHasGrid : Item
{
    public GridBase grid;

    protected override void Start()
    {
        base.Start();

        DisableGrid();
        
        BuildingSystem.Instance.OnWallPlacementSystemEnabled += DisableGrid;
        BuildingSystem.Instance.OnWallPlacementSystemDisabled += EnableGrid;
    }

    public void EnableGrid()
    {
        grid.EnableGrid();
    }
    
    public void DisableGrid()
    {
        grid.DisableGrid();
    }
}
