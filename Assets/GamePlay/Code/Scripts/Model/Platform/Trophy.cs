using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    private void Start()
    {
        winPanel.SetActive(false);
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            if (col.CompareTag("Player"))
            {
                winPanel.SetActive(true);
            }
        }
    }
}
