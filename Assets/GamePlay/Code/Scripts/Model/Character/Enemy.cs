using System;
using GamePlay.Code.Scripts.StateMachine;
using static GamePlay.Code.Scripts.Extension.EnumAttribute;
using UnityEngine;

namespace GamePlay.Code.Scripts
{
    public class Enemy : Character
    {
        private const string PREFAB_ENEMY_WALL_TAG = "EnemyWall";

        private const float ENEMY_DEFAULT_ATTACK_RANGE = 100.0f;
        private const float ENEMY_DEFAULT_MOVE_SPEED = 100.0f;
        private const State ENEMY_DEFAULT_STATE = State.Idle;


        [SerializeField] private float attackRange = ENEMY_DEFAULT_ATTACK_RANGE;
        [SerializeField] private float moveSpeed = ENEMY_DEFAULT_MOVE_SPEED;
        [SerializeField] private State state = ENEMY_DEFAULT_STATE;
        [SerializeField] private Transform wallLeft, wallRight;
        [SerializeField] private Animator anim;
        [SerializeField] private Rigidbody2D rb;

        private Character target;
        public Character Target => target;
        private IState currentState;
        private bool direction = true;
        void Start()
        {
            OnInit();
        }

        private void Update()
        {
            currentState?.OnExcute(this);
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(PREFAB_ENEMY_WALL_TAG))
            {
                ChangeDirection(!direction);
            }
        }

        public void ChangeDirection(bool isFlip)
        {
            this.direction = isFlip;

            transform.rotation = isFlip ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
        }
 
        public void ChangeState(IState newState)
        {
            currentState?.OnExit(this);

            currentState = newState;

            currentState?.OnEnter(this);
        }
        public override void OnInit()
        {
            base.OnInit();
            ChangeState(new IdleState());
        }

        protected override void OnDespawn()
        {
            base.OnDespawn();
            Destroy(gameObject);
        }

        protected override void Dead()
        {
            ChangeState(null);
            OnDespawn();
            base.Dead();
        }
        
        public void SetTarget(Character character)
        {
            this.target = character;

            if (IsTargetInRange())
            {
                ChangeState(new AttackState());
            }
            else if (Target != null)
            {
                ChangeState(new PatrolState());
            }
            else
            {
                ChangeState(new IdleState());
            }
        }
        
        public void Moving()
        {
            ChangeAnimation(State.Run);
            rb.velocity = transform.right * moveSpeed;
        }

        public void StopMoving()
        {
            ChangeAnimation(State.Idle);
            rb.velocity = Vector2.zero;
        }

        public void Attack()
        {
            ChangeAnimation(State.Attack);
            
        }

        public bool IsTargetInRange()
        {
            if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
                return true;
            return false;
        }

        protected void ChangeAnimation(State animName)
        {
            if (IsDead)
            {
                Dead();
            }
            if (currentAnim == animName) return;
            anim.ResetTrigger(GetEnumMemberAttrValue(typeof(State), animName));
            currentAnim = animName;
            anim.SetTrigger(GetEnumMemberAttrValue(typeof(State), currentAnim));
        }
    }
}