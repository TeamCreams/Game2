using System;
using System.Collections.Generic;
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
    private BulletParticle _bulletParticle = null;
    Dictionary<WeaponData.EType, Action> _callbacks = new Dictionary<WeaponData.EType, Action>();
    private Transform _target;
    private WeaponData.EType _eType;
    private float _damage;
    DummyData _dummy;

    public float Damage
    {
        get => _damage;
    }
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        _dummy = new DummyData();

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody not Exist!");
            return false;
        }
        _callbacks[WeaponData.EType.Missile] = MoveBullet;
        _callbacks[WeaponData.EType.GuidedMissile] = MoveToTargetBullet;
        _callbacks[WeaponData.EType.MagneticField] = UpdateMagneticFieldBullets;
        _callbacks[WeaponData.EType.Laser] = UpdateLaserBullets;

        return true;
    }

    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {                
                _callbacks[_eType]?.Invoke();
            }).AddTo(_disposables);

        Observable.Timer(TimeSpan.FromSeconds(_lifeTime))
            .Subscribe(_ =>
            {
                PushBullet();
            }).AddTo(_disposables);

        // this.OnTriggerEnterAsObservable()
        //     .Subscribe(collider => Attack(collider))
        //     .AddTo(_disposables);

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
        _info = _dummy.BulletDataDict[_id];
        _speed = _info.Speed;
        _lifeTime = _info.LifeTime;
        if (_bulletParticle == null)
        {
            _bulletParticle = Managers.Object.Spawn<BulletParticle>(_dummy.BulletTypeMap[_id].Name, Vector3.zero, 0, 0, this.gameObject.transform); // 포지션이 이동을 해야하는데 안하는 듯?
            _bulletParticle.SetParents(this.gameObject.transform);
            _bulletParticle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.LookRotation(-this.transform.forward));
        }
        else
        {
            _bulletParticle.SetInfo(_bulletParticle.ObjectId);
            _bulletParticle.SetParents(this.gameObject.transform);
            _bulletParticle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.LookRotation(-this.transform.forward));
        }
        _damage = _info.Damage;
    }
    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }
    public void SetBulletType(WeaponData.EType eType)
    {
        _eType = eType;
    }
    public void SetParent(Transform parent)
    {
        this.transform.localPosition = Vector3.zero;

        this.gameObject.transform.parent = parent;
    }

    // public void SetParticle()
    // {
    //     var a = _dummy.BulletTypeMap[_id];
    //     if (_bulletParticle == null)
    //     {
    //         a bulletParticle = Managers.Object.Spawn<a>(Vector3.zero, 0, 0, this.gameObject.transform);
    //         BulletParticle bp = bulletParticle.GetComponent<BulletParticle>();
    //         bp.SetParents(this.gameObject.transform);
    //     }
    //     else
    //     {
    //         _bulletParticle.SetInfo(_bulletParticle.ObjectId);
    //     }
    // }

    public void MoveBullet()
    {
        if (_rigidbody != null && _direction != Vector3.zero)
        {
            Vector3 newPosition = transform.position + _direction * _speed * Time.fixedDeltaTime;
            this.transform.LookAt(newPosition);
            _rigidbody.MovePosition(newPosition);
        }
    }
    public void MoveToTargetBullet()
    {
        if (_rigidbody == null)
        {
            return;
        }
        
        Vector3 directionToTarget = (_target.position - transform.position).normalized;
        Vector3 newPosition = transform.position + directionToTarget * _speed * Time.fixedDeltaTime;
        this.transform.LookAt(newPosition);
        _rigidbody.MovePosition(newPosition);
    }
    private void UpdateLaserBullets()
    {
        Debug.Log("UpdateLaserBullets");

        //Contexts.BattleRush.ScreenHeight.Value
        //Contexts.BattleRush.ScreenWidth.Value
    }

    private void UpdateMagneticFieldBullets()
    {
        Debug.Log("UpdateMagneticFieldBullets");
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

    // private void Attack(Collider collision)
    // {
    //     if (collision.gameObject.GetComponent<Enemy>() != null)
    //     {
    //         //dummyData.BulletDataList[_id].Damage;
    //         Managers.Resource.Destroy(this.gameObject);
    //     }
    // }
}