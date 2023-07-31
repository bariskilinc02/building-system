using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WallDesignSystem : PlacementSystemBase
{
    private Camera _camera;

    public Wall currentWall;
    public Wall movingWall;

    private void Start()
    {
        _camera = Camera.main;
    }

    public override void Run()
    {
        PreviewWall();

        SetWall();
    }

    public override void OnSystemEnabled()
    {

    }

    public override void OnSystemDisabled()
    {
        if (currentWall != null)
        {
            currentWall.wallMesh.EnableModel();
            currentWall.wallMesh.EnableInteraction();
        }
        
        if(movingWall != null)
        {
            Destroy(movingWall.gameObject);
            movingWall = null;
        }
    }

    public void PreviewWall()
    {
        if(movingWall == null) return;

        if (!SendRay(out RaycastHit hit))
        {
            movingWall.gameObject.SetActive(false);
            if (currentWall != null)
            {
                currentWall.wallMesh.EnableModel();
                currentWall.wallMesh.EnableInteraction();
                currentWall = null;
            }
            return;    
        }

        if (currentWall != null)
        {
            movingWall.gameObject.SetActive(true);
        }
       

        if (hit.transform.TryGetComponent(out WallMesh wallMesh))
        {
            if (!(currentWall == wallMesh.wall))
            {
                if (currentWall != null)
                {
                    currentWall.wallMesh.EnableModel();
                }
                
                currentWall = wallMesh.wall;
                currentWall.wallMesh.DisableModel();
            }
        }

        if (wallMesh != null)
        {
            movingWall.transform.position = wallMesh.wall.transform.position;
            
            float yAngle = currentWall.wallData.GetWallAngleY();
            var eulerAngles = movingWall.transform.eulerAngles;
            movingWall.transform.eulerAngles = new Vector3(eulerAngles.x, yAngle, eulerAngles.z);
            Debug.Log(yAngle);
        }
    }

    public void SetWall()
    {
        if (!Input.GetMouseButtonUp(0))return;
        
        if(movingWall == null) return;
        
        if (!SendRay(out RaycastHit hit))
        {
            return;    
        }
        
        if (hit.transform.TryGetComponent(out WallMesh wallMesh))
        {
            Debug.Log("zort");
            currentWall.wallMesh.ChangeModel(movingWall.wallMesh.model);
            if (!(currentWall == wallMesh.wall))
            {
                if (currentWall != null)
                {
                    currentWall.wallMesh.EnableModel();
                }
           
                currentWall = wallMesh.wall;
                currentWall.wallMesh.DisableModel();

                
                //movingWall.wallMesh.model.transform.SetParent(currentWall.wallMesh.transform);
            }
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
