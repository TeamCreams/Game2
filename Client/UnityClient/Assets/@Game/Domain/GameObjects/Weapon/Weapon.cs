using System.Collections;
using Data;
using UnityEngine;
using UniRx;
using System;
using System.Runtime.CompilerServices;

public partial class Weapon : BaseObject
{
    Transform _ownerTransform = null;
    private float _creatTime = 3.0f;
    private int _id = 0;
    private WeaponData _info;
    private GameObject _weapon = null;
    private Vector3 _position = Vector3.zero;
    private AbilityData.EType _eType;
    private bool _isSetting = false;
    System.IDisposable _lifeTimer;
    
    //int WeaponId
    public Subject<Unit> OnSpawnBullet { get; private set; } = new Subject<Unit>();

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _weapon = Util.FindChild(this.gameObject, "WeaponObj", true);

        return true;
    }
    
    
    public void SetOwner(int ownerObjectId)
    {
        Debug.Log($"Weapon / ownerObjectId : {ownerObjectId}");
        _ownerTransform = Managers.Object.ObjectDic[ownerObjectId].transform;
        
        _isSetting = true;
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
        Observable.Interval(TimeSpan.FromSeconds(_creatTime))//뭔가 시간이 이상함?
            //.Where(_ => _isSetting == true)
            .Subscribe(_ =>
            {
                OnSpawnBullet?.OnNext(Unit.Default);
            }).AddTo(this);

        OnSpawnBullet
            .Subscribe(_ =>
            {
                Debug.Log("Shoot");
                switch (_info.Type)
                {
                    case WeaponData.EType.Missile:
                        SpawnMissileBullets();
                        break;
                    case WeaponData.EType.GuidedMissile:
                        SpawnGuidedMissileBullets();
                        break;
                    case WeaponData.EType.MagneticField:
                        SpawnMagneticFieldBullets();
                        break;
                    case WeaponData.EType.Laser:
                        SpawnLaserBullets();
                        break;
                }
            })
            .AddTo(_disposables);

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
                }
                break;
            case AbilityData.EType.Follow:
                {
                    _weapon.SetActive(true);
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

    private void SpawnWeapon()
    {
        switch (_info.Type)
        {
            case WeaponData.EType.Missile:
                SpawnMissileBullets();
                break;
            case WeaponData.EType.GuidedMissile:
                SpawnGuidedMissileBullets();
                break;
            case WeaponData.EType.MagneticField:
                SpawnMagneticFieldBullets();
                break;
            case WeaponData.EType.Laser:
                SpawnLaserBullets();
                break;
        }
    }
    
}