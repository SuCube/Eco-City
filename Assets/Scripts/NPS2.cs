using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPS2 : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints; // ������ �����
    private Transform targetWaypoint; // ������� ������� �����
    private Animator animator;

    void Start()
    {
        //speed = 2;
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true); // ������ ��������
        transform.rotation = Quaternion.Euler(0, 0, 0);
        FindNextWaypoint(); // ����� ������ �����
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
        // ����������� � ������� �����
        Vector3 targetPosition = new Vector3(targetWaypoint.position.x, transform.position.y, targetWaypoint.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ������� � ����������� �����
        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
        }

        // �������� ���������� �� �����
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            FindNextWaypoint(); // ����� ��������� ����� ����� ���������� �������
        }
    }

    void FindNextWaypoint()
    {
        // ������� ��� ��������� �����
        Transform closestWaypoint = null;
        Transform secondClosestWaypoint = null;
        float closestDistance = Mathf.Infinity;
        float secondClosestDistance = Mathf.Infinity;

        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.position);

            if (distance < closestDistance)
            {
                // ��������� ������ ��������� ����� ����� ���������� ������
                secondClosestWaypoint = closestWaypoint;
                secondClosestDistance = closestDistance;

                // ��������� ��������� �����
                closestWaypoint = waypoint;
                closestDistance = distance;
            }
            else if (distance < secondClosestDistance)
            {
                // ��������� ������ ������ ��������� �����
                secondClosestWaypoint = waypoint;
                secondClosestDistance = distance;
            }
        }

        // �������� ���� �� ��������� ����� (��������, ��������� �������)
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