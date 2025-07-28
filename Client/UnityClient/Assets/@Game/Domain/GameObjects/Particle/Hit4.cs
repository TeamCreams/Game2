using System;
using UniRx;
using UnityEngine;

public class Hit4 : BaseObject
{
    Transform _ownerTransform = null;
    public override bool Init()
    {
        if (false == base.Init())
        {
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
        Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                PushParticle();
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
    }
    public void SetOwner(int id)
    {
        _ownerTransform = Managers.Object.ObjectDic[id].transform;
        this.transform.position = _ownerTransform.position;
    }
    private void PushParticle()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
