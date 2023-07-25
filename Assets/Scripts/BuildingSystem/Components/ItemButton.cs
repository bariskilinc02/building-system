using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private Button _button;

    public int index;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickedButton);
    }

    public void OnClickedButton()
    {
        BuildingSystem.Instance.CreateNewItem(index);
    }
}
