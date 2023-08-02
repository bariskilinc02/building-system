using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMesh : MonoBehaviour
{
    public Wall wall;

    public GameObject model;
    public Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void EnableModel()
    {
        model.gameObject.SetActive(true);
    }

    public void DisableModel()
    {
        model.gameObject.SetActive(false);
    }


    public void EnableInteraction()
    {
        _collider.enabled = true;
    }

    public void DisableInteraction()
    {
        _collider.enabled = false;
    }

    public void ChangeModel(GameObject newModel)
    {   
        
        Destroy(model.gameObject);
        model = Instantiate(newModel, transform);
        EnableInteraction();
        EnableModel();
        
    }
}
