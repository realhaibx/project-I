using System;
using UnityEngine;

namespace GamePlay.Code.Scripts
{
    public class AttackRange : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Character ch = col.GetComponentInChildren<Character>();
            ch?.OnHit(30.0f);
        }
    }
}