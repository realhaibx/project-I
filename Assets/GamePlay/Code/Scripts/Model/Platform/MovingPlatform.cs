using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private const string PREFABS_PLAYER = "Player";
    
    private const float MOVING_PLATFORM_DEFAULT_SPEED = 100.0f;
    private const float MOVING_PLATFORM_DEFAULT_REACH_DISTANCE = 0.1f;
    
    [SerializeField] private Transform startPoint, endPoint, targetPoint;
    [SerializeField] private float speed = MOVING_PLATFORM_DEFAULT_SPEED;
    [SerializeField] private float minDistance = MOVING_PLATFORM_DEFAULT_REACH_DISTANCE;

    private Vector3 target;
    private Transform c;
    private void Start()
    {
        transform.position = startPoint.position;
        SetTargetPoint(startPoint);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPoint.position) < minDistance)
        {
            SwapTargetPoint();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PREFABS_PLAYER))
        {
            c = collision.transform.parent;
            collision.transform.SetParent(transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PREFABS_PLAYER))
        {
            collision.transform.SetParent(c.transform);
        }
    }
    
    private void SetTargetPoint(Transform point)
    {
        targetPoint = point;
        target = targetPoint.position;
    }

    private void SwapTargetPoint()
    {
        SetTargetPoint(targetPoint == startPoint ? endPoint : startPoint);
    }
}
