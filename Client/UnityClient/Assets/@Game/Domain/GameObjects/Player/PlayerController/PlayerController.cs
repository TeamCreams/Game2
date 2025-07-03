using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class PlayerController : BaseObject
{
    private Stats _stats = null;
    private Stats Stats => _stats;


    private Animator _animator;
    private CharacterController _characterController;
    public Action<EPlayerState, EPlayerState> OnChangedState;
    private EPlayerState _state = EPlayerState.Idle;
    public EPlayerState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                OnChangedState?.Invoke(_state, value);
                _state = value;
            }
        }
    }
    private List<StatModifier> _statModifier;
    private float _currentMoveSpeed;
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();

        return true;
    }

    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);

        this.Stats.StatDic[EStatType.Hp] = 100;
        this.Stats.StatDic[EStatType.MaxHp] = 100;
        this.Stats.StatDic[EStatType.Mp] = 50;
        this.Stats.StatDic[EStatType.MaxMp] = 50;
        this.Stats.StatDic[EStatType.AttackPower] = 10;
        this.Stats.StatDic[EStatType.DefensePower] = 5;
        this.Stats.StatDic[EStatType.CriticalChance] = 5;
        this.Stats.StatDic[EStatType.CriticalDamage] = 150;
        this.Stats.StatDic[EStatType.MovementSpeed] = 5; // 기본 이동속도
        this.Stats.StatDic[EStatType.AttackSpeed] = 100;
        this.Stats.StatDic[EStatType.SkillPower] = 10;
        this.Stats.StatDic[EStatType.SkillCooldownReduction] = 0;
        this.Stats.StatDic[EStatType.GoldFind] = 0;
        this.Stats.StatDic[EStatType.ExperienceGain] = 0;
    }

    private void OnDestroy()
    {

    }

    void Update()
    {
        switch (_state)
        {
            case EPlayerState.Idle:
                Update_Idle();
                break;
            case EPlayerState.Move:
                Update_Move();
                break;
            case EPlayerState.Die:
                Update_Die();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _state = EPlayerState.Die;
            Debug.Log("Q key was pressed");
        }
    }

    #region Update
    private void Update_Idle()
    {
        if (Event_UI_Joystick.JoystickState == EJoystickState.PointerDown)
        {
            this.State = EPlayerState.Move;
        }
    }

    private void Update_Move()
    {
        if (Event_UI_Joystick.JoystickState == EJoystickState.PointerUp)
        {
            this.State = EPlayerState.Idle;
        }
        _currentMoveSpeed = Mathf.Clamp01(Event_UI_Joystick.JoystickAmount.magnitude);
        _animator.SetFloat("MoveSpeed", Mathf.Abs(_currentMoveSpeed));
        _animator.SetFloat("MoveDirectionX", Event_UI_Joystick.JoystickAmount.x);
        _animator.SetFloat("MoveDirectionY", Event_UI_Joystick.JoystickAmount.y);

        Vector3 motion = new Vector3(Event_UI_Joystick.JoystickAmount.x, 0, Event_UI_Joystick.JoystickAmount.y);
        _characterController.Move(motion * Time.deltaTime * 5);
        //_characterController.Move(motion * Time.deltaTime * this._stats.StatDic[EStatType.MovementSpeed]);

        Transform animationTransform = _animator.gameObject.transform;
        Vector3 moveDirection = new Vector3(Event_UI_Joystick.JoystickAmount.x, 0, Event_UI_Joystick.JoystickAmount.y);
        if (0.1f < moveDirection.magnitude)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            animationTransform.rotation = Quaternion.Slerp(animationTransform.rotation, targetRotation, Time.deltaTime * 30f);
        }
    }

    private void Update_Die()
    {
        _animator.SetTrigger("isDie");
    }

    #endregion
}
