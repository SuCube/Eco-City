using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI descriptionText; // ������ �� ������ TextMeshPro ��� ��������
    private bool isButtonClicked = false; // ���� ��� ������������ ������� ������

    void Start()
    {
        // �������� ����� �������� ��� ������
        descriptionText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���������� �������� ������ ���� ������ �� ���� ������
        if (!isButtonClicked)
        {
            descriptionText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // �������� �������� ��� ����� �������
        descriptionText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ������������� ���� ������� ������
        isButtonClicked = true;
        descriptionText.gameObject.SetActive(false); // �������� �������� ��� �������
    }

    public void ResetButtonState()
    {
        // ����� ��� ������ ��������� ������, ����� �������� ����� ������������
        isButtonClicked = false;
    }
}