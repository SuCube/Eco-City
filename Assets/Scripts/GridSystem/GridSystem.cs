//GridSystem.cs
//Òóò áàã ãäå-òî â ñòðîèòåëüñòâå è â cellMenu, íàäî ôèêñèòü
//
using TMPro;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private GameObject _cellIndicator;
    [SerializeField] private GameObject _cellMenu;

    [SerializeField] private ObjectDatabaseSO _objectDatabase;
    private int _selectedObjectIndex = -1; //Если -1, то стройка не идёт - объект не выбран

    [SerializeField] private GameObject _gridVisualization;
    [SerializeField] private Transform _objectsParent;

    private Vector3 _indicatorPosition;
    private Vector3 _selectedCellPosition;
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
        objectsMatrix = new GridMatrix(new Vector3Int(10,1,10), new Vector3Int(5,0,5));
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

        Debug.Log("Íà÷àëî ïîñòðîéêè îáúåêòà " + _objectDatabase.objectsData[_selectedObjectIndex].Name);
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
         Íàäî äîáàâèòü âîçìîæíîñòü:
            * Óäàëÿòü
            * Ïåðåìåùàòü
            * Óëó÷øàòü
            * ...×èñòèòü?
         */

        /*Áàã: äàáë-êëèê ïî êíîïêå ñòðîéêè âûçûâàåò îøèáêó ïðè íàæàòèè ïî òðàâå äî íàæàòèÿ ïêì*/

        if (_inputManager.IsPointerOverUI()) return;

        _selectedCellPosition = _grid.WorldToCell(_indicatorPosition);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_cellIndicator.transform.position + new Vector3(3f, 3f));
        Debug.Log(screenPosition.ToString());
        _cellMenu.transform.position = screenPosition;
        _cellMenu.gameObject.SetActive(true);

        _inputManager.OnExit += HideCellMenu;
        _inputManager.OnClicked += HideCellMenu;
        _inputManager.OnClicked -= ShowCellMenu;

        //Debug.Log(cellCoords.ToString() + (objectsMatrix[cellCoords]));

        // Åñëè ïîëå ïóñòîå, ïîêàçàòü ìåíþ ïîñòðîéêè
        TMP_Text cellobjText = _cellMenu.GetComponentInChildren<TMP_Text>();

        if (objectsMatrix[_selectedCellPosition] == null)
        {
            cellobjText.text = "Empty";
        }
        //Ïîëå íå ïóñòîå, ïîêàçàòü âîçìîæíîñòü ðàçðóøèòü îáúåêò
        else
        {
            cellobjText.text = _objectDatabase.objectsData[objectsMatrix[_selectedCellPosition].Id].Name;
            Debug.Log(objectsMatrix[cellCoords].Id);
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
        //if (objectsMatrix[cellCoords] == -1)
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

            objectsMatrix[cellCoords] = newBuilding;

            // Update pollution or other game stats
            WorldStatistic.ChangePollution(_objectDatabase.objectsData[_selectedObjectIndex].PollutionMultiplier); 
        
            StopPlacement();
        }
    }

    private void DestroyStructure()
    {
        //if (objectsMatrix[cellCoords] == -1;
        _selectedCellPosition = _grid.WorldToCell(_indicatorPosition);
        if (_inputManager.GetSelectedMapPosition(out _indicatorPosition) && objectsMatrix[_selectedCellPosition] == null)
        {
            GameObject obj = _objectDatabase.objectsData[objectsMatrix[_selectedCellPosition].Id].Prefab;
            Destroy (obj);
            objectsMatrix[cellCoords] = null;

            // Update pollution or other game stats
            WorldStatistic.ChangePollution(_objectDatabase.objectsData[_selectedObjectIndex].PollutionMultiplier * -1.0);
        }
        HideCellMenu();
    }
}
