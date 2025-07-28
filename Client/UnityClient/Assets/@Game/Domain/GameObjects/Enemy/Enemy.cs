using System;
using Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseObject
{
    enum State
    {
        Idle,
        Move,
        Attack,
        TakeAttack,
        Die
    }

    private float _hp = 50f;
    private Animator _animator;

    private State _state = State.Move;
    private Transform _target;
    private NavMeshAgent _navMeshAgent;

    private float _moveSpeed = 3.5f;
    private float _stoppingDistance = 5.5f;
    private bool _hasAttacked = false;
    private Flameshrower _bulletParticle = null;

    private float _attackCooldown = 3;
    private float _lastAttackTime = 0f;
    private bool _isAttackAnimationPlaying = false;

    // 자기장
    private IDisposable _magneticFieldTimer;
    private MagneticFieldTriggerBound _currentMagneticField;
    private float _magneticDamageInterval = 1f;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        return true;
    }

    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
        Debug.Log($"ObjectId : {this.ObjectId}");
        int ownerObjectId = Contexts.BattleRush.PlayerObjectId.Value;
        _target = Managers.Object.ObjectDic[ownerObjectId].transform;
    }

    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }

        if (_navMeshAgent != null)
        {
            _navMeshAgent.speed = _moveSpeed; // 이동 속도 설정
            _navMeshAgent.stoppingDistance = _stoppingDistance; // 정지 거리 설정
        }

        // 적 리스트에 추가
        Observable.NextFrame()
            .ObserveOnMainThread()
            .Subscribe(_ => Contexts.BattleRush.EnemyObjectIdList.Add(this.ObjectId));


        // 적 기술
        Observable.NextFrame()
            .ObserveOnMainThread()
            .Subscribe(_ => this.Event_SpawnAbility(20001));

        // 트리거 충돌 감지
        this.OnTriggerEnterAsObservable()
            .Subscribe(collider => Attacked(collider))
            .AddTo(_disposables);

        // 트리거 탈출 감지 (자기장용)
        this.OnTriggerExitAsObservable()
            .Subscribe(collider => OnTriggerExitMagneticField(collider))
            .AddTo(_disposables);

        // 이동 상태 업데이트
        this.UpdateAsObservable()
            .Where(_ => _state == State.Move)
            .Subscribe(_ =>
            {
                this.Update_Move();
            }).AddTo(_disposables);

        // 죽음 상태 업데이트
        this.UpdateAsObservable()
            .Where(_ => _state == State.Die)
            .Subscribe(_ =>
            {
                this.Update_Die();
            }).AddTo(_disposables);

        // 공격 상태 업데이트
        this.UpdateAsObservable()
            .Where(_ => _state == State.Attack)
            .Subscribe(_ =>
            {
                this.Update_Attack();
            }).AddTo(_disposables);

        return true;
    }

    public override void OnDespawn()
    {
        StopMagneticFieldDamage(); // 자기장 데미지 중단
        base.OnDespawn();
        Contexts.BattleRush.EnemyObjectIdList.Remove(this.ObjectId); // 적 리스트에서 제거
    }

    public void Reset()
    {
        _hp = 50f;
        _state = State.Move;
        _animator.SetBool("isDead", false);
        _animator.SetBool("isMoving", true);
    }

    #region Update
    private void Update_Idle()
    {
        // 이게 필요할지 모르겠음
        _animator.SetBool("isMoving", true);
    }

    private void Update_Move()
    {
        _navMeshAgent.SetDestination(_target.position);

        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget <= _stoppingDistance)
        {
            _state = State.Attack;
            _navMeshAgent.ResetPath();
        }
    }

    private void Update_Die()
    {
        // 아이템 드롭
        // 죽은 모션
        // 삭제
        Debug.Log("Die");
        _animator.SetTrigger("Die");
        _animator.SetBool("isDead", true);
        _animator.SetBool("isMoving", false);

        Managers.Resource.Destroy(this.gameObject);
    }

    // 장거리or단거리
    private void Update_Attack()
    {
        float timeSinceLastAttack = Time.time - _lastAttackTime;

        // 공격 실행
        if (!_hasAttacked && timeSinceLastAttack >= _attackCooldown)
        {
            _animator.SetTrigger("Attack");
            DistanceAttack(); //CloseAttack()
            _hasAttacked = true;
            _isAttackAnimationPlaying = true;
            _lastAttackTime = Time.time;
        }

        // 애니메이션이 진행 중일 때만 체크
        if (_isAttackAnimationPlaying)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack01") && 1.0f <= stateInfo.normalizedTime)
            {
                _hasAttacked = false;
                _isAttackAnimationPlaying = false;
                _state = State.Move;
            }
        }
    }

    // 장거리 단거리는 type으로 나누기
    private void CloseAttack()
    {

    }

    private void DistanceAttack()
    {
        if (_bulletParticle == null)
        {
            _bulletParticle = Managers.Object.Spawn<Flameshrower>(Vector3.zero, 0, 0, this.gameObject.transform);
            _bulletParticle.SetParents(this.gameObject.transform);
        }
        else
        {
            _bulletParticle.SetInfo(_bulletParticle.ObjectId);
        }
    }

    private void DistanceAttack<T>() where T : BaseObject
    {
        if (_bulletParticle == null)
        {
            T bulletParticle = Managers.Object.Spawn<T>(Vector3.zero, 0, 0, this.gameObject.transform);
            BulletParticle bp = bulletParticle.GetComponent<BulletParticle>();
            bp.SetParents(this.gameObject.transform);
        }
        else
        {
            _bulletParticle.SetInfo(_bulletParticle.ObjectId);
        }
    }
    #endregion

    private void Attacked(Collider collision)
    {
        float damage = 0f;
        bool isAttacked = false;

        // 총알
        // Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        // if (bullet != null)
        // {
        //     damage = bullet.Damage;
        //     isAttacked = true;
        //     Managers.Resource.Destroy(bullet.gameObject);
        // }

        //자기장
        MagneticFieldTriggerBound magneticField = collision.gameObject.GetComponent<MagneticFieldTriggerBound>();
        if (magneticField != null)
        {
            StartMagneticFieldDamage(magneticField);
            return;
        }

        // 공통 처리
        if (isAttacked)
        {
            TakeDamage(damage);
        }
    }

    private void OnTriggerExitMagneticField(Collider collision)
    {
        MagneticFieldTriggerBound magneticField = collision.gameObject.GetComponent<MagneticFieldTriggerBound>();
        if (magneticField != null && magneticField == _currentMagneticField)
        {
            StopMagneticFieldDamage();
        }
    }

    private void StartMagneticFieldDamage(MagneticFieldTriggerBound magneticField)
    {
        if (_magneticFieldTimer != null)
        {
            return;
        }

        _currentMagneticField = magneticField;
        TakeDamage(magneticField.Damage);

        _magneticFieldTimer = Observable.Interval(TimeSpan.FromSeconds(_magneticDamageInterval))
            .Subscribe(_ =>
            {
                if (_currentMagneticField != null && _currentMagneticField.gameObject != null)
                {
                    Debug.Log($"Attacked : {magneticField.Damage} ");
                    TakeDamage(_currentMagneticField.Damage);
                }
                else
                {
                    StopMagneticFieldDamage();
                }
            }).AddTo(_disposables);
    }

    private void StopMagneticFieldDamage()
    {
        if (_magneticFieldTimer != null)
        {
            _magneticFieldTimer.Dispose();
            _magneticFieldTimer = null;
            _currentMagneticField = null;
        }
    }

    private void TakeDamage(float damage)
    {
        _animator.SetTrigger("TakeDamage");
        _hp -= damage;
        Hit4 hit4 = Managers.Object.Spawn<Hit4>(Vector3.zero, 0, 10000);
        hit4.SetOwner(this.ObjectId);

        Debug.Log($"{gameObject.name} 데미지: {damage}, 남은 HP: {_hp}");

        if (_hp <= 0)
        {
            _state = State.Die;
        }
    }
    
    #region Event
    public void Event_SpawnAbility(int abilityId)
    {        
        int objectId = this.ObjectId;
        Debug.Log($"Enemy objectId : {objectId}");
        Ability abilityObj = Managers.Object.Spawn<Ability>(Vector3.zero, 0, abilityId, this.transform);
        abilityObj.SetOwner(objectId);
    }
    #endregion
}