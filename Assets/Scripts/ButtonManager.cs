using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI descriptionText; // Ссылка на объект TextMeshPro для описания
    private bool isButtonClicked = false; // Флаг для отслеживания нажатия кнопки

    void Start()
    {
        // Скрываем текст описания при старте
        descriptionText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Показываем описание только если кнопка не была нажата
        if (!isButtonClicked)
        {
            descriptionText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Скрываем описание при уходе курсора
        descriptionText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Устанавливаем флаг нажатия кнопки
        isButtonClicked = true;
        descriptionText.gameObject.SetActive(false); // Скрываем описание при нажатии
    }

    public void ResetButtonState()
    {
        // Метод для сброса состояния кнопки, чтобы описание снова отображалось
        isButtonClicked = false;
    }
}