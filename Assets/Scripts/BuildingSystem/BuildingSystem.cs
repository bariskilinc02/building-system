using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingSystem : SingletonBehaviour<BuildingSystem>
{

    [SerializeField] private PlacementSystemBase currentPlacementSystemBase;
    public ObjectPlacementSystem objectPlacementSystem;
    public WallPlacementSystem wallPlacer;
    public WallDesignSystem WallDesigner;
    
    public BuildType currentBuildType;
    public bool inBuildMode;

    public Action OnWallPlacementSystemEnabled;
    public Action OnWallPlacementSystemDisabled;
    
    public Action OnObjectPlacementSystemEnabled;
    public Action OnObjectPlacementSystemDisabled;
    
    public Action OnBuildModeEnabled;
    public Action OnBuildModeDisabled;

    public List<Item> items;

    public List<Wall> wallPrefabs;

    
    private void Start()
    {
        currentBuildType = BuildType.Null;
    }

    private void Update()
    {
        if (currentPlacementSystemBase == null && inBuildMode == false) return;
        
        currentPlacementSystemBase.Run();
    }

    public void SetBuildType(BuildType type)
    {
        if (type == BuildType.Item)
        {
            if (currentBuildType == BuildType.Item) return;
            
            if (currentPlacementSystemBase != null)
            {
                currentPlacementSystemBase.OnSystemDisabled();
            }
            currentPlacementSystemBase = objectPlacementSystem;
            
            currentPlacementSystemBase.OnSystemEnabled();
            
            OnWallPlacementSystemDisabled?.Invoke();
        }
        else if(type == BuildType.Wall)
        {
            if (currentBuildType == BuildType.Wall) return;
            
            if (currentPlacementSystemBase != null)
            {
                currentPlacementSystemBase.OnSystemDisabled();
            }
       
            currentPlacementSystemBase = wallPlacer;
            if (currentPlacementSystemBase != null)
            {
                currentPlacementSystemBase.OnSystemEnabled();
            }
            
            //OnWallPlacementSystemEnabled?.Invoke();
        }
        else if(type == BuildType.WallDesign)
        {
            if (currentBuildType == BuildType.WallDesign) return;
            
            if (currentPlacementSystemBase != null)
            {
                currentPlacementSystemBase.OnSystemDisabled();
            }
       
            currentPlacementSystemBase = WallDesigner;
            if (currentPlacementSystemBase != null)
            {
                currentPlacementSystemBase.OnSystemEnabled();
            }
            
            //OnWallPlacementSystemEnabled?.Invoke();
        }
        currentBuildType = type;
        
        EnableBuildMode();
    }
    
    public void EnableBuildMode()
    {
        inBuildMode = true;
    }
    
    public void DisableBuildMode()
    {
        inBuildMode = false;
    }

    public void CreateNewItem(int index)
    {
        if (objectPlacementSystem.movingItem == null)
        {
            objectPlacementSystem.movingItem = Instantiate(items[index]);
        }
        else if (objectPlacementSystem.movingItem != null)
        {
            if (objectPlacementSystem.movingItem.itemPlacedBefore)
            {
                objectPlacementSystem.PlaceItemToLastPosition(objectPlacementSystem.movingItem);
            }
            else
            {
                Destroy(objectPlacementSystem.movingItem.gameObject);
                objectPlacementSystem.movingItem = Instantiate(items[index]);
            }
        }
    }

    public void SelectWallDesignPrefab(int index)
    {
        if (WallDesigner.movingWall == null)
        {
            WallDesigner.movingWall = Instantiate(wallPrefabs[index]);
            WallDesigner.movingWall.wallMesh.DisableInteraction();
        }
        else if (WallDesigner.movingWall != null)
        {
            Destroy(WallDesigner.movingWall.gameObject);
            WallDesigner.movingWall = Instantiate(wallPrefabs[index]);
            WallDesigner.movingWall.wallMesh.DisableInteraction();
        }
    }
}

public enum BuildType{
    Null,
    Item,
    Wall,
    WallDesign
}