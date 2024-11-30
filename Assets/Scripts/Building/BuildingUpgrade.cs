using TMPro;
using UnityEngine;

public class BuildingUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject[] buildingModels; // ������ ������� ������
    private int upgradeClicks = 0; // ������� ������
    private const int maxClicks = 5; // ������������ ���������� ������ ��� ��������
    private GameObject currentBuilding; // ������� ������

    void Start()
    {
        // ������������� �������� ������
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
            upgradeClicks = 0; // ����� �������� ������
        }
    }

    private void UpgradeBuilding()
    {
        int currentIndex = System.Array.IndexOf(buildingModels, currentBuilding);
        if (currentIndex < buildingModels.Length - 1)
        {
            Destroy(currentBuilding); // ���������� ������� ������
            currentBuilding = Instantiate(buildingModels[currentIndex + 1], transform.position, Quaternion.identity); // ������� ����� ������
        }
        else
        {
            Debug.Log("������������ ������� ������ ���������!");
        }
    }
}