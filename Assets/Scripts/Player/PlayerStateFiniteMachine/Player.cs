using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }

    #endregion

    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform wallCheck;
    
    [SerializeField]
    private Transform ledgeCheck;
    
    [SerializeField]
    private Transform ceillingCheck;
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    public SoundManager soundManager;

    [SerializeField] private GameObject effect;

    private Vector2 workSpaceVector;
    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        MovementCollider = GetComponent<BoxCollider2D>();

        FacingDirection = 1;
        
       // SecondaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.secondary]);

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            soundManager.PlayCoinSound();
            gameManager.CollectCoint();
            //soundManager.PlayCoinSound();
            Instantiate(effect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

        
    }

    #endregion

    #region Set Functions

    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workSpaceVector.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workSpaceVector;
        CurrentVelocity = workSpaceVector;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workSpaceVector = direction * velocity;
        RB.velocity = workSpaceVector;
        CurrentVelocity = workSpaceVector;
    }

    public void SetVelocityX(float velocity)
    {
        workSpaceVector.Set(velocity, CurrentVelocity.y);
        RB.velocity = workSpaceVector;
        CurrentVelocity = workSpaceVector;
    }

    public void SetVelocityY(float velocity)
    {
        workSpaceVector.Set(CurrentVelocity.x, velocity);
        RB.velocity = workSpaceVector;
        CurrentVelocity = workSpaceVector;
    }

    #endregion

    #region Check Functions
    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public  bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    #endregion

    #region Other Functions

    private void AnimationTrigger() => StateMachine.currentState.AnimationTrigger();
    private void AnimationFinishTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
    }

    #endregion
}
