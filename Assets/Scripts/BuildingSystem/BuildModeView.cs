using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeView : MonoBehaviour
{
    public Button enableBuildModeButton;
    public Button disableBuildModeButton;
    
    public Button buildTypeWallButton;
    public Button buildTypeItemButton;

    private void Awake()
    {
        enableBuildModeButton.onClick.AddListener(OnClickedEnableBuildModeButton);
        disableBuildModeButton.onClick.AddListener(OnClickedDisableBuildModeButton);
        
        buildTypeWallButton.onClick.AddListener(OnClickedBuildTypeWallButton);
        buildTypeItemButton.onClick.AddListener(OnClickedBuildTypeItemButton);
    }

    private void OnClickedEnableBuildModeButton()
    {
        BuildingSystem.Instance.inBuildMode = true;
        BuildingSystem.Instance.OnBuildModeEnabled?.Invoke();
    }
    
    private void OnClickedDisableBuildModeButton()
    {
        BuildingSystem.Instance.inBuildMode = false;
        BuildingSystem.Instance.OnBuildModeDisabled?.Invoke();
    }
    
    private void OnClickedBuildTypeWallButton()
    {
        BuildingSystem.Instance.SetBuildType(BuildType.Wall);
    }
    
    private void OnClickedBuildTypeItemButton()
    {
        BuildingSystem.Instance.SetBuildType(BuildType.Item);
    }
}
