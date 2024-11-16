using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomPhraseDisplay : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI displayText; // ������ �� ��������� TextMeshPro
    public TextAsset phrasesFile; // ���������� ��� �������� TextAsset
    public TextAsset GoodPhrasesFile; // ���������� ��� �������� TextAsset
    public TextAsset BadPhrasesFile; // ���������� ��� �������� TextAsset
    private List<string> phrases = new List<string>();
    private List<string> GoodPhrases = new List<string>();
    private List<string> BadPhrases = new List<string>();
    private float timer;
    private float interval = 5f; // �������� ���������� ����

    void Start()
    {
        timer = interval; // ������������� ������
        LoadPhrases(); // ��������� ����� � ������
        DisplayRandomPhrase(); // ���������� ������ �����
    }

    void Update()
    {
        timer -= Time.deltaTime; // ��������� ������
        if (timer <= 0)
        {
            LoadPhrases(); // ��������� ����� � ����������� �� �������� ��������
            DisplayRandomPhrase(); // ���������� ��������� �����
            timer = interval; // ���������� ������
        }
    }

    void LoadPhrases()
    {
        phrases.Clear(); // ������� ������ ����� ��������� ����� ����

        if (slider.value < 30)
        {
            string[] lines = BadPhrasesFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            BadPhrases.AddRange(lines); // ��������� ������ � ������
            phrases.AddRange(BadPhrases); // ��������� � ����� ������
            Debug.Log("Bad phrases loaded: " + BadPhrases.Count); // ��� �������
        }
        else if (slider.value >= 30 && slider.value < 70)
        {
            string[] lines = phrasesFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            phrases.AddRange(lines); // ��������� ������ � ������
            Debug.Log("Neutral phrases loaded: " + phrases.Count); // ��� �������
        }
        else if (slider.value >= 70)
        {
            string[] lines = GoodPhrasesFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            GoodPhrases.AddRange(lines); // ��������� ������ � ������
            phrases.AddRange(GoodPhrases); // ��������� � ����� ������
            Debug.Log("Good phrases loaded: " + GoodPhrases.Count); // ��� �������
        }
    }

    void DisplayRandomPhrase()
    {
        if (phrases.Count > 0)
        {
            int randomIndex = Random.Range(0, phrases.Count); // ���������� ��������� ������
            displayText.text = phrases[randomIndex]; // ������������� �����
        }
    }
}