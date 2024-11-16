using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomPhraseDisplay : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI displayText; // Ссылка на компонент TextMeshPro
    public TextAsset phrasesFile; // Переменная для хранения TextAsset
    public TextAsset GoodPhrasesFile; // Переменная для хранения TextAsset
    public TextAsset BadPhrasesFile; // Переменная для хранения TextAsset
    private List<string> phrases = new List<string>();
    private List<string> GoodPhrases = new List<string>();
    private List<string> BadPhrases = new List<string>();
    private float timer;
    private float interval = 5f; // Интервал обновления фраз

    void Start()
    {
        timer = interval; // Устанавливаем таймер
        LoadPhrases(); // Загружаем фразы в начале
        DisplayRandomPhrase(); // Показываем первую фразу
    }

    void Update()
    {
        timer -= Time.deltaTime; // Уменьшаем таймер
        if (timer <= 0)
        {
            LoadPhrases(); // Загружаем фразы в зависимости от значения слайдера
            DisplayRandomPhrase(); // Показываем случайную фразу
            timer = interval; // Сбрасываем таймер
        }
    }

    void LoadPhrases()
    {
        phrases.Clear(); // Очищаем список перед загрузкой новых фраз

        if (slider.value < 30)
        {
            string[] lines = BadPhrasesFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            BadPhrases.AddRange(lines); // Добавляем строки в список
            phrases.AddRange(BadPhrases); // Переносим в общий список
            Debug.Log("Bad phrases loaded: " + BadPhrases.Count); // Для отладки
        }
        else if (slider.value >= 30 && slider.value < 70)
        {
            string[] lines = phrasesFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            phrases.AddRange(lines); // Добавляем строки в список
            Debug.Log("Neutral phrases loaded: " + phrases.Count); // Для отладки
        }
        else if (slider.value >= 70)
        {
            string[] lines = GoodPhrasesFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            GoodPhrases.AddRange(lines); // Добавляем строки в список
            phrases.AddRange(GoodPhrases); // Переносим в общий список
            Debug.Log("Good phrases loaded: " + GoodPhrases.Count); // Для отладки
        }
    }

    void DisplayRandomPhrase()
    {
        if (phrases.Count > 0)
        {
            int randomIndex = Random.Range(0, phrases.Count); // Генерируем случайный индекс
            displayText.text = phrases[randomIndex]; // Устанавливаем текст
        }
    }
}