using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private Button _button;
    [SerializeField] private TextMeshProUGUI text;

    public int index;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickedButton);
    }

    private void Start()
    {
        text.text = BuildingSystem.Instance.items[index].itemId;
    }

    public void OnClickedButton()
    {
        BuildingSystem.Instance.CreateNewItem(index);

    }
}
