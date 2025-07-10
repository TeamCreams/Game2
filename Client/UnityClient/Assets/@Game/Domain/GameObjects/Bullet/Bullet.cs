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
    private DummyData _dummyData;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }


        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Bullet에 Rigidbody 컴포넌트가 없습니다!");
            return false;
        }
        _dummyData = new DummyData();

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

    private void OnDestroy()
    {

    }
    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
        _id = DataTemplateID;
        _currentTime = 0f;
        Debug.Log($"Bullet Id : {_id}");

        _speed = _dummyData.BulletDataDict[_id].Speed;
        _lifeTime = _dummyData.BulletDataDict[_id].LifeTime;

        //dummyData.BulletDataList[_id].Type;
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