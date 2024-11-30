using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayLength = 120f; // ����� ��� � ��������
    private float rotationSpeed;

    void Start()
    {
        // ������������ �������� �������� � ����������� �� ����� ���
        rotationSpeed = 360f / dayLength;
    }

    void Update()
    {
        // ������� ������
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}