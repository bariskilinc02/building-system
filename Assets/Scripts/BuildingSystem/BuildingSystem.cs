using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : SingletonBehaviour<BuildingSystem>
{
    public ObjectPlacer objectPlacer;

    public BuildType buildType;
    public bool inBuildMode;

    public Action OnBuildModeEnabled;
    public Action OnBuildModeDisabled;

    public List<Item> items;


    public void SetBuildType(BuildType type)
    {
        buildType = type;
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
        if (objectPlacer.movingItem == null)
        {
            objectPlacer.movingItem = Instantiate(items[index]);
        }
        else if (objectPlacer.movingItem != null)
        {
            if (objectPlacer.movingItem.itemPlacedBefore)
            {
                objectPlacer.PlaceItemToLastPosition(objectPlacer.movingItem);
            }
            else
            {
                Destroy(objectPlacer.movingItem.gameObject);
                objectPlacer.movingItem = Instantiate(items[index]);
            }
        }
    }

}

public enum BuildType{
    Item,
    Wall
}