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

    public void PlaceItem(GridBase gridBase ,Vector2Int coordinate, Item item, Direction direction)
    {
        item.itemData.direction = direction;
        item.SetRotation();
        item.lastCoordinate = coordinate;
        item.connectedGrid = gridBase;
        
        Vector2Int cellToPlaced = coordinate;

        var coordinateList = GetAreaCoordinateList(coordinate, item.itemData.size, item.itemData.direction);
        
        foreach (var coordinateInList in coordinateList)
        {
            cells[coordinateInList].placedItem = item;
            item.occupiedCells.Add(cells[coordinateInList]);
        } 
        /*
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
        */
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

    public bool IsCellsEmpty(Vector2Int coordinate, Vector2Int itemSize, Direction direction)
    {
        bool result = false;

        Vector2Int cellToSearch = coordinate;

        var coordinateList = GetAreaCoordinateList(coordinate, itemSize, direction);

        for (int i = 0; i < coordinateList.Count; i++)
        {
            if (!cells.TryGetValue(coordinateList[i], out Cell cell))
                return false;
            
            if (cells[coordinateList[i]].IsOccupied())
                return false;
            
            result = true;
        }
        
        return result;
    }

    public List<Vector2Int> GetAreaCoordinateList(Vector2Int coordinate, Vector2Int itemSize, Direction direction)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        
        Vector2Int cellToSearch = coordinate;

        #region 0 degree

        if (direction == Direction._0)
        {
            for (int i = 0; i < itemSize.x; i++)
            {
                for (int j = 0; j < itemSize.y; j++)
                {
                    result.Add(cellToSearch);
                    cellToSearch.y += 1;
                }

                cellToSearch.y = coordinate.y;
                cellToSearch.x += 1;
            }
        }
        #endregion
        #region 90 degree
        else if (direction == Direction._90)
        {
            for (int i = 0; i < itemSize.y; i++)
            {
                for (int j = 0; j < itemSize.x; j++)
                {
                    result.Add(cellToSearch);
                    cellToSearch.y -= 1;
                }

                cellToSearch.y = coordinate.y;
                cellToSearch.x += 1;
            }
        }
        #endregion
        #region 180 degree
        else if (direction == Direction._180)
        {
            for (int i = 0; i < itemSize.x; i++)
            {
                for (int j = 0; j < itemSize.y; j++)
                {
                    result.Add(cellToSearch);
                    cellToSearch.y += 1;
                }

                cellToSearch.y = coordinate.y;
                cellToSearch.x -= 1;
            }
        }
        #endregion
        #region 270 degree
        else if (direction == Direction._270)
        {
            for (int i = 0; i < itemSize.y; i++)
            {
                for (int j = 0; j < itemSize.x; j++)
                {
                    result.Add(cellToSearch);
                    cellToSearch.y += 1;
                }

                cellToSearch.y = coordinate.y;
                cellToSearch.x += 1;
            }

        }
        #endregion
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
