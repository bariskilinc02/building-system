using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerContainer : SingletonBehaviour<ManagerContainer>
{
    public List<MonoBehaviour> Managers;

    public T GetInstance<T>() where T : MonoBehaviour
    {
        MonoBehaviour instance = Managers.Find(x => x as T != null);

        return instance as T;
    }
}
