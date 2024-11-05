using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class WorldStatistic : MonoBehaviour
{
    // �������� ��� (�� ������� ���������, ������� ����������) �� ������� (����� ��������)
    [SerializeField] private float startingEcoStatus = 30.0f;
    [SerializeField] private float maxEcoStatus = 100.0f;
    [SerializeField] private static float currentPollutionMultiplier = 1.0f;
    [SerializeField] private float pollutionUnit = 1.0f;
    [SerializeField] private Slider ecoSlider;

    private bool? checkWin = null;
    private float currentEcoStatus = 0.0f;

    private void Start()
    {
        currentEcoStatus = startingEcoStatus;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (checkWin == null)
        {
            currentEcoStatus += currentPollutionMultiplier * pollutionUnit * (Time.deltaTime / 1); // ������ += ���������() * ������������������ * �����

            ecoSlider.value = currentEcoStatus;

            //Debug.Log(currentEcoStatus.ToString() + " / " + currentPollutionMultiplier.ToString());

            if (currentEcoStatus <= 0.0f)
            {
                checkWin = false;
                Debug.Log("��������");
            }
            if (currentEcoStatus >= maxEcoStatus)
            {
                checkWin = true;
                Debug.Log("������");
            }
        }
    }
    public static void ChangePollution(float multiply)
    {
        currentPollutionMultiplier += multiply;
    }
}
