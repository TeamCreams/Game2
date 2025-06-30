using System;
using UnityEngine;
using static Define;

public class PlayerController : BaseObject
{
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
    }

    #region Update
    private void Update_Idle()
    {
        if (Managers.Game.JoystickState == EJoystickState.PointerDown)
        {
            this.State = EPlayerState.Move;
        }
    }

    private void Update_Move()
    {
        _currentMoveSpeed = Mathf.Clamp01(Managers.Game.JoystickAmount.magnitude);
        _animator.SetFloat("MoveSpeed", Mathf.Abs(_currentMoveSpeed));
        _animator.SetFloat("MoveDirectionX", Managers.Game.JoystickAmount.x);
        _animator.SetFloat("MoveDirectionY", Managers.Game.JoystickAmount.y);
            
        Vector3 motion = new Vector3(Managers.Game.JoystickAmount.x, 0, Managers.Game.JoystickAmount.y);
        _characterController.Move(motion * Time.deltaTime);
    }

    private void Update_Die()
    {
        _animator.SetTrigger("isDie");
    }

    #endregion
}
