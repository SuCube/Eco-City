using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform cameraPoint; 
    [SerializeField, Range(0, 100)] private float moveSpeed = 50.0f;
    [SerializeField, Range(0, 100)] private float rotationSpeed = 30.0f;
    [SerializeField, Range(0, 100)] private float zoomSpeed = 25.0f;
    [Space]
    /*[Header("Camera Movement Limits")]
    [SerializeField] private float minX = -100.0f;
    [SerializeField] private float maxX = 100.0f;
    [SerializeField] private float minZ = -100.0f;
    [SerializeField] private float maxZ = 100.0f;*/
    [Space]
    [Header("Camera Rotation Limits")]
    [SerializeField] private float minRotationX = 20.0f;
    [SerializeField] private float maxRotationX = 70.0f;
    private float verticalAngle = 0.0f;

    void Update()
    {
        // Вращение камеры вокруг сферы при зажатой ПКМ
        if (Input.GetMouseButton(1)) // ПКМ
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * 10 * Time.deltaTime;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * 10 * Time.deltaTime;

            verticalAngle -= rotationY;
            verticalAngle = Mathf.Clamp(verticalAngle, minRotationX, maxRotationX);

            //transform.RotateAround(cameraPoint.position, Vector3.up, rotationX);
            //transform.eulerAngles = new Vector3(verticalAngle, transform.eulerAngles.y, 0);

            transform.RotateAround(cameraPoint.position, Vector3.up, rotationX);
            transform.RotateAround(cameraPoint.position, transform.right, -rotationY);
        }

        // Перемещение камеры при зажатой ЛКМ
        if (Input.GetMouseButton(0)) // ЛКМ
        {
            float moveX = Input.GetAxis("Mouse X") * moveSpeed * 10 * Time.deltaTime;
            float moveZ = Input.GetAxis("Mouse Y") * moveSpeed * 10 * Time.deltaTime;

            //Vector3 forward = transform.forward;
            //Vector3 right = transform.right;

            //Vector3 move = (forward * -moveZ + right * moveX);
            //transform.position += move * moveSpeed; // Перемещение камеры

            Vector3 moveDirection = new Vector3(moveX, 0.0f, moveZ);
            transform.Translate(moveDirection);
        }

        // Зум
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && playerCamera != null)
        {
            playerCamera.fieldOfView -= scroll * zoomSpeed * 2;
            playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, 15f, 90f); // Ограничение FOV
        }
    }
}
