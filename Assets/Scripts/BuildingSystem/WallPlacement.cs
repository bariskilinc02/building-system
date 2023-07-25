using System;using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallPlacement : MonoBehaviour
{
    private Camera _camera;

    public List<Transform> wallIdentifiers;
    public Transform wallIdentifier;

    public GridGround currentGrid;
    
    public bool isOnBuilding;
    public Vector2Int startPoint;
    public Vector2Int finalPoint;

    public GameObject wallPrefab;

    void Start()
    {
        _camera = Camera.main;
    }


    void Update()
    {
        PickItem();
        
        DrawPlacement();
        
        PlaceItem();
    }

    public void DrawPlacement()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo)) return;
        
        if(hitInfo.transform == null) return;


        if (hitInfo.transform.TryGetComponent(out GridMesh gridMesh))
        {
            GridBase gridBase = gridMesh.GetGrid();

            if (gridBase is GridGround gridGround)
            {
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
                
            }
            
            
            //Vector2Int cellCoordinate = gridBase.GetCellCoordinate(hitInfo.point);
            //var result = gridBase.IsCellsEmpty(cellCoordinate, movingItem.itemData.size);
            
            //movingItem.transform.position = gridBase.GetCellPosition(hitInfo.point);
            //Debug.Log(result);
        }
    }

    public void PlaceItem()
    {
        if (!Input.GetMouseButtonUp(0))return;

        if (!isOnBuilding) return;

        if (!SendRay(out RaycastHit hit))
        {
            isOnBuilding = false;
            return;
        }
            
        
        if (hit.transform.TryGetComponent(out GridMesh gridMesh))
        {
            GridBase gridBase = gridMesh.GetGrid();

            if (gridBase is GridGround gridGround)
            {
                finalPoint = gridGround.GetEdgeVectorPoint(hit.point);
                CreateAndDrawWall();
            }
        }

        isOnBuilding = false;

    }

    public void PickItem()
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

        for (int i = 0; i < edgeVectors.Count; i++)
        {
            if (currentGrid.edges[edgeVectors[i].Fix()].wall != null)
            {
                Debug.Log("zorting");
                continue;
            }
            
            Vector3 edgeWorldPosition = currentGrid.GetEdgeWorldPosition(edgeVectors[i]);

            float wallAngle = 0;

            wallAngle = direction is VectorDirection.Left or VectorDirection.Right ? 0 : 90;
            
            GameObject instantWall = Instantiate(wallPrefab);
            instantWall.transform.position = edgeWorldPosition;
            instantWall.transform.eulerAngles = new Vector3(0,wallAngle,0);
            
            currentGrid.edges[edgeVectors[i].Fix()].wall = instantWall;
            
            //Debug.Log(edgeVectors[i].GetMiddlePoint());
            //Debug.Log(edgeVectors[i].smallerPoint +"  " + edgeVectors[i].biggerPoint);
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
}

public enum WallDirection
{
    Vertical,
    Horizontal
}