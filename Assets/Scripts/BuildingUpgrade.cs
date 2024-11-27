using TMPro;
using UnityEngine;

public class BuildingUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject[] buildingModels; // Массив моделей зданий
    private int upgradeClicks = 0; // Счетчик кликов
    private const int maxClicks = 5; // Максимальное количество кликов для апгрейда
    private GameObject currentBuilding; // Текущее здание

    void Start()
    {
        // Инициализация текущего здания
        if (buildingModels.Length > 0)
        {
            currentBuilding = Instantiate(buildingModels[0], transform.position, Quaternion.identity);
        }
    }

    public void OnUpgradeButtonClicked()
    {
        upgradeClicks++;
        if (upgradeClicks >= maxClicks)
        {
            UpgradeBuilding();
            upgradeClicks = 0; // Сброс счетчика кликов
        }
    }

    private void UpgradeBuilding()
    {
        int currentIndex = System.Array.IndexOf(buildingModels, currentBuilding);
        if (currentIndex < buildingModels.Length - 1)
        {
            Destroy(currentBuilding); // Уничтожаем текущее здание
            currentBuilding = Instantiate(buildingModels[currentIndex + 1], transform.position, Quaternion.identity); // Создаем новое здание
        }
        else
        {
            Debug.Log("Максимальный уровень здания достигнут!");
        }
    }
}