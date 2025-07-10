using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class Ability : BaseObject
{
    AbilityData _info;
    private int _id = 0;
    private DummyData _dummyData;
    
    // 무기
    private List<Weapon> _weaponList = new List<Weapon>();
    private AbilityData.EType _abilityType;
    
    // Around 변수들
    private float _rotationSpeed = 50f; 
    private float _currentRotationAngle = 0f; 
    private float _orbitRadius = 2f;
    
    // Follow 변수들
    private Queue<Vector3> _playerPositionHistory = new Queue<Vector3>(); 
    private float _historyDuration = 1.0f; 
    private float _positionUpdateInterval = 0.5f;
    private float _positionUpdateTimer = 0f;

    Transform _ownerTransform = null;
    
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _dummyData = new DummyData();

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

    public void SetOwner(int ownerObjectId)
    {
        _ownerTransform = Managers.Object.ObjectDic[ownerObjectId].transform;
    }
    
    private void OnDestroy()
    {
        _weaponList.Clear();
        _playerPositionHistory.Clear();
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

        if (!_dummyData.AbilityDataDict.ContainsKey(_id))
        {
            return;
        }
        _info = _dummyData.AbilityDataDict[_id];

        _abilityType = _info.Type;
        
        SpawnWeapon();
    }
    
    private void SpawnWeapon()
    {
        // 타입에 따른 생성 로직 분기 (C# 9 호환)
        switch (_info.Type)
        {
            case AbilityData.EType.Static:
                SpawnStaticWeapons(_info);
                break;
            case AbilityData.EType.Follow:
                SpawnFollowWeapons(_info);
                break;
            case AbilityData.EType.Around:
                SpawnAroundWeapons(_info);
                break;
        }
    }
    
    // Static
    private void SpawnStaticWeapons(AbilityData abilityData)
    {
        for (int cnt = 0; cnt < abilityData.Count; cnt++)
        {
            Vector3 spawnPosition = _ownerTransform != null ? _ownerTransform.position : transform.position;
            Weapon weaponObj = Managers.Object.Spawn<Weapon>(spawnPosition, 0, abilityData.WeaponId, this.transform);
            if (weaponObj != null)
            {
                weaponObj.SetInfo(abilityData.WeaponId);
                weaponObj.SetAbilityDataEType(abilityData.Type);
                _weaponList.Add(weaponObj);
            }
        }
    }
    
    // Follow
    private void SpawnFollowWeapons(AbilityData abilityData)
    {
        for (int cnt = 0; cnt < abilityData.Count; cnt++)
        {
            Vector3 spawnPosition = _ownerTransform != null ? _ownerTransform.position : transform.position;
            Weapon weaponObj = Managers.Object.Spawn<Weapon>(spawnPosition, 0, abilityData.WeaponId, this.transform);
            if (weaponObj != null)
            {
                weaponObj.SetInfo(abilityData.WeaponId);
                weaponObj.SetAbilityDataEType(abilityData.Type);
                _weaponList.Add(weaponObj);
            }
        }
        
        // 히스토리 초기화
        InitializePositionHistory();
    }
    
    // Around
    private void SpawnAroundWeapons(AbilityData abilityData)
    {
        float angleStep = 360f / abilityData.Count; 
        
        for (int cnt = 0; cnt < abilityData.Count; cnt++)
        {
            float angle = cnt * angleStep;
            Vector3 spawnPosition = CalculateOrbitPosition(angle);
            
            Weapon weaponObj = Managers.Object.Spawn<Weapon>(spawnPosition, 0, abilityData.WeaponId, this.transform);
            if (weaponObj != null)
            {
                weaponObj.SetInfo(abilityData.WeaponId);
                weaponObj.SetAbilityDataEType(abilityData.Type);
                _weaponList.Add(weaponObj);
            }
        }
    }
    
    private void InitializePositionHistory()
    {
        if (_ownerTransform== null) return;
        
        int historySize = Mathf.RoundToInt(_historyDuration / _positionUpdateInterval);
    
        for (int i = 0; i < historySize; i++)
        {
            _playerPositionHistory.Enqueue(_ownerTransform.position);
        }
    }
    
    private Vector3 CalculateOrbitPosition(float angle)
    {
        if (_ownerTransform == null) return transform.position;

        float radian = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * _orbitRadius;
        float z = Mathf.Sin(radian) * _orbitRadius;
        return _ownerTransform.position + new Vector3(x, 0, z);
    }
    
    private void LateUpdate() //모든 Update 함수가 호출된 후, 마지막으로 호출
    {
        switch (_abilityType)
        {
            case AbilityData.EType.Follow:
                UpdateFollowWeapons();
                break;
            case AbilityData.EType.Around:
                UpdateAroundWeapons();
                break;
        }
    }
   
    private void UpdateFollowWeapons() 
    {
        UpdatePositionHistory();
        
        // 1초 전 위치 가져오기
        Vector3 targetPosition = GetHistoryPosition();
        
        foreach (var weapon in _weaponList)
        {
            if (weapon != null)
            {
                weapon.transform.position = Vector3.Lerp(weapon.transform.position, targetPosition, Time.deltaTime * 5f);
            }
        }
    }
    
    // Around
    private void UpdateAroundWeapons()
    {
        if (_weaponList.Count == 0) return;
        
        _currentRotationAngle += _rotationSpeed * Time.deltaTime;
        if (360 <= _currentRotationAngle)
        {
            _currentRotationAngle -= 360f;
        }
        
        for (int i = 0; i < _weaponList.Count; i++)
        {
            if (_weaponList[i] == null) continue;
            
            float baseAngle = (float)(360 / _weaponList.Count) * i;
            float currentAngle = baseAngle + _currentRotationAngle;
            
            Vector3 newPosition = CalculateOrbitPosition(currentAngle);
            _weaponList[i].transform.position = newPosition;
            
            Vector3 direction = (newPosition - _ownerTransform.position).normalized;
            if (direction != Vector3.zero)
            {
                _weaponList[i].transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
    
    private void UpdatePositionHistory()
    {
        _positionUpdateTimer += Time.deltaTime;
    
        if (_positionUpdateInterval <= _positionUpdateTimer)
        {
            // 오래된 위치 제거
            if ((int)(_historyDuration / _positionUpdateInterval) <= _playerPositionHistory.Count)
            {
                _playerPositionHistory.Dequeue();
            }
            
            // 새로운 위치 추가
            _playerPositionHistory.Enqueue(_ownerTransform.position);
            
            _positionUpdateTimer = 0f;
        }
    }
    
    // 1초 전 위치 가져오기
    private Vector3 GetHistoryPosition()
    {
        if (_playerPositionHistory.Count == 0)
        {
            return _ownerTransform.position;
        }
        
        return _playerPositionHistory.Peek();
    }
    
    private void PushAbility()
    {
        _weaponList.Clear();
        _playerPositionHistory.Clear();
        Managers.Resource.Destroy(this.gameObject);
    }
}