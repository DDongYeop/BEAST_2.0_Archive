using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool isEditor = true;

    // event
    public Action AimingkEvent;

    // Movement
    private float xInput;
    public float XInput => xInput;
    public bool IsMoveInputIn
    {
        get
        {
            return Mathf.Abs(xInput) > Mathf.Epsilon;
        }
    }

    // Attack
    [SerializeField] private float touchCoolTime = 0.75f;
    private float clickPassedTime => (Time.time - attackTime);
    private float attackTime;
    public bool IsCanAttack => (clickPassedTime >= touchCoolTime);
    public bool IsThrowReady { get; set; }

    public bool IsActivate { get; set; } = true;

    [Header("InputUI")]
    [SerializeField] private FixedJoystick throwJoystick;
    [SerializeField] UI_EventHandler leftMoveUIHandler;
    [SerializeField] UI_EventHandler rightMoveUIHandler;

    private void OnEnable()
    {
        throwJoystick.OnJoystickDownHandler += OnJoystickDownEvent;
        throwJoystick.OnJoystickUpHandler += OnJoystickUpEvent;

        leftMoveUIHandler.OnDownHandler += OnLeftMovePressEvent;
        rightMoveUIHandler.OnDownHandler += OnRightMovePressEvent;

        leftMoveUIHandler.OnUpHandler += OnStopMoveUpEvent;
        rightMoveUIHandler.OnUpHandler += OnStopMoveUpEvent;
    }
    
    // FOR_PC_TEST
    private void Update()
    {
        MoveInput();
    }

    private void OnDisable()
    {
        throwJoystick.OnJoystickDownHandler -= OnJoystickDownEvent;
        throwJoystick.OnJoystickUpHandler -= OnJoystickUpEvent;

        leftMoveUIHandler.OnDownHandler -= OnLeftMovePressEvent;
        rightMoveUIHandler.OnDownHandler -= OnRightMovePressEvent;

        leftMoveUIHandler.OnUpHandler -= OnStopMoveUpEvent;
        rightMoveUIHandler.OnUpHandler -= OnStopMoveUpEvent;
    }

    private void MoveInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    #region NEW_MOBILE_INPUT

    public void OnLeftMovePressEvent(PointerEventData data, Transform transform)
    {
        xInput = -1;
    }

    public void OnRightMovePressEvent(PointerEventData data, Transform transform)
    {
        xInput = 1;
    }

    public void OnStopMoveUpEvent(PointerEventData data, Transform transform)
    {
        xInput = 0;
    }

    public void OnJoystickDownEvent()
    {
        if (IsCanAttack)
        {
            IsThrowReady = true;
            AimingkEvent?.Invoke();
        }
    }

    public void OnJoystickUpEvent()
    {
        if (IsThrowReady)
        {
            IsThrowReady = false;
            attackTime = Time.time;
        }
    }

    #endregion

    public Vector2 GetDragDirection()
    {
        return throwJoystick.Direction;
    }

}
