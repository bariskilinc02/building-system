using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    #region CameraTPS
    [Header("TPSLook")]
    public Camera Camera;
    public Transform lookAt;

    private const float yMin = 0;
    private const float yMax = 89.0f;


    [SerializeField] private float distance = 12.0f;
    [SerializeField] private float currentX = 50.0f;
    [SerializeField] private float currentY = 50.0f;
    [SerializeField] private float sensivity = 800.0f;

    private bool invertMouseLook;

    #endregion

    private bool autoLockCursor;

    #region Unity Functions

    public void Init()
    {
        Cursor.lockState = (autoLockCursor) ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void Start()
    {
        Init();
        
        CameraTPSLook();
        
    }

    private void Update()
    {
        if(Input.GetMouseButton(1))
            CameraTPSLook();
    }
    #endregion

    #region Camera Look
    private void CameraTPSLook()
    {
        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * (invertMouseLook ? 1 : -1) * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, yMin, yMax);

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Camera.transform.position = lookAt.position + rotation * Direction;

        Camera.transform.LookAt(lookAt.position);
    }
    #endregion
   
    #region Set Camera Target Transform
    private void SetCameraTransformTo(Transform transform)
    {
        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * (invertMouseLook ? 1 : -1) * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, yMin, yMax);

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * Direction;

        transform.LookAt(lookAt.position);
    }
    #endregion

}
