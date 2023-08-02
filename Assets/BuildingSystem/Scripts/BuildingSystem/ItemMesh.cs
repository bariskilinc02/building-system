using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMesh : MonoBehaviour
{
    [SerializeField] private Item item; 
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public Item GetItem()
    {
        return item;
    }

    public void EnableTrigger()
    {
        _collider.enabled = true;
    }
    
    public void DisableTrigger()
    {
        _collider.enabled = false;
    }
}
