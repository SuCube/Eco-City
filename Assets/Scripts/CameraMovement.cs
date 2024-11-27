using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Написано по видосу https://www.youtube.com/watch?v=rnqF6S7PfFA
    // Модернезированно и подстроено под проект мною :D

    // Этот код, на сцене объекты (Player и его объекты) и компоненты, код в WorldStatistic, исправления в GridSystem перенести в новый проект (сделано)
    // Управление:
    // WASD / Стрелки / ЛКМ = Передвижение камеры (ЛКМ в разработке (и отключено))
    // Left Shift = Ускоренное передвижение
    // Q,E / ПКМ = Вращение
    // R,F / Колёсико мыши = Зум
    // C = Возвращает вращение и зум в стандартное положение 

    [Header("Movement settings")]
    [SerializeField] private float normalSpeed = 0.5f;
    [SerializeField] private float shiftSpeed = 1.0f;
    [SerializeField] private float movementTime = 5.0f;
    [Header("Movement limits")]
    [SerializeField] private float minMovementX = -60.0f;
    [SerializeField] private float maxMovementX = 60.0f;
    [SerializeField] private float minMovementZ = -60.0f;
    [SerializeField] private float maxMovementZ = 60.0f;
    private float movementSpeed = 0.0f;
    private Vector3 newPosition;

    [Space]
    [Space]

    [Header("Rotation and zoom settings")]
    [SerializeField] private float rotationAmount = 0.5f;
    [SerializeField, Range(0, 1f)] private float zoomAmount = 0.2f;
    [Header("Zoom limits")]
    [SerializeField] private float maxZoom = 10.0f;
    [SerializeField] private float minZoom = 40.0f;
    private Quaternion newRotation;
    private float newZoom;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;

    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    private float defaultRotationY = 45.0f;
    private float defaultZoom = 20.0f;

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;

        newZoom = Camera.main.orthographicSize;
    }

    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        } */

        if (Input.GetMouseButtonDown(1))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 differance = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-differance.x / 5.0f));
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount * 10.0f;
        }
    }

    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = shiftSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * +rotationAmount);
        }

        if (Input.GetKey(KeyCode.R))
        {
            newZoom -= zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.C))
        {
            newRotation = Quaternion.Euler(Vector3.up * defaultRotationY);
            newZoom = defaultZoom;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, minMovementX, maxMovementX);
        newPosition.z = Mathf.Clamp(newPosition.z, minMovementZ, maxMovementZ);

        newZoom = Mathf.Clamp(newZoom, maxZoom, minZoom);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, newZoom, Time.deltaTime * movementTime);
    } 
}
