using System;
using System.Collections.Generic;
using UnityEngine;

public class GridBase : MonoBehaviour
{
    public Vector2Int gridSize;
    public GridMesh _gridMesh;
    public int cellsPerUnit;
    protected Vector3 _rootPosition => transform.position;
    public Dictionary<Vector2Int, Cell> cells;
    public List<Item> items;

    private void Awake()
    {
        _gridMesh = GetComponentInChildren<GridMesh>();
    }

    protected virtual void Start()
    {
        CreateGrid();
    }

    [Method()]
    public void CreateGrid()
    {
        transform.localScale = new Vector3(gridSize.x, 1, gridSize.y);
        cells = new Dictionary<Vector2Int, Cell>();
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                cells.Add(new Vector2Int(i, j), new Cell());
            }
        }
        
    }
    
    public Vector2Int GetCellCoordinate(Vector3 position)
    {
        var actualPosition = position - _rootPosition;
        
        var actualCoordinate = new Vector2(actualPosition.x, actualPosition.z) * cellsPerUnit;
        var result = new Vector2Int(Mathf.FloorToInt(actualCoordinate.x),
            Mathf.FloorToInt(actualCoordinate.y));
        
        return result;
    }
    
    public Vector3 GetCellPosition(Vector3 position)
    {
        var actualPosition = position;
        
        var result = new Vector3(Mathf.FloorToInt(actualPosition.x), _rootPosition.y,
            Mathf.FloorToInt(actualPosition.z));
        
        return result;
    }
    
    public Vector3 GetCellWorldPositionFromCoordinate(Vector2Int coordinate)
    {
        var actualPosition = _rootPosition + new Vector3(coordinate.x, _rootPosition.y, coordinate.y);
        
        return actualPosition;
    }

    public void PlaceItem(GridBase gridBase ,Vector2Int coordinate, Item item)
    {
        item.lastCoordinate = coordinate;
        item.connectedGrid = gridBase;
        
        Vector2Int cellToPlaced = coordinate;
        
        for (int i = 0; i < item.itemData.size.x; i++)
        {
            for (int j = 0; j < item.itemData.size.y; j++)
            {
                cells[cellToPlaced].placedItem = item;
                item.occupiedCells.Add(cells[cellToPlaced]);
                
                cellToPlaced.y += 1;
            }

            cellToPlaced.y = coordinate.y;
            cellToPlaced.x += 1;
        }
    }
    
    public void RemoveItem(Item item)
    {
        foreach (var occupiedCell in item.occupiedCells)
        {
            //item.connectedGrid = null;
            occupiedCell.placedItem = null;
        }
    }

    #region Occupations
    private bool IsCellOccupied(Vector2Int coordinate)
    {
        return cells[coordinate].IsOccupied();
    }

    public bool IsCellsEmpty(Vector2Int coordinate, Vector2Int itemSize)
    {
        bool result = false;

        Vector2Int cellToSearch = coordinate;
        
        for (int i = 0; i < itemSize.x; i++)
        {
            for (int j = 0; j < itemSize.y; j++)
            {
                if (!cells.TryGetValue(cellToSearch, out Cell cell))
                    return false;
              
                
                if (cells[cellToSearch].IsOccupied())
                {
                    return false;
                }

                result = true;

                cellToSearch.y += 1;
                
                //Debug.Log(cellToSearch);
            }

            cellToSearch.y = coordinate.y;
            cellToSearch.x += 1;
        }
        
        return result;
    }

        #endregion
   
    
    public Cell GetCell(Vector2Int cellCoordinate)
    {
        return cells[cellCoordinate];
    }

    public void EnableGrid()
    {
        _gridMesh.gameObject.SetActive(true);
    }
    
    public void DisableGrid()
    {
        _gridMesh.gameObject.SetActive(false);
    }
    
}
