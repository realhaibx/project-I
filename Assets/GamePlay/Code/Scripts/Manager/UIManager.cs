using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // public static UIManager Instance
    // {
    //     get
    //     {
    //         if (instance == null)
    //         {
    //             instance = FindObjectOfType<UIManager>();
    //         }
    //
    //         return instance;
    //     }
    // }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private TMP_Text ctx;

    public void setCoin(long coin)
    {
        ctx.text = coin.ToString();
    }
}
