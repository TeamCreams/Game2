using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UniRx;
using System;
using UniRx.Triggers;

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
    
    Dictionary<AbilityData.EType, Action> _callbacks = new Dictionary<AbilityData.EType, Action>();
    
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        _callbacks[AbilityData.EType.Follow] = UpdateFollowWeapons;
        _callbacks[AbilityData.EType.Around] = UpdateAroundWeapons;
        _callbacks[AbilityData.EType.Static] = null;
        
        return true;
    }

    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }
        
        // this.UpdateAsObservable()
        //     .Subscribe(_ =>
        //     {
        //         if (_info == null)
        //             return;
                
        //         _callbacks[_info.Type]?.Invoke();
        //     }).AddTo(_disposables);

        
        return true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        _weaponList.Clear();
        _playerPositionHistory.Clear();
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