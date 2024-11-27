// GridSystem.cs
// УБРАТЬ ИВЕНТЫ (CellMenu спамит несколько раз, ивенты накапливаются)

using TMPro;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private GameObject _cellIndicator;
    [SerializeField] private GameObject _cellMenu;

    [SerializeField] private ObjectDatabaseSO _objectDatabase;
    private int _selectedObjectIndex = -1; // Если -1, то стройка не идёт - объект не выбран

    [SerializeField] private GameObject _gridVisualization;
    [SerializeField] private Transform _objectsParent;

    private Vector3 _indicatorPosition;
    private Vector3Int _selectedCellPosition;
    private bool _isCellMenuShown;
    private bool _canSelectorMove;

    GridMatrix objectsMatrix;

    private class GridMatrix
    {
        private readonly Building[,,] matrix; 
        private readonly Vector3Int startingPoint;

        public GridMatrix(Vector3Int size, Vector3Int startingPoint)
        {
            matrix = new Building[size.x, 1, size.z];
            this.startingPoint = startingPoint;

            InitializeMatrix(size);
        }

        public Building this[Vector3Int coords]
        {
            get => matrix[NormalizeIndex(coords).x, 0, NormalizeIndex(coords).z];
            set => matrix[NormalizeIndex(coords).x, 0, NormalizeIndex(coords).z] = value;
        }

        private void InitializeMatrix(Vector3Int size)
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.z; j++)
                {
                    matrix[i, 0, j] = null;
                }
            }
        }

        private Vector3Int NormalizeIndex(Vector3Int coords)
        {
            return coords + startingPoint;
        }
    }

    void Start()
    {
        objectsMatrix = new GridMatrix(new Vector3Int(10, 1, 10), new Vector3Int(5, 0, 5));
        _isCellMenuShown = false;
        _canSelectorMove = true;
        _inputManager.OnClicked += ShowCellMenu;
    }

    public void StartPlacement(int ID)
    {
        if (ID == _selectedObjectIndex) return;
        if (_selectedObjectIndex != -1) StopPlacement();
        
        _selectedObjectIndex = _objectDatabase.objectsData.FindIndex(data => data.ID == ID);
        if (_selectedObjectIndex < 0)
        {
            Debug.LogError($"Object with ID {ID} not found");
            return;
        }
        _gridVisualization.SetActive(true);

        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
        _inputManager.OnClicked -= ShowCellMenu;

        Debug.Log("Начало постройки объекта " + _objectDatabase.objectsData[_selectedObjectIndex].Name);
    }

    void Update()
    {
        //if (_selectedObjectIndex < 0) return;
        if (_canSelectorMove)
        {
            if (_inputManager.GetSelectedMapPosition(out _indicatorPosition))
            {
                _cellIndicator.transform.position = _grid.CellToWorld(_grid.WorldToCell(_indicatorPosition));
                _cellIndicator.SetActive(true);
            }
            else
                _cellIndicator.SetActive(false);
        }
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
        /* Надо добавить возможность:    
         * Удалять DONE  
         * Перемещать   
         * Улучшать 
         * Чистить от грязи
         */

        if (_inputManager.IsPointerOverUI()) return;

        _selectedCellPosition = _grid.WorldToCell(_indicatorPosition);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_cellIndicator.transform.position + new Vector3(3f, 3f));
        Debug.Log(screenPosition.ToString());
        _cellMenu.transform.position = screenPosition;
        _cellMenu.gameObject.SetActive(true);

        _inputManager.OnExit += HideCellMenu;
        _inputManager.OnClicked += HideCellMenu;
        _inputManager.OnClicked -= ShowCellMenu;

        _isCellMenuShown = true;
        _canSelectorMove = false;

        //Debug.Log(cellCoords.ToString() + (objectsMatrix[cellCoords]));

        // Если поле пустое, показать меню постройки.
        TMP_Text cellobjText = _cellMenu.GetComponentInChildren<TMP_Text>();

        if (objectsMatrix[_selectedCellPosition] == null)
        {
            cellobjText.text = "Empty";
        }
        // Поле не пустое, показать возможность разрушить объект
        else
        {
            cellobjText.text = _objectDatabase.objectsData[objectsMatrix[_selectedCellPosition].Id].Name;
            _selectedObjectIndex = objectsMatrix[_selectedCellPosition].Id;
            //Debug.Log(objectsMatrix[_selectedCellPosition].Id);
        }
    }

    private void HideCellMenu()
    {
        if (_inputManager.IsPointerOverUI()) return;

        _cellMenu.gameObject.SetActive(false);
        _inputManager.OnExit -= HideCellMenu;
        _inputManager.OnClicked -= HideCellMenu;
        _inputManager.OnClicked += ShowCellMenu;

        _isCellMenuShown = false;
        _canSelectorMove = true;
    }

    private void PlaceStructure()
    {
        if (_inputManager.IsPointerOverUI()) return;

        Vector3Int cellCoords = _grid.WorldToCell(_indicatorPosition);
        if (_inputManager.GetSelectedMapPosition(out _indicatorPosition) && objectsMatrix[cellCoords] == null)
        {
            GameObject newObject = Instantiate(_objectDatabase.objectsData[_selectedObjectIndex].Prefab);
        
            newObject.transform.position = _grid.CellToWorld(cellCoords);
            newObject.transform.parent = _objectsParent;

            // Get the Building component from the instantiated prefab
            Building newBuilding = newObject.GetComponent<Building>();
            if (newBuilding != null)
            {
                newBuilding.Id = _selectedObjectIndex; // Set the ID from the database
                newBuilding.BuildingStatus = Building.Status.IsUnderConstruction; // Set initial status
                objectsMatrix[cellCoords] = newBuilding; // Store the Building directly
            }
            else
            {
                Debug.LogError("The prefab does not have a Building component attached.");
            }

            // НЕ УДАЛЯЙ !!!
            if (WorldStatistic.HaveMoney(_objectDatabase.objectsData[_selectedObjectIndex].Price) == false)
            {
                StopPlacement();
                Destroy(newObject);
            }

            WorldStatistic.ChangePollution(_objectDatabase.objectsData[_selectedObjectIndex].PollutionMultiplier);
            WorldStatistic.ChangeCurrentMoney(_objectDatabase.objectsData[_selectedObjectIndex].Price);
            if (_selectedObjectIndex == 0)
            {
                WorldStatistic.ChangeIncome(-10);
            }
            else if (_selectedObjectIndex == 1)
            {
                WorldStatistic.ChangeIncome(100);
            }
            else if (_selectedObjectIndex == 2)
            {
                WorldStatistic.ChangeIncome(-200);
            }
            //

            StopPlacement();
        }
    }

    public void DestroyStructure()
    {
        // Отрицательные координаты работают странно. Удаление цепляет объект не везде

        _selectedCellPosition = _grid.WorldToCell(_indicatorPosition);
        Debug.Log("Удаляем объект " + _selectedCellPosition + objectsMatrix[_selectedCellPosition].Id); // Null Reference Exception caught
        if (objectsMatrix[_selectedCellPosition] != null)
        {
            GameObject obj = objectsMatrix[_selectedCellPosition].gameObject; // NO PREFAB
            Destroy(obj);
            objectsMatrix[_selectedCellPosition] = null;
            Debug.Log(obj.ToString());
            Debug.Log(_selectedObjectIndex);
            // Update pollution or other game stats
            WorldStatistic.ChangePollution(_objectDatabase.objectsData[_selectedObjectIndex].PollutionMultiplier * -1.0f); // same
        }
        HideCellMenu();
    }
}