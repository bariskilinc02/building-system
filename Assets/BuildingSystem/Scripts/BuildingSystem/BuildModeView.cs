using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuildModeView : MonoBehaviour
{ 
    public Button onOffBuildModeButton;
    
    public Button buildTypeWallButton;
    public Button changeWallPlacementTypeButton;
    
    public Button buildTypeItemButton;
    
    public Button buildTypeWallDesignButton;

    public GameObject itemBuildingScreen;
    public GameObject wallBuildingScreen;
    public GameObject wallDesignScreen;
    private void Awake()
    {
        onOffBuildModeButton.onClick.AddListener(OnClickedOnOffBuildModeButton);
        
        buildTypeWallButton.onClick.AddListener(OnClickedBuildTypeWallButton);
        changeWallPlacementTypeButton.onClick.AddListener(OnClickedChangeWallPlacementTypeButton);
        
        buildTypeItemButton.onClick.AddListener(OnClickedBuildTypeItemButton);
        
        buildTypeWallDesignButton.onClick.AddListener(OnClickedBuildTypeWallDesignButton);
    }

    private void OnClickedOnOffBuildModeButton()
    {
        bool status = BuildingSystem.Instance.inBuildMode;
        if (status)
        {
            BuildingSystem.Instance.inBuildMode = false;
            BuildingSystem.Instance.OnBuildModeDisabled?.Invoke();
            itemBuildingScreen.SetActive(false);
            wallBuildingScreen.SetActive(false);
            wallDesignScreen.SetActive(false);
        }
        else
        {
            BuildingSystem.Instance.inBuildMode = true;
            BuildingSystem.Instance.OnBuildModeEnabled?.Invoke();
        }
      

    }
    
    private void OnClickedBuildTypeWallButton()
    {
        BuildingSystem.Instance.SetBuildType(BuildType.Wall);
        itemBuildingScreen.SetActive(false);
        wallBuildingScreen.SetActive(true);
        wallDesignScreen.SetActive(false);
    }
    
    private void OnClickedBuildTypeItemButton()
    {
        BuildingSystem.Instance.SetBuildType(BuildType.Item);
        itemBuildingScreen.SetActive(true);
        wallBuildingScreen.SetActive(false);
        wallDesignScreen.SetActive(false);
    }
    
    private void OnClickedBuildTypeWallDesignButton()
    {
        BuildingSystem.Instance.SetBuildType(BuildType.WallDesign);
        itemBuildingScreen.SetActive(false);
        wallBuildingScreen.SetActive(false);
        wallDesignScreen.SetActive(true);
    }
    
    private void OnClickedChangeWallPlacementTypeButton()
    {
        BuildingSystem.Instance.wallPlacer.ChangeWallPlacementType();
    }
}
