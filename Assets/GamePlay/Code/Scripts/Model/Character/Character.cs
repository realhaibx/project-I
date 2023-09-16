using System;
using System.Runtime.Serialization;
using GamePlay.Code.Scripts;
using UnityEngine;
using static GamePlay.Code.Scripts.Extension.EnumAttribute;

public class Character : MonoBehaviour
{
    #region PLAYER STATE

    public enum State
    {
        [EnumMember(Value = "Idle")] Idle,
        [EnumMember(Value = "AttackNormal")] Attack,
        [EnumMember(Value = "Jump")] Jump,
        [EnumMember(Value = "Throw")] Throw,
        [EnumMember(Value = "Climb")] Climb,
        [EnumMember(Value = "Dead")] Dead,
        [EnumMember(Value = "Glide")] Glide,
        [EnumMember(Value = "Run")] Run,
        [EnumMember(Value = "Slide")] Slide,
        [EnumMember(Value = "Fall")] Fall
    };

    #endregion
    
    private float hp;
    private Animator anim;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private CombatText combatText;
    public State currentAnim;
    public bool IsDead => hp <= 0;
    
    private void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(hp, transform);
    }

    protected virtual void OnDespawn()
    {
            
    }

    protected virtual void OnRespawn()
    {

    }

    public virtual void OnHit(float damage)
    {
        hp -= damage;
        healthBar.SetNewHp(hp > 0 ? hp : 0);
        Instantiate(combatText, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        if (IsDead)
            Dead();
    }

    protected virtual void Dead()
    {
        ChangeAnim(State.Dead);
        Invoke(nameof(OnDespawn), 0f);
    }

    private void ChangeAnim(State animName)
    {
        if (currentAnim == animName) return;
        if (anim != null)
        {
            anim.ResetTrigger(GetEnumMemberAttrValue(typeof(State), animName));
            currentAnim = animName;
            anim.SetTrigger(GetEnumMemberAttrValue(typeof(State), currentAnim));   
        }
    }
}
