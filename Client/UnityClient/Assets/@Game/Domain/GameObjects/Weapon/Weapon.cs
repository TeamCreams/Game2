using System.Collections;
using Data;
using UnityEngine;
using UniRx;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UniRx.Triggers;

public partial class Weapon : BaseObject
{
    Transform _ownerTransform = null;
    private float _creatTime = 3.0f;
    private int _id = 0;
    private WeaponData _info;
    private GameObject _weapon = null;
    private Vector3 _position = Vector3.zero;
    private AbilityData.EType _eType;
    System.IDisposable _lifeTimer;
    
    //int WeaponId
    public Subject<Unit> OnSpawnBullet { get; private set; } = new Subject<Unit>();
    Dictionary<WeaponData.EType, Action> _callbacks = new Dictionary<WeaponData.EType, Action>();

    private Action _action;
    private int _index;
    private int _allCount;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _weapon = Util.FindChild(this.gameObject, "WeaponObj", true);
        _callbacks[WeaponData.EType.Missile] = SpawnMissileBullets;
        _callbacks[WeaponData.EType.GuidedMissile] = SpawnGuidedMissileBullets;
        _callbacks[WeaponData.EType.MagneticField] = SpawnMagneticFieldBullets;
        _callbacks[WeaponData.EType.Laser] = SpawnLaserBullets;

        return true;
    }
    
    
    public void SetOwner(int ownerObjectId)
    {
        Debug.Log($"Weapon / ownerObjectId : {ownerObjectId}");
        _ownerTransform = Managers.Object.ObjectDic[ownerObjectId].transform;
    }
    public void SetParent(Transform parent)
    {
        this.transform.parent = parent;
    }
    public void SetIndex(int index, int allCount)
    {
        _index = index;
        _allCount = allCount;
    }
    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
        _id = DataTemplateID;
        Debug.Log($"Weapon Id : {_id}");
        DummyData dummy = new DummyData();
        _info = dummy.WeaponDataDict[_id];
        _creatTime = _info.CoolDown;
        Debug.Log("SetInfo");

        //_dummyData.WeaponDataList[_id].Type;
    }
    public override bool OnSpawn()
    {
        Debug.Log("OnSpawn");
        if (false == base.OnSpawn())
        {
            return false;
        }
        Observable.Interval(TimeSpan.FromSeconds(_creatTime))
            .Subscribe(_ =>
            {
                OnSpawnBullet?.OnNext(Unit.Default);
            }).AddTo(this);

        OnSpawnBullet
            .Subscribe(_ =>
            {
                Debug.Log("Shoot");
                if (_info == null)
                    return;
                
                _callbacks[_info.Type]?.Invoke();
            })
            .AddTo(_disposables);

            this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // moving
                _action?.Invoke();
            }).AddTo(_disposables);

        return true;
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
    }
    public void SetAbilityDataEType(AbilityData.EType eType)
    {
        _eType = eType;
        switch (_eType)
        {
            case AbilityData.EType.Static:
                {
                    _weapon.SetActive(false);
                }
                break;
            case AbilityData.EType.Around:
                {
                    _weapon.SetActive(true);
                    _action = UpdateAroundWeapon;
                }
                break;
            case AbilityData.EType.Follow:
                {
                    _weapon.SetActive(true);
                    _action = UpdateFollowWeapons;
                }
                break;
        }
    }
    public void SetWeaponPosition()
    {
        switch (_eType)
        {
            case AbilityData.EType.Static:
                {
                    _position = _ownerTransform.position;
                }
                break;
            case AbilityData.EType.Around:
                {
                    _position = _weapon.transform.position;
                }
                break;
            case AbilityData.EType.Follow:
                {
                    _position = _weapon.transform.position;
                }
                break;
        }
    }
    
}