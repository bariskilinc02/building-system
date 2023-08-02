using System;using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallPlacementSystem : PlacementSystemBase
{
    private Camera _camera;

    public List<Transform> wallIdentifiers;
    public Transform wallIdentifier;

    public GridGround currentGrid;
    
    public bool isOnBuilding;
    public WallPlacementType wallPlacementType;
    
    public Vector2Int startPoint;
    public Vector2Int finalPoint;

    public GameObject wallPrefab;

    void Start()
    {
        _camera = Camera.main;
    }
    
    public override void Run()
    {
        PickItem();
        
        DrawPlacement();
        
        PlaceWall();
    }

    public override void OnSystemEnabled()
    {
        
    }

    public override void OnSystemDisabled()
    {
        wallIdentifiers.ForEach((identifier) => identifier.gameObject.SetActive(false));
    }

    private void DrawPlacement()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo)) return;
        
        if(hitInfo.transform == null) return;


        if (hitInfo.transform.TryGetComponent(out GridMesh gridMesh))
        {
            GridBase gridBase = gridMesh.GetGrid();

            if (gridBase is GridGround gridGround)
            {
                switch (wallPlacementType)
                {
                    case WallPlacementType.Add:
                        if (isOnBuilding == false)
                        {
                            currentGrid = gridGround;
                            Vector2Int point = gridGround.GetEdgeVectorPoint(hitInfo.point);
                            DrawWallIdentifierOnCoordinate(point);
                        }
                        else
                        {
                            finalPoint = gridGround.GetEdgeVectorPoint(hitInfo.point);
                            DrawWallIdentifiers();
                        }
                        break;
                    case WallPlacementType.Remove:
                        if (isOnBuilding == false)
                        {
                            currentGrid = gridGround;
                            Vector2Int point = gridGround.GetEdgeVectorPoint(hitInfo.point);
                            DrawWallIdentifierOnCoordinate(point);
                        }
                        else
                        {
                            finalPoint = gridGround.GetEdgeVectorPoint(hitInfo.point);
                            DrawWallIdentifiers();
                        }
                        break;
                }

                
                
            }
        }
    }

    private void PlaceWall()
    {
        if (!Input.GetMouseButtonUp(0))return;

        if (!isOnBuilding) return;

        if (!SendRay(out RaycastHit hit))
        {
            Place();
            isOnBuilding = false;
            return;
        }
            
        
        if (hit.transform.TryGetComponent(out GridMesh gridMesh))
        {
            GridBase gridBase = gridMesh.GetGrid();

            if (gridBase is GridGround gridGround)
            {
                finalPoint = gridGround.GetEdgeVectorPoint(hit.point);
                Place();
            }
        }
        else
        {
            Place();
        }
        
        isOnBuilding = false;

    }

    private void Place()
    {
        switch (wallPlacementType)
        {
            case WallPlacementType.Add:
                CreateAndDrawWall();
                break;
            case WallPlacementType.Remove:
                RemoveWall();
                break;
        }
    }

    private void PickItem()
    {
        if (!Input.GetMouseButtonDown(0))return;

        if (!SendRay(out RaycastHit hit)) return;
        
        if (hit.transform.TryGetComponent(out GridMesh gridMesh))
        {
            GridBase gridBase = gridMesh.GetGrid();

            if (gridBase is GridGround gridGround)
            {
                startPoint = gridGround.GetEdgeVectorPoint(hit.point);
                currentGrid = gridGround;
                isOnBuilding = true;
            }
        }
        
    }

    private void DrawWallIdentifiers()
    {
        wallIdentifiers.ForEach((identifier) => identifier.gameObject.SetActive(false));
        
        VectorDirection direction = currentGrid.GetVectorDirectionAndLength(startPoint, finalPoint, out int length);

        List<Vector2Int> corners = currentGrid.GetEdgeCornersInRange(startPoint, direction, length + 1);

        for (int i = 0; i < corners.Count; i++)
        {
            wallIdentifiers[i].gameObject.SetActive(true);
            Vector3 previewPosition = currentGrid.GetCornerWorldPosition(corners[i]);

            wallIdentifiers[i].position = previewPosition;
        }

    }

    private void DrawWallIdentifierOnCoordinate(Vector2Int coordinate)
    {
        wallIdentifiers.ForEach((identifier) => identifier.gameObject.SetActive(false));
        Vector3 previewPosition = currentGrid.GetCornerWorldPosition(coordinate);
        wallIdentifiers[0].gameObject.SetActive(true);
        wallIdentifiers[0].position = previewPosition;
    }

    private void CreateAndDrawWall()
    {
        VectorDirection direction = currentGrid.GetVectorDirectionAndLength(startPoint, finalPoint, out int length);

        List<VectorEdge> edgeVectors = currentGrid.GetEdgeVectorsInRange(startPoint, direction, length);

       

        WallDirection wallDirection = direction is VectorDirection.Bottom or VectorDirection.Top
            ? WallDirection.Horizontal
            : WallDirection.Vertical;
 
        
        
        for (int i = 0; i < edgeVectors.Count; i++)
        {
            if(IsAnyItemInEdge(edgeVectors[i], wallDirection))
            {
                continue;
            }
            
            if (currentGrid.edges[edgeVectors[i].Fix()].wall != null)
            {
                continue;
            }
            
            Vector3 edgeWorldPosition = currentGrid.GetEdgeWorldPosition(edgeVectors[i]);

            float wallAngle = 0;

            wallAngle = direction is VectorDirection.Left or VectorDirection.Right ? 0 : 90;
            
            GameObject instantWall = Instantiate(wallPrefab);
            instantWall.transform.position = edgeWorldPosition;
            instantWall.transform.eulerAngles = new Vector3(0,wallAngle,0);
            instantWall.GetComponent<Wall>().wallData.wallDirection = wallDirection;

            currentGrid.edges[edgeVectors[i].Fix()].wall = instantWall;
        }
    }
    
    private void RemoveWall()
    {
        VectorDirection direction = currentGrid.GetVectorDirectionAndLength(startPoint, finalPoint, out int length);

        List<VectorEdge> edgeVectors = currentGrid.GetEdgeVectorsInRange(startPoint, direction, length);

        for (int i = 0; i < edgeVectors.Count; i++)
        {
            Edge edge = currentGrid.edges[edgeVectors[i].Fix()];
            if (edge.wall != null)
            {
                Destroy(edge.wall);
            }
        }
    }

    private bool IsAnyItemInEdge(VectorEdge edgeVector, WallDirection wallDirection)
    {
        bool result = false;
        if (wallDirection == WallDirection.Vertical)
        {
            if (!currentGrid.cells.TryGetValue(edgeVector.Fix().smallerPoint + new Vector2Int(0, 1), out Cell cellRight))
            {
                return false;
            }
            
            //Cell cellOnRight = currentGrid.cells[edgeVector.Fix().smallerPoint];
            Cell cellOnRight = cellRight;
            
            if (!currentGrid.cells.TryGetValue(edgeVector.Fix().smallerPoint - new Vector2Int(0, 1), out Cell cell))
            {
                return false;
            }

            Cell cellOnLeft = cell;

            if (cellOnRight.placedItem == cellOnLeft.placedItem && cellOnRight.placedItem != null)
            {
                result = true;
            }
        }
        else
        {
       
            if (!currentGrid.cells.TryGetValue(edgeVector.Fix().smallerPoint + new Vector2Int(1, 0), out Cell cellTop))
            {
                return false;
            }
            //Cell cellOnTop = currentGrid.cells[edgeVector.Fix().smallerPoint];
            Cell cellOnTop = cellTop;
            
            if (!currentGrid.cells.TryGetValue(edgeVector.Fix().smallerPoint - new Vector2Int(1, 0), out Cell cell))
            {
                return false;
            }
            
            Cell cellOnBottom = cell;

            if (cellOnTop.placedItem == cellOnBottom.placedItem && cellOnTop.placedItem != null)
            {
                result = true;
            }
        }

        return result;
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

    private void SetWallPlacementType(WallPlacementType type)
    {
        wallPlacementType = type;
    }
    
    public void ChangeWallPlacementType()
    {
        switch (wallPlacementType)
        {
            case WallPlacementType.Add:
                SetWallPlacementType(WallPlacementType.Remove);
                break;
            case WallPlacementType.Remove:
                SetWallPlacementType(WallPlacementType.Add);
                break;
        }
    }
}

public enum WallDirection
{
    Vertical,
    Horizontal
}

public enum WallPlacementType
{
    Add,
    Remove
}