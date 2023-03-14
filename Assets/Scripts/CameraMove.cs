using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform _follower;
    [SerializeField] Transform _realCam;

    float rotX;
    float rotY;

    [SerializeField] float minClamp = -45f;
    [SerializeField] float maxClamp = 25f;

    [SerializeField] float sensitivity = 200f;

    [SerializeField] float followSpeed = 5f;

    Vector3 finalDir;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float minDistance = 2f;

    [SerializeField] float smoothness = 3f;
    
    Vector3 dirNormal;
    float finalDis;

    bool _isMouseVisible = false;

    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormal = _realCam.localPosition.normalized;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isMouseVisible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _isMouseVisible = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _isMouseVisible = true;
        }

        if (_isMouseVisible) return;

        rotX += Input.GetAxis("Mouse Y") * -1 * Time.deltaTime * sensitivity;
        rotY += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        rotX = Mathf.Clamp(rotX, minClamp, maxClamp);

        Quaternion rot = Quaternion.Euler(rotX, rotY, 0f);
        transform.rotation = rot;

        
    }

    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _follower.position, Time.deltaTime * followSpeed);

        finalDir = transform.TransformPoint(dirNormal * maxDistance);

        RaycastHit hit;
        //Debug.DrawLine(transform.position, finalDir, Color.green, 5f);
        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            finalDis = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDis = maxDistance;
        }

        _realCam.localPosition = Vector3.Lerp(_realCam.localPosition, dirNormal * finalDis, Time.deltaTime * smoothness);
    }
}
