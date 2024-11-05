using System;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private GameObject _cellIndicator;
    [SerializeField] private GameObject _cellMenu;

    [SerializeField] private ObjectDatabaseSO _objectDatabase;
    private int _selectedObjectIndex = -1;

    [SerializeField] private GameObject _gridVisualization;
    [SerializeField] private Transform _objectsParent;

    private Vector3 _indicatorPosition;
    GridMatrix objectsMatrix;
    private class GridMatrix
    {
        private int[,,] matrix;
        private Vector3Int startingPoint;

        public GridMatrix(Vector3Int size, Vector3Int startingPoint)
        {
            matrix = new int[size.x, 1, size.z];
            this.startingPoint = startingPoint;

            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.x; j++)
                    matrix[i, 0, j] = -1;
        }

        public int this[Vector3Int coords]
        {
            get
            {
                Vector3Int result = NormalizeIndex(coords);
                return matrix[result.x, 0, result.z];
            }
            set
            {
                Vector3Int result = NormalizeIndex(coords);
                matrix[result.x, 0, result.z] = value;
            }
        }
        Vector3Int NormalizeIndex(Vector3Int coords)
        {
            Vector3Int result = coords + startingPoint;
            return result;
        }
    }
    void Start()
    {
        StopPlacement();
        objectsMatrix = new GridMatrix(new Vector3Int(10,1,10), new Vector3Int(5,0,5));
    }

    public void StartPlacement(int ID)
    {
        //StopPlacement();
        _selectedObjectIndex = _objectDatabase.objectsData.FindIndex(data => data.ID == ID);
        if (_selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        _gridVisualization.SetActive(true);
        _cellIndicator.SetActive(true);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
        _inputManager.OnClicked -= ShowCellMenu;

    }

    void Update()
    {
        //if (_selectedObjectIndex < 0) return;
        if (_inputManager.GetSelectedMapPosition(out _indicatorPosition))
        {
            _cellIndicator.transform.position = _grid.CellToWorld(_grid.WorldToCell(_indicatorPosition));
            _cellIndicator.SetActive(true);
        }
        else
            _cellIndicator.SetActive(false);
    }
    public void StopPlacement()
    {
        _selectedObjectIndex = -1;
        _gridVisualization.SetActive(false);
        _cellIndicator.SetActive(false);
        _inputManager.OnClicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
        _inputManager.OnClicked += ShowCellMenu;
    }

    private void ShowCellMenu()
    {
        /*
         Надо добавить возможность:
            * Удалять
            * Перемещать
            * Улучшать
            * ...Чистить?
         */

        /*
         Для удаления надо будет переписать матрицу, чтобы она не только ID держала, но и ссылки на инстансы (структура?)
         */

        /*Баг: дабл-клик по кнопке стройки вызывает ошибку при нажатии по траве до нажатия пкм*/

        if (_inputManager.IsPointerOverUI()) return;

        Vector3Int cellCoords = _grid.WorldToCell(_indicatorPosition);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_cellIndicator.transform.position + new Vector3(3f, 3f));
        Debug.Log(screenPosition.ToString());
        _cellMenu.transform.position = screenPosition;
        _cellMenu.gameObject.SetActive(true);

        _inputManager.OnExit += HideCellMenu;
        _inputManager.OnClicked += HideCellMenu;
        _inputManager.OnClicked -= ShowCellMenu;

        //Debug.Log(cellCoords.ToString() + (objectsMatrix[cellCoords]));

        // Если поле пустое, показать меню постройки
        if (objectsMatrix[cellCoords] == -1) 
        {
            _cellMenu.GetComponentInChildren<TMP_Text>().text = "Empty";
        }
        //Поле не пустое, показать возможность разрушить объект
        else
        {
            _cellMenu.GetComponentInChildren<TMP_Text>().text = _objectDatabase.objectsData[objectsMatrix[cellCoords]].Name;
        }
    }

    private void HideCellMenu()
    {
        if (_inputManager.IsPointerOverUI()) return;

        _cellMenu.gameObject.SetActive(false);
        _inputManager.OnExit -= HideCellMenu;
        _inputManager.OnClicked -= HideCellMenu;
        _inputManager.OnClicked += ShowCellMenu;
    }

    private void PlaceStructure()
    {
        if (_inputManager.IsPointerOverUI()) return;

        Vector3Int cellCoords = _grid.WorldToCell(_indicatorPosition);
        //Debug.Log(cellCoords.ToString() + (objectsMatrix[cellCoords]));
        if (objectsMatrix[cellCoords] == -1)
        {
            GameObject newObject = Instantiate(_objectDatabase.objectsData[_selectedObjectIndex].Prefab);
            
            newObject.transform.position = _grid.CellToWorld(cellCoords);
            newObject.transform.parent = _objectsParent;

            objectsMatrix[cellCoords] = _selectedObjectIndex;
            // НЕ УДАЛЯЙ !!!
            WorldStatistic.ChangePollution(_objectDatabase.objectsData[_selectedObjectIndex].PollutionMultiplier);
            //
            Debug.Log("Объект поставлен");
            StopPlacement();
        }
    }
}