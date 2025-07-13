using System.Collections;
using Data;
using UnityEngine;
using UniRx;
using System;
public class Weapon : BaseObject
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
        //(_isSetting == true)
        .ThrottleFirst(TimeSpan.FromSeconds(_creatTime))
        .Subscribe(bulletId =>
        {
            this.Event_SpawnBullet(bulletId);
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
    private void Event_SpawnBullet(int bulletId)
    {
        // 시간마다
        // 개수
        // 플레이어를 중심으로 5개부터 시작해서 최대 20개까지
        SetWeaponPosition(); // 위치 조정
        int maxCount = _info.Count;
        float angleStep = 360f / maxCount;
        for (int cnt = 0; cnt < maxCount; cnt++)
        {
            float currentAngle = cnt * angleStep;

            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(
                Mathf.Cos(radians),
                0f,
                Mathf.Sin(radians)
            );

            Bullet bulletObj = Managers.Object.Spawn<Bullet>(_position, 0, _info.BulletId);

            if (bulletObj != null)
            {
                bulletObj.SetInfo(_info.BulletId);
                bulletObj.SetDirection(direction);
            }
        }
    }

    private void SpawnWeapon()
    {
        // 타입에 따른 생성 로직 분기 (C# 9 호환)
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

    private void SpawnMissileBullets(WeaponData weaponData)
    {
        _isSetting = true;
        // 피젯스피너에서 나오는 총알은 lifetime을 짧게
        Event_SpawnBullet(weaponData.BulletId);
    }
    private void SpawnGuidedMissileBullets(WeaponData weaponData)
    {
        _isSetting = true;
        // 인근에 있는 적 transform을 추적.
        Transform target = Managers.Enemy.GetNearestEnemy(_ownerTransform.position);
        //target을 총알에 있는 스크립트로 넘기기
    }
    private void SpawnMagneticFieldBullets(WeaponData weaponData)
    {
        _isSetting = true;
        // 총알은 lifetime을 짧게
    }
    private void SpawnLaserBullets(WeaponData weaponData)
    {
        _isSetting = true;

    }
    

}