using Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Bullet : BaseObject
{
    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private float _speed = 10f;
    private float _lifeTime = 2f; // 총알 생존 시간 (2초)
    private float _currentTime = 0f;
    private int _id = 0;
    private BulletData _info;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }


        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody not Exist!");
            return false;
        }

        return true;
    }

    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }

        this.OnTriggerEnterAsObservable()
            .Subscribe(collider => Attack(collider))
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
        _id = DataTemplateID;
        _currentTime = 0f;
        Debug.Log($"Bullet Id : {_id}");
        DummyData dummy = new DummyData();
        _info = dummy.BulletDataDict[_id];
        _speed = _info.Speed;
        _lifeTime = _info.LifeTime;

    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    private void FixedUpdate()
    {
        MoveBullet();
        
        CheckLifeTime();
    }

    private void MoveBullet()
    {
        if (_rigidbody != null && _direction != Vector3.zero)
        {
            Vector3 newPosition = transform.position + _direction * _speed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPosition);
        }
    }

    private void CheckLifeTime()
    {
        _currentTime += Time.fixedDeltaTime;
        
        // 2초가 지나면 총알 제거
        if (_lifeTime <= _currentTime)
        {
            PushBullet(); 
        }
    }
    private void PushBullet()
    {
        Managers.Resource.Destroy(this.gameObject);
    }

    private void Attack(Collider collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            //dummyData.BulletDataList[_id].Damage;
            Managers.Resource.Destroy(this.gameObject);
        }
    }
}