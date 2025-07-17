using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class Player : BaseObject
{
    private Stats _stats = new Stats();
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

        Contexts.BattleRush.SpawnAbilityEvent
            .Subscribe(abilityId =>
            {
                this.Event_SpawnAbility(abilityId);
            })
            .AddTo(_disposables);
        //치트키
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    _state = EPlayerState.Die;
                    Debug.Log("Q key was pressed");
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Debug.Log("W key was pressed");
                    Contexts.BattleRush.SpawnWeapon(10101);
                }
            })
            .AddTo(_disposables);

        return true;
    }

    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }

        this.UpdateAsObservable()
            .Where(_ => _state == EPlayerState.Idle)
            .Subscribe(_ =>
            {
                //Debug.Log("Idle");

                this.Update_Idle();
            })
            .AddTo(_disposables);

        this.UpdateAsObservable()
            .Where(_ => _state == EPlayerState.Move)
            .Subscribe(_ =>
            {
                //Debug.Log("Move");

                this.Update_Move();
            })
            .AddTo(_disposables);

        this.UpdateAsObservable()
            .Where(_ => _state == EPlayerState.Die)
            .Subscribe(_ =>
            {
                this.Update_Die();
            })
            .AddTo(_disposables);

        

        this.LateUpdateAsObservable()
            .Subscribe(_ =>
            {
                if (Contexts.BattleRush.PlayerDir.sqrMagnitude < float.Epsilon)
                {
                    _state = EPlayerState.Idle;
                }
                else
                {
                    _state = EPlayerState.Move;
                }
            })
            .AddTo(_disposables);


        return true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
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


    #region Update
    private void Update_Idle()
    {
    }

    private void Update_Move()
    {
        _currentMoveSpeed = Mathf.Clamp01(Contexts.BattleRush.PlayerDir.magnitude);
        _animator.SetFloat("MoveSpeed", Mathf.Abs(_currentMoveSpeed));
        _animator.SetFloat("MoveDirectionX", Contexts.BattleRush.PlayerDir.x);
        _animator.SetFloat("MoveDirectionY", Contexts.BattleRush.PlayerDir.y);

        Vector3 motion = new Vector3(Contexts.BattleRush.PlayerDir.x, 0, Contexts.BattleRush.PlayerDir.y);
        _characterController.Move(motion * Time.deltaTime * 5);
        //_characterController.Move(motion * Time.deltaTime * this._stats.StatDic[EStatType.MovementSpeed]);

        Transform animationTransform = _animator.gameObject.transform;
        Vector3 moveDirection = new Vector3(Contexts.BattleRush.PlayerDir.x, 0, Contexts.BattleRush.PlayerDir.y);
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

    #region Event
    public void Event_SpawnAbility(int abilityId)
    {
        Debug.Log($"Event_SpawnAbility / PlayerObjectId : {Contexts.BattleRush.PlayerObjectId}");
        
        int playerObjectId = Contexts.BattleRush.PlayerObjectId.Value;
        int templateId = 10000 + (int)abilityId;
        Ability abilityObj = Managers.Object.Spawn<Ability>(this.transform.position, 0, templateId, this.transform);
        abilityObj.SetOwner(playerObjectId);
    }
    #endregion
}
