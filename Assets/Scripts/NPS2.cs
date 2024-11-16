using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPS2 : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints; // Массив точек
    private Transform targetWaypoint; // Текущая целевая точка
    private Animator animator;

    void Start()
    {
        //speed = 2;
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true); // Запуск анимации
        transform.rotation = Quaternion.Euler(0, 0, 0);
        FindNextWaypoint(); // Найти первую точку
    }

    void Update()
    {
        if (targetWaypoint != null)
        {
            MoveTowardsWaypoint();
        }
    }

    void MoveTowardsWaypoint()
    {
        // Перемещение к текущей точке
        Vector3 targetPosition = new Vector3(targetWaypoint.position.x, transform.position.y, targetWaypoint.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Поворот в направлении точки
        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
        }

        // Проверка расстояния до точки
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            FindNextWaypoint(); // Найти следующую точку после достижения текущей
        }
    }

    void FindNextWaypoint()
    {
        // Находим две ближайшие точки
        Transform closestWaypoint = null;
        Transform secondClosestWaypoint = null;
        float closestDistance = Mathf.Infinity;
        float secondClosestDistance = Mathf.Infinity;

        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.position);

            if (distance < closestDistance)
            {
                // Обновляем вторую ближайшую точку перед изменением первой
                secondClosestWaypoint = closestWaypoint;
                secondClosestDistance = closestDistance;

                // Обновляем ближайшую точку
                closestWaypoint = waypoint;
                closestDistance = distance;
            }
            else if (distance < secondClosestDistance)
            {
                // Обновляем только вторую ближайшую точку
                secondClosestWaypoint = waypoint;
                secondClosestDistance = distance;
            }
        }

        // Выбираем одну из ближайших точек (например, случайным образом)
        if (secondClosestWaypoint != null)
        {
            targetWaypoint = (Random.value > 0.5f) ? closestWaypoint : secondClosestWaypoint;
        }
        else
        {
            targetWaypoint = closestWaypoint;
        }
    }
}