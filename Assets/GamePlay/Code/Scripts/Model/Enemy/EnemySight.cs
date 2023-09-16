using System;
using UnityEngine;

namespace GamePlay.Code.Scripts
{
    public class EnemySight : MonoBehaviour
    {
        private const string PREFAB_PLAYER_TAG = "Player";
        
        public Enemy enemy;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(PREFAB_PLAYER_TAG))
            {
                enemy.SetTarget(col.GetComponent<Character>());
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag(PREFAB_PLAYER_TAG))
            {
                enemy.SetTarget(null);
            }
        }
    }
}