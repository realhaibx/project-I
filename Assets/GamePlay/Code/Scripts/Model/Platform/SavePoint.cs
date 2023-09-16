using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private const string PREFABS_PLAYER = "Player";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PREFABS_PLAYER))
        {
            collision.GetComponent<Player>().SavePoint(collision.transform.position);
        }
    }
}
