using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementSystem : PlacementSystemBase
{
    private Camera _camera;

    public Item movingItem;
    
    public ItemPlacementType itemPlacementType;
    
    void Start()
    {
        _camera = Camera.main;
    }
    
    public override void Run()
    {
        PickItem();
        
        DrawPlacement();
        
        PlaceItemTest();

        RotateItem();
    }

    public void RotateItem()
    {
        if (movingItem == null) return;

        if (!Input.GetKeyDown(KeyCode.E)) return;
        
        movingItem.ChangeDirection();

    }
    
    public override void OnSystemEnabled()
    {
        
    }

    public override void OnSystemDisabled()
    {
        if(movingItem != null)
            Destroy(movingItem.gameObject);
    }

    public void DrawPlacement()
    {
        if (movingItem == null) return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo)) return;
        
        if(hitInfo.transform == null) return;

        if (hitInfo.transform.TryGetComponent(out GridMesh gridMesh))
        {
            GridBase gridBase = gridMesh.GetGrid();

            Vector2Int cellCoordinate = gridBase.GetCellCoordinate(hitInfo.point);
            var result = gridBase.IsCellsEmpty(cellCoordinate, movingItem.itemData.size, movingItem.itemData.direction,gridBase._gridDirection);

            movingItem.transform.position = gridBase.GetCellPosition(hitInfo.point);
        }
    }

    /*
    public void PlaceItem()
    {
        if (!Input.GetMouseButtonUp(0))return;

        if (movingItem == null) return;

        if (!SendRay(out RaycastHit hit)) return;
        
        if (hit.transform.TryGetComponent(out GridMesh gridMesh))
        {
            GridBase gridBase = gridMesh.GetGrid();
            
            Vector2Int cellCoordinate = gridBase.GetCellCoordinate(hit.point);
            
            if (gridBase is GridGround gridGround)
            {
                Edge edge = gridGround.GetEdgeInDirection(cellCoordinate, movingItem.itemData.direction);
                if (edge.wall == null)
                {
                    
                }
            }

            
            var result = gridBase.IsCellsEmpty(cellCoordinate, movingItem.itemData.size, movingItem.itemData.direction);
            if (!result)
            {
                PlaceItemToLastPosition(movingItem);
                return;
            }

            

            movingItem.transform.position = gridBase.GetCellPosition(hit.point);
            gridBase.PlaceItem(gridBase, cellCoordinate, movingItem);
            movingItem.EnableTrigger();
            movingItem.itemPlacedBefore = true;
            movingItem = null;
  
        }
        
    }
*/
    public void PlaceItemTest()
    {
        if (!Input.GetMouseButtonUp(0))return;

        if (movingItem == null) return;

        if (!SendRay(out RaycastHit hit)) return;
        
        if (hit.transform.TryGetComponent(out GridMesh gridMesh))
        {
            GridBase gridBase = gridMesh.GetGrid();
            
            Vector2Int cellCoordinate = gridBase.GetCellCoordinate(hit.point);
            bool canBuild = true;

            if (gridBase is GridGround gridGround)
            {
                Edge edge = gridGround.GetEdgeInDirection(cellCoordinate, movingItem.itemData.direction);
                if (edge.wall == null && movingItem.itemData.requireWall == true)
                {
                    canBuild = false;
                }
                
                if (gridGround.IsThereAnyWallInFieldAndIsFieldExist(cellCoordinate, movingItem.itemData.direction, movingItem.itemData.size))
                {
                    canBuild = false;
                }
            }
            
            var cellIsEmpty = gridBase.IsCellsEmpty(cellCoordinate, movingItem.itemData.size, movingItem.itemData.direction, gridBase._gridDirection);
            
            
            if (!cellIsEmpty)
            {
                canBuild = false;
            }

            if (!canBuild)
            {
                if (movingItem.itemPlacedBefore)
                {
                    PlaceItemToLastPosition(movingItem);
                }
            }
            else
            {
                movingItem.transform.position = gridBase.GetCellPosition(hit.point);
                gridBase.PlaceItem(gridBase, cellCoordinate, movingItem, movingItem.itemData.direction,gridBase._gridDirection);
                if (movingItem is ItemHasGrid hasGridItem)
                {
                    hasGridItem.EnableGrid();
                }
                movingItem.EnableTrigger();
                movingItem.itemPlacedBefore = true;
                movingItem = null;
            }

           
  
        }
        
    }
    public void PlaceItemToLastPosition(Item item)
    {
        item.connectedGrid.PlaceItem(movingItem.connectedGrid, movingItem.lastCoordinate, movingItem, movingItem.lastDirection, item.connectedGrid._gridDirection);
        movingItem.transform.position = item.connectedGrid.GetCellWorldPositionFromCoordinate(movingItem.lastCoordinate);
        movingItem.EnableTrigger();
        movingItem.itemPlacedBefore = true;
        movingItem = null;
    }
    
    public void PickItem()
    {

        if (!Input.GetMouseButtonDown(0))return;

        if (!SendRay(out RaycastHit hit)) return;
        if (hit.transform.TryGetComponent(out ItemMesh itemMesh))
        {
            Item item = itemMesh.GetItem();

            item.lastDirection = item.itemData.direction;
            
            if (item.itemPlacedBefore)
            {
                item.connectedGrid.RemoveItem(item);
            }

            if (item is ItemHasGrid itemHasGrid)
            {
                itemHasGrid.DisableGrid();
            }
            
            item.DisableTrigger();
            
            movingItem = item;
        }
        
    }

    private bool SendRay(out RaycastHit hit)
    {
        hit = new RaycastHit();
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit raycastHit))
            return false;
        
        if(raycastHit.transform == null) 
            return false;
        
        
        hit = raycastHit;
        return true;
    }
    
    public void ChangeWallPlacementType()
    {
        switch (itemPlacementType)
        {
            case ItemPlacementType.Add:
                SetItemPlacementType(ItemPlacementType.Remove);
                
                break;
            case ItemPlacementType.Remove:
                SetItemPlacementType(ItemPlacementType.Add);
                break;
        }
    }
    
    private void SetItemPlacementType(ItemPlacementType type)
    {
        itemPlacementType = type;
    }
}
public enum ItemPlacementType
{
    Add,
    Remove
}