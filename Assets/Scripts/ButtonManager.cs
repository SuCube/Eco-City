using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject descriptionPanel; // Панель с описанием
    private bool isButtonPressed; // Последняя нажатая кнопка

    void Start()
    {
        // Скрываем панель с описанием в начале
        descriptionPanel.SetActive(false);
        isButtonPressed = false;
    }

    public void OnButtonHover(Button button)
    {
        // Проверяем, была ли нажата кнопка
        if (!isButtonPressed)
        {
            // Показываем описание, если кнопка не была нажата
            descriptionPanel.SetActive(true);
        }
    }

    public void OnButtonClick(Button button)
    {
        // Скрываем описание при нажатии на кнопку
        descriptionPanel.SetActive(false);
        isButtonPressed = true; // Запоминаем последнюю нажатую кнопку
    }

    public void OnMouseExit(Button button)
    {
        // Скрываем описание, если мышь покинула кнопку
        if (!isButtonPressed)
        {
            descriptionPanel.SetActive(false);
        }
    }
}