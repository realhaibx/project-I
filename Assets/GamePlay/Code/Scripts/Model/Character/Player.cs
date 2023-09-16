using System;
using System.Runtime.Serialization;
using GamePlay.Code.Scripts;
using UnityEngine;
using static GamePlay.Code.Scripts.Extension.EnumAttribute;

public class Player : Character
{
    #region STATIC VARIABLE

    private const long PLAYER_DEFAULT_COIN = 0L;
    private const float PLAYER_DEFAULT_SPEED = 200.0f;
    private const float PLAYER_DEFAULT_ATTACK_SPEED = 1.0f;
    private const float PLAYER_DEFAULT_JUMP_FORCE = 1000.0f;
    private const float PLAYER_DEFAULT_REVIVE_TIME = 1.0f;
    private const float MINIMUM_HORIZONTAL_DIFF = 0.2f;
    private const float MINIMUM_VERTICAL_DIFF = 0.1f;
    private const bool PLAYER_DEFAULT_ALIVE_STATE = true;
    private const bool PLAYER_DEFAULT_IDLE_STATE = true;
    private const bool PLAYER_DEFAULT_JUMP_STATE = false;
    private const bool PLAYER_DEFAULT_ATTACK_STATE = false;

    private const string PREFABS_COIN = "Coin";
    private const string PREFABS_DEATH_ZONE = "DeathZone";

    #endregion

    #region SERIALIZE FIELD AND PRIVATE VARIABLE

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private long coin = PLAYER_DEFAULT_COIN;
    [SerializeField] private float cd = 1 / PLAYER_DEFAULT_ATTACK_SPEED;
    [SerializeField] private float speed = PLAYER_DEFAULT_SPEED;
    [SerializeField] private float jumpForce = PLAYER_DEFAULT_JUMP_FORCE;
    [SerializeField] private float reviveTime = PLAYER_DEFAULT_REVIVE_TIME;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Kunai kunai;
    [SerializeField] private GameObject attackRange;

    private bool isDead = !PLAYER_DEFAULT_ALIVE_STATE;
    private bool isGrounded = PLAYER_DEFAULT_IDLE_STATE;
    private bool isJumping = PLAYER_DEFAULT_JUMP_STATE;
    private bool isAttacking = PLAYER_DEFAULT_ATTACK_STATE;
    private float attackTime = 0.517f;
    private float throwTime = 0.517f;
    private AnimationClip animClip;

    private float horizontal;

    private new State currentAnim = State.Idle;

    [SerializeField] private Vector3 savePoint;

    #endregion

    #region UNITY FUNCTION

    public void Update()
    {
        isGrounded = CheckGrounded();
        UIManager.instance.setCoin(coin);
        if (!isDead)
            HandleState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PREFABS_COIN))
        {
            coin++;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag(PREFABS_DEATH_ZONE))
        {
            Dead();
        }
    }

    #endregion

    #region USER FUNCTION

    public override void OnInit()
    {
        base.OnInit();
        isDead = !PLAYER_DEFAULT_ALIVE_STATE;
        isAttacking = PLAYER_DEFAULT_ATTACK_STATE;
        SavePoint(transform.position);
        ChangeAnim(State.Idle);
        UIManager.instance.setCoin(0);
    }

    protected override void OnDespawn()
    {
        base.OnDespawn();
        isDead = false;
        OnInit();
    }

    protected override void OnRespawn()
    {
        base.OnRespawn();
        this.transform.position = savePoint;
        OnInit();
    }

    private void HandleState()
    {

        if (isDead)
            return;

        if (isAttacking)
            return;

        if (!isGrounded && Math.Abs(rb.velocity.y) <= MINIMUM_VERTICAL_DIFF)
        {
            Fall();
        }

        if (isJumping)
            return;

        HandleInput();

        if (IsMoving())
        {
            Run();
        }
        else if (isGrounded)
        {
            Idle();
        }

    }

    private void HandleInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Throw();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Idle()
    {
        ChangeAnim(State.Idle);
        rb.velocity = Vector2.zero;
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            rb.velocity = Vector2.zero;
            isAttacking = true;
            ChangeAnim(State.Attack);
            Invoke(nameof(ResetAttack), attackTime);

            ActiveAttack();
            Invoke(nameof(DeactiveAttack), cd); 
        }
    }

    protected override void Dead()
    {
        isDead = true;
        ChangeAnim(State.Dead);
        Invoke(nameof(OnRespawn), reviveTime);
    }

    private void Fall()
    {
        isJumping = false;
        ChangeAnim(State.Fall);
    }

    public void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            ChangeAnim(State.Jump);
            rb.AddForce(jumpForce * Vector2.up);
        }
    }

    private void Run()
    {
        ChangeAnim(State.Run);
        rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
        transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
    }

    public void Throw()
    {
        if (!isAttacking)
        {
            rb.velocity = Vector2.zero;
            isAttacking = true;
            ChangeAnim(State.Throw);
            Instantiate(kunai, throwPoint.position, throwPoint.rotation);
            Invoke(nameof(ResetThrow), throwTime);
        }
    }

    private bool IsMoving()
    {
        return Math.Abs(horizontal) > MINIMUM_HORIZONTAL_DIFF;
    }

    private void ResetAttack()
    {
        ChangeAnim(State.Idle);
        Update();
    }

    private void ResetThrow()
    {
        isAttacking = false;
        ChangeAnim(State.Idle);
        Update();
    }

    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    private void ChangeAnim(State animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(GetEnumMemberAttrValue(typeof(State), animName));
            currentAnim = animName;
            anim.SetTrigger(GetEnumMemberAttrValue(typeof(State), currentAnim));
        }
    }

    public void SavePoint(Vector3 pos)
    {
        savePoint = pos;
    }

    private void ActiveAttack()
    {
        attackRange.SetActive(true);
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
        Run();
    }

    private void DeactiveAttack()
    {
        isAttacking = false;
        attackRange.SetActive(false);
    }
    #endregion
}