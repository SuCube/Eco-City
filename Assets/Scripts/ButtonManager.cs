using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject descriptionPanel; // ������ � ���������
    private bool isButtonPressed; // ��������� ������� ������

    void Start()
    {
        // �������� ������ � ��������� � ������
        descriptionPanel.SetActive(false);
        isButtonPressed = false;
    }

    public void OnButtonHover(Button button)
    {
        // ���������, ���� �� ������ ������
        if (!isButtonPressed)
        {
            // ���������� ��������, ���� ������ �� ���� ������
            descriptionPanel.SetActive(true);
        }
    }

    public void OnButtonClick(Button button)
    {
        // �������� �������� ��� ������� �� ������
        descriptionPanel.SetActive(false);
        isButtonPressed = true; // ���������� ��������� ������� ������
    }

    public void OnMouseExit(Button button)
    {
        // �������� ��������, ���� ���� �������� ������
        if (!isButtonPressed)
        {
            descriptionPanel.SetActive(false);
        }
    }
}