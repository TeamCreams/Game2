using System.Collections;
using Data;
using UnityEngine;
using UniRx;
using System;
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
        _ownerTransform = Managers.Object.ObjectDic[ownerObjectId].transform;
    }
    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
        _id = DataTemplateID;
        Debug.Log($"Weapon Id : {_id}");
        DummyData dummy = new DummyData();
        _info = dummy.WeaponDataDict[_id];
        _creatTime = _info.CoolDown;

        //_dummyData.WeaponDataList[_id].Type;
    }
    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }

        Contexts.BattleRush.SpawnBulletEvent
        .Where(_ => _isSetting == true)
        .ThrottleFirst(TimeSpan.FromSeconds(_creatTime))
        .Subscribe(bulletId =>
        {
            this.Event_SpawnMissile(bulletId);
        })
        .AddTo(this);

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
                SpawnMissileBullets(_info);
                break;
            case WeaponData.EType.GuidedMissile:
                SpawnGuidedMissileBullets(_info);
                break;
            case WeaponData.EType.MagneticField:
                SpawnMagneticFieldBullets(_info);
                break;
            case WeaponData.EType.Laser:
                SpawnLaserBullets(_info);
                break;
        }
    }
    
}