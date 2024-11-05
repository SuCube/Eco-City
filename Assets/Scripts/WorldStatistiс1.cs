using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;

public class WorldStatistic1 : MonoBehaviour
{
    [Header("Pollution")]
    [SerializeField] private float startingEcoStatus = 30.0f;
    [SerializeField] private float maxEcoStatus = 100.0f;
    [SerializeField] private static float currentPollutionMultiplier = 1.0f;
    [SerializeField] private float pollutionUnit = 1.0f;
    [SerializeField] private Slider ecoSlider;
    [Header("Finance")]
    [SerializeField] private int startingMoney = 20000;
    [SerializeField] private int startingIncome = 100;
    [SerializeField] private float timeToIncome = 5.0f;
    [SerializeField] private TextMeshProUGUI moneyText;
    private static int currentIncome = 0;
    private static int currentMoney = 0;

    private bool? checkWin = null;
    private float currentEcoStatus = 0.0f;

    private float timerForIncome = 0.0f;

    private void Start()
    {
        currentEcoStatus = startingEcoStatus;
        currentMoney = startingMoney;
    }

    void FixedUpdate()
    {
        WorldStatus();
        timerForIncome += Time.deltaTime;
        
        // �����������
        if (checkWin == null)
        {
            currentEcoStatus += currentPollutionMultiplier * pollutionUnit * (Time.deltaTime / 60); // ������ += ���������() * ������������������ * �����

            ecoSlider.value = currentEcoStatus;

            //Debug.Log(currentEcoStatus.ToString() + " / " + currentPollutionMultiplier.ToString());

            if (currentEcoStatus <= 0.0f)
            {
                checkWin = false;
                //Debug.Log("��������");
            }
            if (currentEcoStatus >= maxEcoStatus)
            {
                checkWin = true;
                //Debug.Log("������");
            }
        }

        // ���������
        //Debug.Log(timerForIncome.ToString() + "/" + (currentIncome + startingIncome).ToString());
        if (timerForIncome >= timeToIncome)
        {
            if (currentMoney - currentMoney + startingIncome < 0) // �����
                currentMoney = 0; //,
            else // �����������
                currentMoney += currentIncome + startingIncome;

            timerForIncome = 0.0f;
        }
    }
    private void Update()
    {
        // ���������
        moneyText.text = currentMoney.ToString() + "$";

        
    }
    public static void ChangePollution(float multiply)
    {
        currentPollutionMultiplier += multiply;
    }

    public static void ChangeIncome(int income)
    {
        currentIncome += income;
    }
    public static void ChangeCurrentMoney(int value)
    {
        currentMoney -= value;
    }
    public static bool HaveMoney(int price)
    {
        bool result = true;

        if (currentMoney - price < 0)
            result = false;

        return result;
    }
    private void WorldStatus()
    {
        Debug.Log($"[���������: " + MathF.Round(currentEcoStatus, 2) + "%] [����. ������.: " + currentPollutionMultiplier + "] [�����. ��: " + (currentIncome + startingIncome).ToString() + "] [����� �� ��: " + (5 - MathF.Round(timerForIncome)).ToString() + "]");
    }
}
