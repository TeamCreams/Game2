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
    private DummyData _dummy;
    private Transform _ownerTransform = null;
    private MagneticFieldTriggerBound _magneticFieldTriggerBound = null;
    
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

        //_info.Damage, _info.ID 넣으면 참조에러 남
        if (_magneticFieldTriggerBound == null)
        {
            _magneticFieldTriggerBound = Managers.Object.Spawn<MagneticFieldTriggerBound>(Vector3.zero, 0, 100000);

            if (_magneticFieldTriggerBound != null)
            {
                _magneticFieldTriggerBound.gameObject.SetActive(true);
                _magneticFieldTriggerBound.SetBulletInfo(10);
                _magneticFieldTriggerBound.SetParent(this.gameObject.transform);
            }
        }
        else
        {
            _magneticFieldTriggerBound.gameObject.SetActive(true);
            _magneticFieldTriggerBound.SetBulletInfo(10);
            _magneticFieldTriggerBound.SetParent(this.gameObject.transform);
        }
        return true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        //Managers.Resource.Destroy(_magneticFieldTriggerBound.gameObject);
    }

    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
        _id = DataTemplateID;
        Debug.Log($"Bullet Id : {_id}");

        _info = _dummy.BulletDataDict[_id];
        _lifeTime = _info.LifeTime;
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
