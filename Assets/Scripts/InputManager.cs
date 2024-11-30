// InputManager.cs
// УБРАТЬ ИВЕНТЫ (CellMenu спамит несколько раз, ивенты накапливаются)

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] 
    private Camera _sceneCamera;
    
    [SerializeField] 
    private LayerMask _placementLayerMask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (IsPointerOverUI()) return;

        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public bool GetSelectedMapPosition(out Vector3 _selectorPosition)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _sceneCamera.nearClipPlane;

        Ray ray = _sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, _placementLayerMask))
        {
            _selectorPosition = hit.point;
            return true;
        }
        _selectorPosition = Vector3.zero;
        return false;
    }
}
