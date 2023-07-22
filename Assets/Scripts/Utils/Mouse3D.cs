using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour {

    public static Mouse3D Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

    private void Awake() {
        Instance = this;
    }
    
    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();
    public static bool GetComponentMouseClick<T>(out T t) => Instance.GetComponentMouseClick_Instance<T>(out t); 
    private Vector3 GetMouseWorldPosition_Instance() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
            return raycastHit.point;
        } else {
            return Vector3.zero;
        }
    }
    
    private bool GetComponentMouseClick_Instance<T>(out T t) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
        {
            t = raycastHit.transform.GetComponent<T>();
            //return t != null ? t : null;
            return  t != null ? true : false;
        }
        else
        {
            t = default;
            return false;
        }
    }

}
