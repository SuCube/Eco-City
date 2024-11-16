using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRIVE : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints; // Массив точек
    private Transform targetWaypoint; // Текущая целевая точка
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true); // Запуск анимации
        transform.rotation = Quaternion.Euler(0, 0, 0);
        FindWaypoints(); // Найти все вейпоинты
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
        Vector3 targetPosition = new Vector3(targetWaypoint.position.x, transform.position.y, targetWaypoint.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            FindNextWaypoint(); // Найти следующую точку после достижения текущей
        }
    }

    void FindWaypoints()
    {
        // Получаем все объекты дороги в сцене
        GameObject[] roads = GameObject.FindGameObjectsWithTag("StreetRoadPrefab(Clone)");
        waypoints = new Transform[roads.Length];

        for (int i = 0; i < roads.Length; i++)
        {
            waypoints[i] = roads[i].transform; // Добавляем координаты дороги в массив вейпоинтов
        }
    }

    void FindNextWaypoint()
    {
        Transform closestWaypoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.position);
            if (distance < closestDistance)
            {
                closestWaypoint = waypoint;
                closestDistance = distance;
            }
        }

        targetWaypoint = closestWaypoint; // Устанавливаем ближайшую точку как целевую
    }
}