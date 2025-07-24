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
    private float _hp = 0f;
    private Animator _animator;

    private State _state = State.Move;
    private Transform _target;
    private NavMeshAgent _navMeshAgent;
    
    private float _moveSpeed = 3.5f;
    private float _stoppingDistance = 1.5f;
    private bool _hasAttacked = false;
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
        int ownerObjectId = Contexts.BattleRush.PlayerObjectId.Value;
        Debug.Log($"ownerObjectId : {ownerObjectId}");
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
            _navMeshAgent.speed = _moveSpeed;
            _navMeshAgent.stoppingDistance = _stoppingDistance;
        }

        Contexts.BattleRush.EnemyObjectIdList.Add(this.ObjectId);
        this.OnTriggerEnterAsObservable()
            .Subscribe(collider => Attacked(collider))
            .AddTo(_disposables);

        this.UpdateAsObservable()
            .Where(_ => _state == State.Move)
            .Subscribe(_ =>
            {
                // moving
                this.Update_Move();
            }).AddTo(_disposables);

        this.UpdateAsObservable()
            .Where(_ => _state == State.Die)
            .Subscribe(_ =>
            {
                this.Update_Die();
            }).AddTo(_disposables);

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
        base.OnDespawn();
        Contexts.BattleRush.EnemyObjectIdList.Remove(this.ObjectId);
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
        if (!_hasAttacked)
        {
            _animator.SetTrigger("Attack");
            _hasAttacked = true; 
        }
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack01") && 1.0f <= stateInfo.normalizedTime)
        {
            _hasAttacked = false;
            _state = State.Move;
        }
    }

    #endregion
    private void Attacked(Collider collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            _animator.SetTrigger("TakeDamage");
            float _damage = collision.gameObject.GetComponent<Bullet>().Damage;
            // bullet 삭제
            // 데미지 처리
            _hp -= _damage;
            if (_hp <= 0)
            {
                _state = State.Die;
            }
        }
    }

}
