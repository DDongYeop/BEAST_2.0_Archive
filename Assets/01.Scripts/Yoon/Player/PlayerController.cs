using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("충돌체크")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject groundEffect;

    // Components
    public Animator Animator { get; private set; }

    // PlayerComponents
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public PlayerAttack PlayerAttack { get; private set; }
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private PlayerAmmo playerAmmo;

    private bool isSturned;

    #region MainLogic

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        PlayerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = transform.Find("Collision").GetComponent<PlayerHealth>();
        playerAmmo = GetComponent<PlayerAmmo>();

        Transform visualTransform = transform.Find("Visual");
        Animator = visualTransform.GetComponent<Animator>();

        // StateMachine Setting
        StateMachine = new PlayerStateMachine();
        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            // Type 생성과 State의 생성자 매개변수인 animationBoolName으로도 사용한다.
            string typeName = stateEnum.ToString();

            // PlayerState를 상속받고 있는 클래스들을 가져와서 StateMachine에 등록시켜 준다.
            try
            {
                Type type = Type.GetType($"Player{typeName}State");
                PlayerState stateInstance = Activator.CreateInstance(type, this, StateMachine, typeName) as PlayerState;

                StateMachine.AddState(stateEnum, stateInstance); 
            }
            catch (Exception e)
            {
                Debug.LogError($"{typeName} 클래스 인스턴스를 생성할 수 없습니다. {e.Message}");
            }
        }
    }

    private void OnEnable()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);

        PlayerInput.AimingkEvent += HandleAimingEvent;
        playerHealth.OnPlayerDieEvent += DieEvent;
        playerHealth.OnDamgeEvent += SturnEvent;
    }
    
    private void OnDisable()
    {
        PlayerInput.AimingkEvent -= HandleAimingEvent;
        playerHealth.OnPlayerDieEvent -= DieEvent;
        playerHealth.OnDamgeEvent -= SturnEvent;
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    private void DieEvent()
    {
        StateMachine.ChangeState(PlayerStateEnum.Die);
    }

    private void SturnEvent()
    {
        if (false == isSturned)
        {
            isSturned = true;
            StateMachine.ChangeState(PlayerStateEnum.Sturn);
        }
    }

    public void ToInvincibleMode()
    {
        this.DelayAfterAction(3.0f, () =>
        {
            isSturned = false;
        }).Forget();
    }

    #endregion

    #region InputEvent

    private string onlyTypeWeapon = "Bow";
    public void HandleAimingEvent()
    {
        if (playerHealth.IsDead || playerAmmo.IsAmmoEmpty()) return;

        if (PlayerAttack.EquipWeaponId == onlyTypeWeapon)
        {
            StateMachine.ChangeState(PlayerStateEnum.BowAming);
        }
        else
        {
            StateMachine.ChangeState(PlayerStateEnum.Aiming);
        }
    }

    #endregion

    #region SubLogic

    public void UseAmmo()
    {
        playerAmmo.UseAmmo();
    }

    public void SetVelocity(float x)
    {
        playerMovement.SetVelocity(x);
    }

    public void FlipPlayer(float x)
    {
        playerMovement.FlipSprite(x);
    }

    public void ControllGroundEffect(bool isOn)
    {
        groundEffect.SetActive(isOn);
    }

    // 즉시 멈춤
    public void StopImmediately()
    {
        SetVelocity(0);
    }

    #endregion

    #region ETC

    // 땅에 닿았는지
    public bool IsGroundDetected
    {
        get
        {
            bool isHitGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
            return isHitGround;
        }
    }

    // Animation Event
    public void AnimationEndTrigger()
    {
        StateMachine.CurrentState.AnimationEndTrigger();
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(throwTransform.position, throwTransform.position * (throwInfo.direction * 5f));
    // }

    #endregion
}
