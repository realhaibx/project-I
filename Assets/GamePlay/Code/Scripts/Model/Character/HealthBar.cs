using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    [SerializeField] private Vector3 offset;

    private float hp;
    private float maxHP;

    private Transform target;
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHP, Time.deltaTime * 2f);
        if (target != null)
        {
            transform.position = target.position + offset;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnInit(float maxHp, Transform target)
    {
        this.target = target;
        this.maxHP = maxHp;
        hp = maxHp;
        imageFill.fillAmount = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }
}
