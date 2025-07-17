using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UniRx;
using System;

public partial class Ability : BaseObject
{
    AbilityData _info;
    private int _id = 0;
    
    // 무기
    private List<Weapon> _weaponList = new List<Weapon>();
    
    // Around 변수들
    private float _rotationSpeed = 50f; 
    private float _currentRotationAngle = 0f; 
    private float _orbitRadius = 2f;
    
    // Follow 변수들
    private Queue<Vector3> _playerPositionHistory = new Queue<Vector3>(); 
    private float _historyDuration = 1.0f; 
    private float _positionUpdateInterval = 0.5f;
    private float _positionUpdateTimer = 0f;
    System.IDisposable _lifeTimer;

    Transform _ownerTransform = null;
    private int _ownerObjectId = 0;
    
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        // Contexts.BattleRush.SpawnWeaponEvent
        //     .Subscribe(weaponId =>
        //     {
        //         Debug.Log("SpawnWeapon");
        //         SpawnWeapon();
        //     })
        //     .AddTo(_disposables);
        _lifeTimer?.Dispose();

        _lifeTimer = Observable.Interval(TimeSpan.FromSeconds(0.3f)) //뭔가 시간이 이상함?
            .Subscribe(_ =>
            {
                Debug.Log("UpdateFollowWeapons");
                if (_info == null)
                    return;
                switch (_info.Type)
                    {
                        case AbilityData.EType.Follow:
                            UpdateFollowWeapons();
                            break;
                        case AbilityData.EType.Around:
                            UpdateAroundWeapons();
                            break;
                    }
            }).AddTo(this.gameObject); //_disposable로 하면 안됨

        return true;
    }

    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }
        
        return true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        _weaponList.Clear();
        _playerPositionHistory.Clear();
        // 타이머 중지 및 리소스 정리
        _lifeTimer?.Dispose();
        _lifeTimer = null;
    }

    public void SetOwner(int ownerObjectId)
    {
        Debug.Log($"ownerObjectId : {ownerObjectId}");
        _ownerObjectId = ownerObjectId;
        _ownerTransform = Managers.Object.ObjectDic[ownerObjectId].transform;
        
        SpawnWeapon();
    }

    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
        _id = DataTemplateID;
        Debug.Log($"Ability Id : {_id}");

        if (_id <= 0)
        {
            return;
        }
        DummyData dummy = new DummyData();
        _info =  dummy.AbilityDataDict[_id];
    }
    
    private void SpawnWeapon()
    {
        switch (_info.Type)
        {
            case AbilityData.EType.Static:
                SpawnStaticWeapons();
                break;
            case AbilityData.EType.Follow:
                SpawnFollowWeapons();
                break;
            case AbilityData.EType.Around:
                SpawnAroundWeapons();
                break;
        }
    }
    
    // private void LateUpdate() //모든 Update 함수가 호출된 후, 마지막으로 호출
    // {
    //     switch (_info.Type)
    //     {
    //         case AbilityData.EType.Follow:
    //             UpdateFollowWeapons();
    //             break;
    //         case AbilityData.EType.Around:
    //             UpdateAroundWeapons();
    //             break;
    //     }
    // }
    
}