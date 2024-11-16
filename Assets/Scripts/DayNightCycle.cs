using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayLength = 120f; // Длина дня в секундах
    private float rotationSpeed;

    void Start()
    {
        // Рассчитываем скорость вращения в зависимости от длины дня
        rotationSpeed = 360f / dayLength;
    }

    void Update()
    {
        // Вращаем солнце
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}