using UnityEngine;
using System;
using System.Collections.Generic;
using Data;
using UniRx;
using UniRx.Triggers;
public class MagneticField : BaseObject
{
    private float _lifeTime = 2f; // 총알 생존 시간 (2초)
    private int _id = 0;
    private BulletData _info;
    private float _damage;
    DummyData _dummy;
    Transform _ownerTransform = null;

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

        return true;
    }

    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }
        Observable.Timer(TimeSpan.FromSeconds(_lifeTime))
            .Subscribe(_ =>
            {
                PushBullet();
            }).AddTo(_disposables);

        this.UpdateAsObservable()
        .Subscribe(_ =>
            {
                Move();
            }).AddTo(_disposables);

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
        Debug.Log($"Bullet Id : {_id}");

        _info = _dummy.BulletDataDict[_id];
        _lifeTime = _info.LifeTime;
        _damage = _info.Damage;
    }
    public void SetOwner(Transform ownerObject)
    {
        _ownerTransform = ownerObject;
    }

    private void PushBullet()
    {

        Managers.Resource.Destroy(this.gameObject);

    }

    private void Move()
    {
        this.transform.position = _ownerTransform.position;
    }
    
    
}
