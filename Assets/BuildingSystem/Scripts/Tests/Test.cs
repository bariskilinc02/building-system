using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject testObject;
    public GameObject testGameObject;
    public Test test;
    
    void Start()
    {
        testObject.transform.SetPosition(1,1,1);
        testObject.transform.SetEulerRotation(5,1,50);
        //testGameObject = testObject.transform.FindChildWithComponent<Test>().gameObject;
    }

}
