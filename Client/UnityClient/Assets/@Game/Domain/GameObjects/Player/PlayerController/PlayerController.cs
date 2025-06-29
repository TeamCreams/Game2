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
            case EPlayerState.Boring:
                Update_Boring();
                break;
        }
    }

    #region Update
    private void Update_Idle()
    {

    }

    private void Update_Move()
    {

    }

    private void Update_Boring()
    {
        
    }

    #endregion
}
